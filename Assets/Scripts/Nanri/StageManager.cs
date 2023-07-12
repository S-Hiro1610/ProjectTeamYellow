using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public List<GameObject> SpawnerList => _spawnerList;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private List<GameObject> _spawnerList;
    #endregion

    #region private
    // プライベートなメンバー変数。
    // ルート座標リスト
    private List<Vector3> _routeList = new List<Vector3>();
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
        //_routeList.Add(new Vector3(0, 1, 0));
        //// 初期位置
        //transform.position = _routeList[_routeIndex];

        //// ルート設定(仮で手動設定)
        //_routeList.Add(new Vector3(-2, 1, 0));
        //_routeList.Add(new Vector3(-2, 1, 3));
        //_routeList.Add(new Vector3(-4, 1, 3));
        //_routeList.Add(new Vector3(-4, 1, 1));
        //_routeList.Add(new Vector3(-6, 1, 1));
        //_routeList.Add(new Vector3(-6, 1, 4));
        //_routeList.Add(new Vector3(-8, 1, 4));
        //_routeList.Add(new Vector3(-8, 1, 1));
        //_routeList.Add(new Vector3(-9, 1, 1));

        //// 向きの設定
        //_routeIndex++;
        //transform.LookAt(_routeList[_routeIndex]);
    }

    private void Update()
    {
        // 移動フラグを確認
        //if (_moveFlag)
        //{
        //    // 目的地へ移動
        //    float step = _speed * Time.deltaTime;
        //    transform.position = Vector3.MoveTowards(transform.position, _routeList[_routeIndex], step);

        //    // 目的地に到達後、次のルート先を設定
        //    if (transform.position == _routeList[_routeIndex])
        //    {
        //        // 現在地座標番号がルートリスト数より小さければ、次の座標番号に移る
        //        if (_routeIndex < (_routeList.Count - 1))
        //        {
        //            _routeIndex++;
        //            transform.LookAt(_routeList[_routeIndex]);
        //        }
        //        else
        //        {
        //            _moveFlag = false;
        //        }

        //    }
        //}
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
