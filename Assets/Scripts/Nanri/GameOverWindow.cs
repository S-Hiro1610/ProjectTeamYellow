using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void RestartButton()
    {
        Debug.Log("Restart Game!");
        if (GameManager.Instance == null)
        {
            Debug.Log("GameManager Instance Not Found at GameOverWindow.RestartButton()");
            return;
        }
        GameManager.Instance.StartGame();
    }

    public void ReturnTitleButton()
    {
        Debug.Log("Return Title!");
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
