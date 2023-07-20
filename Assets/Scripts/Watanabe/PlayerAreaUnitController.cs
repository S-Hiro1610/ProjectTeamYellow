using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaUnitController : CharactorBase
{
    #region property
    // プロパティを入れる
    public int MaxAttackCount => _maxAttackCount;
    public SphereCollider AreaCollider;
    public float BaseAreaRadius => _baseAreaRadius;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private int _maxAttackCount;
    [Tooltip("範囲攻撃の攻撃判定をする子オブジェクト")]
    [SerializeField]
    protected AttackCollider _attackAreaCollider;
    [SerializeField]
    protected ParticleSystem _explosion;
    [SerializeField]
    private float _baseAreaRadius;
    [SerializeField]
    private float _areaRadius;
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
        // 攻撃範囲の初期値を設定
        _areaRadius = _baseAreaRadius;
        AreaCollider.radius = _areaRadius;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        // HPバーの向きをカメラ方向に固定
        SetRotationHPBarUI();

        // ターゲットが非アクティブである場合は初期化を行う
        if (_attackCollider.Target != null)
        {
            if (!_attackCollider.Target.gameObject.activeSelf)
            {
                _attackCollider.TargetUpdate(_attackCollider.Target);
            }
        }

        if (_isCanAttack && _attackCollider.IsTarget)
        {
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

            // 攻撃回数の初期化
            int currentSubjects = 0;
            // Targetを順番に取得
            foreach (CharactorBase target in _attackCollider.Targets)
            {
                // 現在の攻撃回数が最大攻撃回数より小さい場合、Attackを実行
                if (currentSubjects < _maxAttackCount)
                {
                    // DrawRayで攻撃を可視化(仮)
                    var pos = target.transform.position - gameObject.transform.position;
                    Debug.DrawRay(gameObject.transform.position, pos, Color.white, 1.0f);

                    // 範囲攻撃用コライダーをターゲットの位置へ移動
                    _attackAreaCollider.transform.position = target.transform.position;
                    _attackAreaCollider.transform.gameObject.SetActive(true);

                    // 範囲攻撃用コライダーで取得したターゲットが1以上であれば範囲内のターゲットに攻撃
                    if (_attackAreaCollider.Targets.Count > 0)
                    {
                        StartCoroutine(AreaAttack(_attackAreaCollider, target.transform.position));
                    }
                    // 現在の攻撃回数を増やす
                    currentSubjects++;
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
        // 範囲攻撃の範囲設定 3Lvごとに0.5上昇
        _areaRadius = _baseAreaRadius + (level / 3 * 0.5f);
        AreaCollider.radius = _areaRadius;
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private IEnumerator AreaAttack(AttackCollider areacollider, Vector3 position)
    {
        _isCanAttack = false;
        yield return new WaitForSeconds(_attackCoolTime);
        foreach (CharactorBase areatarget in areacollider.Targets)
        {
            // Targetがnull(ユニットが消滅)の場合はリストから削除
            if (areatarget == null || !areatarget.gameObject.activeSelf)
            {
                areacollider.Targets.RemoveAt(areacollider.Targets.IndexOf(areatarget));
                _isCanAttack = true;
                yield break;
            }
            NoDelayAttack(areatarget);
        }
        // パーティクルをターゲットの位置へ移動させて、Playを実行
        if (_explosion != null)
        {
            _explosion.transform.position = position;
            _explosion.Play();
        }

        // 取得したターゲットをクリアする
        areacollider.transform.gameObject.SetActive(false);
        areacollider.Targets.Clear();
        _isCanAttack = true;
    }
    #endregion
}