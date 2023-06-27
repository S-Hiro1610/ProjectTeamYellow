﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitController : CharactorBase
{
    #region property
    // プロパティを入れる
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
        if (_isCanAttack && _attackCollider.IsTarget)
        {
            Attack(_attackCollider.Target);
        }
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public override void Attack(CharactorBase target)
    {
        base.Attack(target);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}