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
    public GameObject SubButtonPlane;
    public GameObject SubButtonObjcet;

    public int WaveCnt = 0; //現在のwave index
    public int WaveMaxNum = 0;//総wave数
    public Text WaveUIText;

    //public string WaveCnt;
    public List<string> SubButtonTextList;


    public UnityEvent OnOpenEvent;
    public UnityEvent OnCloseEvent;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
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
        ExitButton.onClick.AddListener(() => { SetActive(false); });
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.WaveCount.Subscribe(count => { WaveCnt = count; });//現在のwave index
            WaveManager.Instance.WaveEnemyCount.Subscribe(allCnt => { WaveMaxNum = allCnt; });//総wave数
        }

        StartCoroutine(UpdateWaveInfo());
    }

    private void Start()
    {
        const int fontSize = 96;

        for(int bCnt = 0;bCnt< SubButtonTextList.Count; bCnt++)
        {
            GameObject buttonObj = Instantiate(SubButtonObjcet);
            buttonObj.GetComponentInChildren<Text>().text = SubButtonTextList[bCnt];
            buttonObj.GetComponentInChildren<Text>().fontSize = fontSize;
            buttonObj.transform.SetParent(SubButtonPlane.transform,false);
            int n = bCnt;
            buttonObj.GetComponent<Button>().onClick.AddListener(()=>SubButtonEvent(n)) ;
        }
    }

    private void Update()
    {
        UpdateText(WaveUIText, WaveCnt + "/" + WaveMaxNum);
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public void SetActive(bool onoff)
    {
        //gameObject.transform.parent.gameObject.SetActive(onoff);
        //UpdateText(WaveUIText, WaveCnt + "/" + WaveMaxNum);
        if (onoff)
        {
            Open();
            return;
        }
        Close();
    }

    public void Open()
    {
        Debug.Log("Open");
        gameObject.transform.parent.gameObject.SetActive(true);
        
        OnOpenEvent.Invoke();
    }

    public void Close()
    {
        Debug.Log("Close");
        gameObject.transform.parent.gameObject.SetActive(false);
        OnCloseEvent.Invoke();
    }

    public void SubButtonEvent(int buttonCnt)
    {
        Debug.Log(buttonCnt);

        switch(buttonCnt)
        {
            case 0:
                Close();
                break;
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
                break;
            case 3:
                break;
            default:
                break;
        }
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
