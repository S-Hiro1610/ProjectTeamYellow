using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBlock : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public bool isEnemyRoute => _isEnemyRoute;
    public bool isPlayerUnitSet => _isPlayerUnitSet;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField] private bool _isEnemyRoute;
    [SerializeField] private bool _isPlayerUnitSet;
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
        // 敵の進行ルート：赤
        // ユニットが配置できる箇所：緑
        // 敵の進行ルートかつユニットが配置できる：青
        // 敵の進行ルートでもユニットの配置もできない：白
        // 敵の進行ルートがわかりやすい用に色付け（仮）
        if (_isEnemyRoute)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        // ユニットが配置できる場合は緑色で色付け（仮）
        if (_isPlayerUnitSet)
        {
            GetComponent<Renderer>().material.color = Color.green;
            // 敵の進行ルートでもある場合
            if (_isEnemyRoute)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
        }

    }

    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
