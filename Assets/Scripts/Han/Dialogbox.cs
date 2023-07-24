using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;

public class Dialogbox : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public Button ExitButton;
    public Button SubExitButton;
    public GameObject SubButtonPlane;
    public GameObject SubButtonObjcet;

    public DIALOG_TYPE dialogType;

    public int WaveCnt = 0; //現在のwave index
    public int WaveMaxNum = 0;//総wave数
    public Text WaveUIText;

    //public string WaveCnt;
    public List<string> SubButtonTextList;


    public UnityEvent OnOpenEvent;
    public UnityEvent OnCloseEvent;

    [System.Serializable]
    public class OpenWindowEvent:UnityEvent<DIALOG_TYPE,Button> { }

    public OpenWindowEvent OnSubWindowEvent;

    public Slider VolumeSlider;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。

    #endregion

    #region Constant
    // 定数をいれる。
    public enum DIALOG_TYPE
    {
        TYPE_MAIN_MENU,
        TYPE_OPTION
    }
    #endregion

    #region Event
    //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #endregion

    #region unity methods
    //  Start, UpdateなどのUnityのイベント関数。
    private void Awake()
    {
        if (dialogType == DIALOG_TYPE.TYPE_MAIN_MENU)
        {
            ExitButton.onClick.AddListener(() => { SetActive(false); });
            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.WaveCount.Subscribe(count => { WaveCnt = count; });//現在のwave index
                WaveManager.Instance.WaveEnemyCount.Subscribe(allCnt => { WaveMaxNum = allCnt; });//総wave数
            }

            StartCoroutine(UpdateWaveInfo());
        }else
        {
            SubExitButton.onClick.AddListener(() => { SetActive(false); });
        }
    }

    private void Start()
    {
        if (dialogType == DIALOG_TYPE.TYPE_MAIN_MENU)
        {
            const int fontSize = 96;

            for (int bCnt = 0; bCnt < SubButtonTextList.Count; bCnt++)
            {
                GameObject buttonObj = Instantiate(SubButtonObjcet);
                buttonObj.GetComponentInChildren<Text>().text = SubButtonTextList[bCnt];
                buttonObj.GetComponentInChildren<Text>().fontSize = fontSize;
                buttonObj.transform.SetParent(SubButtonPlane.transform, false);
                int n = bCnt;
                Button button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() => SubButtonEvent(n, button));

                if(SubButtonTextList[bCnt] == "オプション")
                {
                    UIManager.Instance.OptionMenuButton = button;
                }
            }
        }
    }

    private void Update()
    {
        if (dialogType == DIALOG_TYPE.TYPE_MAIN_MENU)
            UpdateText(WaveUIText, WaveCnt + "/" + WaveMaxNum);
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public void SetActive(bool onoff,bool fromExit = true)
    {
        //gameObject.transform.parent.gameObject.SetActive(onoff);
        //UpdateText(WaveUIText, WaveCnt + "/" + WaveMaxNum);
        if (onoff)
        {
            Open(fromExit);
            return;
        }
        Close(fromExit);
    }

    public void Open(bool fromExit = true)
    {
        Debug.Log("Open");
        GameManager.Instance.TimerStop();
        if (VolumeSlider != null) VolumeSlider.value = AudioPlayer.Instance.BGMVolume;
        if (dialogType == DIALOG_TYPE.TYPE_MAIN_MENU)
        {
            if (fromExit == true)
            {
                transform.parent.gameObject.SetActive(true);
                gameObject.SetActive(true);

                OnOpenEvent.Invoke();
            }
        }else
        {
            
            gameObject.SetActive(true);
        }
    }

    public void Close(bool fromExit = true)
    {
        Debug.Log("Close");
        GameManager.Instance.TimerStart();
        if (dialogType == DIALOG_TYPE.TYPE_MAIN_MENU)
        {
            if (fromExit == true)
            {
                transform.parent.gameObject.SetActive(false);
                gameObject.SetActive(false);
                OnCloseEvent.Invoke();
            }else
            { 
            }
        }else
        {
            Debug.Log(dialogType);
            gameObject.SetActive(false);
            OnCloseEvent.Invoke();
        }
    }

    public void SubButtonEvent(int buttonCnt,Button button)
    {
        Debug.Log(buttonCnt);

        switch(buttonCnt)
        {
            case 0:
                Close();
                break;
            case 1:
                //UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
                Close();
                GameManager.Instance.ReturnTitle();
                break;
            case 2:
                OnSubWindowEvent.Invoke(DIALOG_TYPE.TYPE_OPTION, button);
                break;
            default:
                break;
        }
    }

    public void OnChangeVolume()
    {
        AudioPlayer.Instance.BGMVolume = VolumeSlider.value;
        AudioPlayer.Instance.SEVolume = VolumeSlider.value;
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    IEnumerator UpdateWaveInfo()
    {
        while (true)
        {
            UpdateText(WaveUIText, WaveCnt + "/" + WaveMaxNum);

            yield return new WaitForSeconds(.1f);
        }
    }


    private void UpdateText(Text text, object str)
    {
        text.text = (string)str;
    }
    #endregion
}
