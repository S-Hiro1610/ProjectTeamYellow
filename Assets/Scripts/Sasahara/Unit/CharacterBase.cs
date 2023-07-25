using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.UI;

public abstract class CharactorBase : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public int Lv => _lv;
    public int BaseHp => _baseHp;
    public int Hp => _hp;
    public int BasePower => _basePower;
    public int Power => _power;
    public int SeIndex => _seIndex;
    public float AttackCoolTime => _attackCoolTime;
    public bool IsCanAttack => _isCanAttack;
    public IObservable<CharactorBase> OnAttack => _attackSubject;
    public Slider HpSlider => _hpSlider;

    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    protected int _lv;
    [SerializeField]
    protected int _baseHp;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _basePower;
    [SerializeField]
    protected int _power;
    [SerializeField]
    protected int _seIndex;
    [Tooltip("攻撃間隔(秒)")]
    [SerializeField]
    protected float _attackCoolTime;
    [SerializeField]
    protected bool _isCanAttack;
    [Tooltip("攻撃判定をする子オブジェクト")]
    [SerializeField]
    protected AttackCollider _attackCollider;
    [SerializeField]
    protected Slider _hpSlider;
    #endregion

    #region private
    // プライベートなメンバー変数。
    private Subject<CharactorBase> _attackSubject = new Subject<CharactorBase>();
    #endregion

    #region Constant
    // 定数をいれる。
    protected const string _enemyTag = "Enemy";
    protected const string _playerTag = "Player";
    #endregion

    #region Event
    //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #endregion

    #region unity methods
    //  Start, UpdateなどのUnityのイベント関数。
    private void Awake()
    {
        OnAttack.Subscribe(_ => Attack(this));
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
    public void SetParameter(int level)
    {
        // Lvに合わせたパラメータを設定
        _lv = level;
        _hp = _baseHp + (_lv * 5);
        _power = _basePower + _lv;
        _hpSlider.value = 1.0f;
        _isCanAttack = true;
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    protected virtual IEnumerator Attack(CharactorBase target)
    {
        if (target != null)
        {
            // 自身と同じタグである場合は攻撃しない
            if (target.transform.tag == this.transform.tag) yield break;
            // クールタイムを待ってから攻撃する
            _isCanAttack = false;
            yield return new WaitForSeconds(_attackCoolTime);
            // 効果音再生
            AudioPlayer.Instance.SEPlay(SeIndex);
            target.Damaged(this);
            _isCanAttack = true;
        }
    }

    protected virtual void NoDelayAttack(CharactorBase target)
    {
        if (target != null)
        {
            // 自身と同じタグである場合は攻撃しない
            if (target.transform.tag == this.transform.tag) return;
            _isCanAttack = false;
            // 効果音再生
            AudioPlayer.Instance.SEPlay(SeIndex);
            target.Damaged(this);
            _isCanAttack = true;
        }
    }

    protected virtual void Damaged(CharactorBase target)
    {
        _hp -= target.Power;
        _hpSlider.value = (float)_hp / (float)(_baseHp + (_lv * 5));
        Debug.Log($"Damaged:{_hp}");
        if (_hp <= 0 && gameObject.activeSelf)
        {
            Debug.Log($"Dead{gameObject}");
            // エネミーが倒された時、カウントするためにGameManagerからAddEnemyCountを呼ぶ
            if (this.transform.tag == _enemyTag)
            {
                GameManager.Instance.AddEnemyCount();
            }

            // 非アクティブにする
            UnitObjectPool.Instance.ReleaseGameObject(gameObject);
        }
    }

    protected virtual void SetRotationHPBarUI()
    {
        // HPバーの向きをカメラ方向に固定
        _hpSlider.transform.rotation = Camera.main.transform.rotation;
    }
    #endregion
}
