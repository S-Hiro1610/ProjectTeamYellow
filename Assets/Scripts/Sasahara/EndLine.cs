using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndLine : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    [SerializeField]
    private string EnemyTag = "Enemy";
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

    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(EnemyTag))
        {
            GameManager.Instance.TimerStop();
            GameManager.Instance.EndGame();
        }
    }
    #endregion
}
