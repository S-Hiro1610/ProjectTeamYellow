using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BaseManager
{
    #region property
    // プロパティを入れる。
    //TopPanelText
    public Text PauseMenuUIText;
    public Text WaveUIText;
    public Text EnemyCntUIText;
    //BottomPlaneText
    public Text PowerUIText;
    //Unit_Cards_PanelText(TODO)

    //TopPanelText
    public string PauseMenuUIString;
    public string WaveCnt;
    public string EnemyCntUICnt;
    //BottomPlaneText
    public string PowerUICnt;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    //TopPanelText
    private string _currentPauseMenuUIString;
    private string _currentWaveString;
    private string _currentEnemyCntUIString;
    //BottomPlaneText
    private string _currentPowerUIString;
    #endregion

    #region Constant
    // 定数をいれる。
    #endregion

    #region Event
    //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #endregion

    #region unity methods
    //  Start, UpdateなどのUnityのイベント関数。
    private void Awake()
    {
        base.Initialize();
    }

    private void Start()
    {
        _currentPauseMenuUIString = PauseMenuUIText.text;
        UpdateText(PauseMenuUIText, PauseMenuUIString);

        _currentWaveString = WaveUIText.text;
        UpdateText(WaveUIText, WaveCnt);

        _currentEnemyCntUIString = EnemyCntUIText.text;
        UpdateText(EnemyCntUIText, EnemyCntUICnt);

        _currentPowerUIString = PowerUIText.text;
        UpdateText(PowerUIText, PowerUICnt);

        StartCoroutine(CheckValuesChanged());
    }

    private void Update()
    {

    }
    #endregion

    #region public method
    // 自身で作成したPublicな関数を入れる。
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。

    private IEnumerator CheckValuesChanged()
    {
        while(true)
        {
            string newPauseMenuUIString = PauseMenuUIString;
            if (newPauseMenuUIString != _currentPauseMenuUIString)
            {
                _currentPauseMenuUIString = newPauseMenuUIString;
                UpdateText(PauseMenuUIText, _currentPauseMenuUIString);
            }

            string newWaveString = WaveCnt;
            if (newWaveString != _currentWaveString)
            {
                _currentWaveString = newWaveString;
                UpdateText(WaveUIText, _currentWaveString);
            }

            string newEnemyCntUIString = EnemyCntUICnt;
            if (newEnemyCntUIString != _currentEnemyCntUIString)
            {
                _currentEnemyCntUIString = newEnemyCntUIString;
                UpdateText(EnemyCntUIText, _currentEnemyCntUIString);
            }

            string newPowerUIString = PowerUICnt;
            if (newPowerUIString != _currentPowerUIString)
            {
                _currentPowerUIString = newPowerUIString;
                UpdateText(PowerUIText, _currentPowerUIString);
            }

            yield return new WaitForSeconds(.1f);//呼び出しを頻繁し過ぎないように
        }
    }

    private void UpdateText(Text text,object str)
    {
        text.text = (string)str;
    }
    #endregion
}
