using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;


public abstract class CharactorBase : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public int Hp => _hp;
    public int Power => _power;
    public int AttackCoolTime => _attackCoolTime;
    public bool IsCanAttack => _isCanAttack;
    public IObservable<CharactorBase> OnAttack => _attackSubject;

    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _power;
    [Tooltip("攻撃間隔(ミリ秒) 1000=1s")]
    [SerializeField]
    protected int _attackCoolTime;
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
    public virtual void Attack(CharactorBase target)
    {
        target.Damaged(this);
    }

    public virtual void Damaged(CharactorBase target)
    {
        _hp -= target.Power;
        Debug.Log($"Damaged:{_hp}");
        if (_hp < 0)
        {
            Debug.Log($"Dead{gameObject}");
        }
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
