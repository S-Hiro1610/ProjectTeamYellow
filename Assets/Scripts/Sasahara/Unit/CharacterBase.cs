using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

public abstract class CharactorBase : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public int MaxHp => _maxHp;
    public int Hp => _hp;
    public int Power => _power;
    public float AttackCoolTime => _attackCoolTime;
    public bool IsCanAttack => _isCanAttack;
    public IObservable<CharactorBase> OnAttack => _attackSubject;

    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _power;
    [Tooltip("攻撃間隔(秒)")]
    [SerializeField]
    protected float _attackCoolTime;
    [SerializeField]
    protected bool _isCanAttack;
    [Tooltip("攻撃判定をする子オブジェクト")]
    [SerializeField]
    protected AttackCollider _attackCollider;
    #endregion

    #region private
    // プライベートなメンバー変数。
    private Subject<CharactorBase> _attackSubject = new Subject<CharactorBase>();
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
    public void SetMaxHP(int level)
    {
        _hp = _maxHp * level;
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
            target.Damaged(this);
            _isCanAttack = true;
        }
    }

    protected virtual void Damaged(CharactorBase target)
    {
        _hp -= target.Power;
        Debug.Log($"Damaged:{_hp}");
        if (_hp <= 0)
        {
            Debug.Log($"Dead{gameObject}");
            //Destroy(gameObject);
            EnemyObjectPool.Instance.ReleaseGameObject(gameObject);
            _hp = _maxHp;
        }
    }
    #endregion
}
