﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UniRx;

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
    private EnemyMove _enemyMove;
    // 画面停止中のフラグ
    private bool _stopflag = false;
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
        // エネミーの場合はEnemyMoveコンポーネントを取得
        if (transform.tag == _enemyTag) _enemyMove = GetComponent<EnemyMove>();
    }

    private void Start()
    {
        GameManager.Instance.OnChangeInitialize.Subscribe(_ => UnitObjectPool.Instance.ReleaseGameObject(gameObject));
        GameManager.Instance.OnStop.Subscribe(_ => _stopflag = true);
        GameManager.Instance.OnStart.Subscribe(_ => _stopflag = false);
    }

    private void Update()
    {
        // HPバーの向きをカメラ方向に固定
        SetRotationHPBarUI();

        if(!_stopflag)
        {
            // ターゲットが非アクティブである場合は初期化を行う
            if (_attackCollider.Target != null)
            {
                if (!_attackCollider.Target.gameObject.activeSelf)
                {
                    _attackCollider.TargetUpdate(_attackCollider.Target);
                    if (transform.tag == _enemyTag) _enemyMove.MoveSet(true);
                }
            }

            if (_isCanAttack && _attackCollider.IsTarget)
            {
                // エネミーは攻撃中に足を止める
                if (transform.tag == _enemyTag) _enemyMove.MoveSet(false);

                if (_attackCollider.Target != null)
                {
                    StartCoroutine(Attack(_attackCollider.Target));
                }
                else
                {
                    if (transform.tag == _enemyTag) _enemyMove.MoveSet(true);
                }
            }
        }
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public void UnitInitilize(int level)
    {
        // パラメータの設定
        SetParameter(level);
        // ターゲットの初期化
        _attackCollider.Initilized();
        // エネミーの場合はEnemyMoveのフラグをtrue
        if (_enemyMove != null) _enemyMove.MoveSet(true);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}