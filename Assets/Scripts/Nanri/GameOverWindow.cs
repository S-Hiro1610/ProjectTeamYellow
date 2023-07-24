using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameOverWindow : MonoBehaviour
{
    #region property
    public int SeIndex => _seIndex;
    #endregion

    #region serialize
    [SerializeField]
    protected int _seIndex;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion
    #region unity method
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnChangeTitle.Subscribe(_ => gameObject.SetActive(false));
        GameManager.Instance.OnChangeInGame.Subscribe(_ => gameObject.SetActive(false));
        GameManager.Instance.OnChangeGameOver.Subscribe(_ => gameObject.SetActive(true));
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //
    private void OnEnable()
    {
        if (AudioPlayer.Instance == null)
        {
            Debug.Log("Audio Player is not found!");
        }
        else
        {
            // ゲームオーバージングルを一回流す
            AudioPlayer.Instance.SEPlay(_seIndex);
        }
    }
    #endregion

    #region public
    /// <summary>
    /// はじめからやりなおすボタンクリック時の処理（ボタン廃止）
    /// </summary>
    //public void RestartButton()
    //{
    //    Debug.Log("Restart Game!");
    //    if (GameManager.Instance == null)
    //    {
    //        Debug.Log("GameManager Instance Not Found at GameOverWindow.RestartButton()");
    //        return;
    //    }
    //    GameManager.Instance.StartGame();
    //}

    /// <summary>
    /// タイトルにもどるボタンクリック時の処理
    /// </summary>
    public void ReturnTitleButton()
    {
        if (AudioPlayer.Instance == null)
        {
            Debug.Log("Audio Player is not found!");
        }
        else
        {
            // ゲームオーバージングルを一回流す
            AudioPlayer.Instance.SEStop();
        }

        //Debug.Log("Return Title!");
        if (GameManager.Instance == null)
        {
            Debug.Log("GameManager Instance Not Found at GameOverWindow.ReturnTitleButton()");
            return;
        }
        GameManager.Instance.ReturnTitle();
    }
    #endregion

    #region private method
    #endregion
}
