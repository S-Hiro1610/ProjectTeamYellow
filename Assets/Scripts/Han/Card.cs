using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Card : CardBase
{
    #region property
    // プロパティを入れる。
    
    public Text LVUIText;
    public Text costUIText;

    public Image coolTimePlane;
    public ReactiveProperty<SELSCT_MODE> SelectMode => selectMode;

    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    private ReactiveProperty<SELSCT_MODE> selectMode  = new ReactiveProperty<SELSCT_MODE>(SELSCT_MODE.SELECT_MOD_NO);
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

    public void OnClick()
    {

        if (selectMode.Value == SELSCT_MODE.SELECT_MOD_NO)
        {
            selectMode.Value = SELSCT_MODE.SELECT_MOD_SELECT;
        }
        //Debug.Log("Name=>"+gameObject.name+",LV=>"+ LVUIText.text+",COST=>"+costUIText.text);

    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
