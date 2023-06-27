using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class CharactorBase : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public int _hp;
    public int _power;

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
    public virtual void Attack(GameObject target)
    {
        
    }

    public virtual void Damaged(int power)
    {
        Debug.Log("Damaged");
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
