using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class AttackCollider : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public bool IsTarget => _isTarget;
    public CharactorBase Target => _target;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    private bool _isTarget = false;
    private CharactorBase _target;
    private string _tag = "";
    private List<CharactorBase> _targets = new List<CharactorBase>();
    #endregion

    #region Constant
    // 定数をいれる。
    private const string _enemyTag = "Enemy";
    private const string _playerTag = "Player";
    #endregion

    #region Event
    //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #endregion

    #region unity methods
    //  Start, UpdateなどのUnityのイベント関数。
    private void Awake()
    {
        _isTarget = false;
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
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private void OnTriggerEnter(Collider other)
    {
        _tag = other.gameObject.tag;
        
        if (other.CompareTag(_enemyTag) || other.CompareTag(_playerTag))
        {
            if (_tag == _enemyTag)
            {
                if (other.TryGetComponent(out CharactorBase item))
                {
                    _targets.Add(item);
                }
            }
            else if (_tag == _playerTag)
            {
                if (other.TryGetComponent(out _target))
                {
                    _isTarget = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(_enemyTag))
        {
            _target = _targets[0];
            _isTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isTarget = false;
    }
    #endregion
}
