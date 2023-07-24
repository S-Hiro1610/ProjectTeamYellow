using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleWindow : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    // Audio 再生可否
    private bool _isBGMPlayable = false;
    private Slider _volumeSlider;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion
    #region unity method
    // Start is called before the first frame update
    void Start()
    {
        // タイトル画面のＢＧＭ処理
        _isBGMPlayable = false;
        if (AudioPlayer.Instance == null)
        {
            Debug.Log("Audio Player is not found!");
        }
        else if (AudioPlayer.Instance.BgmList.Count <= 0)
        {
            Debug.Log("Title BGM is not found!");
        }
        else
        {
            // タイトル画面用ＢＧＭのスタート
            _isBGMPlayable = true;
            _volumeSlider = transform.Find("Panel/Slider").GetComponent<Slider>();
            _volumeSlider.value = AudioPlayer.Instance.BGMVolume;
            AudioPlayer.Instance.BGMPlay("BGM_02");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0))
            StartButton();
    }

    //
    private void OnEnable()
    {
        if(_isBGMPlayable)
        {
            // タイトル画面用ＢＧＭのスタート
            _volumeSlider = transform.Find("Panel/Slider").GetComponent<Slider>();
            _volumeSlider.value = AudioPlayer.Instance.BGMVolume;
            AudioPlayer.Instance.BGMPlay("BGM_02");
        }
    }
    #endregion

    #region public
    public void StartButton()
    {
        if (_isBGMPlayable)
        {
            AudioPlayer.Instance.BGMStop();
        }
        if (GameManager.Instance == null)
        {
            Debug.Log("GameManager Instance Not Found at GameExitButton.Click()");
            return;
        }
        GameManager.Instance.StartGame();
    }

    public void ExitButton()
    {
        Debug.Log("Exit Game!");
        if (_isBGMPlayable)
        {
            AudioPlayer.Instance.BGMStop();
        }
        Application.Quit();
    }

    public void OnChangeVolume()
    {
        AudioPlayer.Instance.BGMVolume = _volumeSlider.value;
        AudioPlayer.Instance.SEVolume = _volumeSlider.value;
    }
    #endregion

    #region private method
    #endregion
}
