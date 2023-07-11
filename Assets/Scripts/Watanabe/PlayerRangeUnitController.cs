using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeUnitController : CharactorBase
{
    #region property
    // プロパティを入れる
    public int MaxAttackCount => _maxAttackCount;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private int _maxAttackCount;
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
        if (transform.tag == "Enemy") _enemyMove = GetComponent<EnemyMove>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        // HPバーの向きをカメラ方向に固定
        SetRotationHPBarUI();

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
            else if(_attackCollider.Targets.Count == 0)
            {
                // 攻撃対象がいない場合、エネミーは移動を開始
                if (transform.tag == _enemyTag) _enemyMove.MoveSet(true);
            }

            // 攻撃回数の初期化
            int currentSubjects = 0;
            // Targetを順番に取得
            foreach (CharactorBase target in _attackCollider.Targets)
            {
                // Targetがnull(ユニットが消滅)の場合はリストから削除
                if (target == null || !target.gameObject.activeSelf)
                {
                    _attackCollider.Targets.RemoveAt(_attackCollider.Targets.IndexOf(target));
                    return;
                }

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
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public void EnemyInitilize(int level)
    {
        _attackCollider.Initilized();
        SetMaxHP(level);
        _enemyMove.MoveSet(true);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}