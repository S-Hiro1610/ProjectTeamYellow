using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardInfo
{
    #region property
    // プロパティを入れる。
    public SELSCT_MODE selectMode;
    public GameObject thisGameObjcet;
    public Card cardContext;

    public float coolTime;
    public string LVUIString;
    public string costUIString;
    #endregion

}

public enum SELSCT_MODE
{
    SELECT_MOD_NO,
    SELECT_MOD_SELECT
}
public abstract class CardBase : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    //public event Action<CardBase> OnCardClicked;
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
    
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
