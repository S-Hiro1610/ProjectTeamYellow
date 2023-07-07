using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

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
        
    }

    private void Update()
    {
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
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public void EnemyInitilize(int level)
    {
        SetMaxHP(level);
        _enemyMove.MoveSet(true);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}