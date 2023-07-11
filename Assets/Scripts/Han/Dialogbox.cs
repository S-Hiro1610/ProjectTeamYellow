using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialogbox : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public Button ExitButton;

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
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public void SetActive(bool onoff)
    {
        //gameObject.transform.parent.gameObject.SetActive(onoff);

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
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
