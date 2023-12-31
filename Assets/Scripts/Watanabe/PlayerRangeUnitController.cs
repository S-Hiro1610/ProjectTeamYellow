﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerRangeUnitController : CharactorBase
{
    #region property
    // プロパティを入れる
    public int BaseAttackCount => _baseAttackCount;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private int _baseAttackCount;
    [SerializeField]
    private int _maxAttackCount;
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
        if (transform.tag == "Enemy") _enemyMove = GetComponent<EnemyMove>();
        // 最大攻撃対象数の初期値を設定
        _maxAttackCount = _baseAttackCount;
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
                // エネミーは攻撃中停止する
                if (transform.tag == _enemyTag) _enemyMove.MoveSet(false);

                // ターゲットリストをソート
                if (_attackCollider.Targets.Count > 1)
                {
                    _attackCollider.Targets.Sort((x, y) => {
                        // 対象がいない場合は0を返す
                        if (x == null) return 0;
                        else if (y == null) return 0;

                        // ユニットとターゲットの位置の差分からベクトルの長さを求める
                        float x_magnitude = (gameObject.transform.position - x.transform.position).magnitude;
                        float y_magnitude = (gameObject.transform.position - y.transform.position).magnitude;
                        // XがYより大きければ後ろへ、XがYより小さければ前に回す
                        if (x_magnitude > y_magnitude)
                        {
                            return 1;
                        }
                        else if (x_magnitude < y_magnitude)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    });
                }
                else if (_attackCollider.Targets.Count == 0)
                {
                    // 攻撃対象がいない場合、エネミーは移動を開始
                    if (transform.tag == _enemyTag) _enemyMove.MoveSet(true);
                }

                // 攻撃回数の初期化
                int currentSubjects = 0;
                // Targetを順番に取得
                foreach (CharactorBase target in _attackCollider.Targets)
                {
                    // 現在の攻撃回数が最大攻撃回数より小さい場合、Attackを実行
                    if (currentSubjects < _maxAttackCount)
                    {
                        StartCoroutine(Attack(_attackCollider.Target));
                        // DrawRayで攻撃を可視化(仮)
                        var pos = target.transform.position - gameObject.transform.position;
                        Debug.DrawRay(gameObject.transform.position, pos, Color.white, 1.0f);
                        // 現在の攻撃回数を増やす
                        currentSubjects++;
                    }
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
        // 最大攻撃対象数の設定 3Lvごとに1上昇
        _maxAttackCount = _baseAttackCount + (level / 3);
        // エネミーの場合はEnemyMoveのフラグをtrue
        if (transform.tag == _enemyTag) _enemyMove.MoveSet(true);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}