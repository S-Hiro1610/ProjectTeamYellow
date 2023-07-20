using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public float Speed => _speed;
    public bool MoveFlg => _moveFlag;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _moveFlag = true;
    #endregion

    #region private
    // プライベートなメンバー変数。
    // ルート座標リスト(仮)
    private List<Vector3> _routeList = new List<Vector3>();
    // 現在地座標番号
    private int _routeIndex = 0;
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
        // 初期値設定
        _routeIndex = 0;
        _routeList.Add(new Vector3(0, 1, 0));
        // 初期位置
        transform.position = _routeList[_routeIndex];
        // 向きの設定
        _routeIndex++;
    }

    private void Update()
    {
        // 移動フラグを確認
        if (_moveFlag)
        {
            // 目的地へ移動
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _routeList[_routeIndex], step);

            // 目的地に到達後、次のルート先を設定
            if (transform.position == _routeList[_routeIndex])
            {
                // 現在地座標番号がルートリスト数より小さければ、次の座標番号に移る
                if (_routeIndex < (_routeList.Count -1))
                {
                    _routeIndex++;
                }
                else
                {
                    _moveFlag = false;
                }
 
            }
        }
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    /// <summary>
    /// エネミー移動フラグをセット
    /// </summary>
    /// <param name="flag">true:移動 false:停止</param>
    public void MoveSet(bool flag)
    {
        _moveFlag = flag;
    }

    /// <summary>
    /// Spawnerからアクティブ化時に呼び出して進軍ルートをセットする
    /// </summary>
    /// <param name="route"></param>
    public void SetRoute(List<Vector3> route)
    {
        _routeList = route;
        transform.position = _routeList[_routeIndex];
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    // アクティブになった際、位置の初期化を行う(オブジェクトプール用)
    private void OnEnable()
    {
        // 初期値設定
        _routeIndex = 0;
        // 初期位置
        transform.position = _routeList[_routeIndex];
        // 向きの設定
        _routeIndex++;
    }

    #endregion
}
