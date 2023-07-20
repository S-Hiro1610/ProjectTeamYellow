using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrail : MonoBehaviour
{
    #region property
    // プロパティを入れる
    public List<Vector3> RouteList => _routeList;
    #endregion

    #region SerializeField
    // unity inspectorに表示したいものを記述。
    [SerializeField]
    // Trailの移動速度
    private float _speed = 1.0f;
    [SerializeField]
    private List<Vector3> _routeList;
    [SerializeField]
    private bool _moveFlag;
    #endregion

    #region private
    // プライベートなメンバー変数。
    // 現在地座標番号
    private int _routeIndex = 0;
    #endregion

    #region unity methods

    private void Awake()
    {
        // 仮データ
        //_routeList = new List<Vector3>();
        //_routeList.Add(new Vector3(0, 0, 0));
        //_routeList.Add(new Vector3(1, 0, 0));
        //_routeList.Add(new Vector3(2, 0, 0));
        //_routeList.Add(new Vector3(3, 0, 0));
        //_routeList.Add(new Vector3(4, 0, 0));
        //_routeList.Add(new Vector3(5, 0, 0));
        //_routeList.Add(new Vector3(5, 0, -1));
        //_routeList.Add(new Vector3(5, 0, -2));
        //_routeList.Add(new Vector3(6, 0, -2));
        //_routeList.Add(new Vector3(7, 0, -2));
        //_routeList.Add(new Vector3(8, 0, -2));
        //_routeList.Add(new Vector3(9, 0, -2));
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _routeList[_routeIndex];
        _moveFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveFlag)
        {
            // デルタタイム間の移動距離だけ次の目的地へ移動する
            var step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _routeList[_routeIndex], step);

            // 目的地に到達なら、次の目的地を設定
            if (transform.position == _routeList[_routeIndex])
            {
                _routeIndex++;

                // ルートリストの数より小さければ、次の座標番号に移る
                if (_routeIndex < _routeList.Count)
                {
                    transform.LookAt(_routeList[_routeIndex]);
                }
                else
                {
                    _moveFlag = false;
                }

            }
        }
    }
    #endregion
    #region public
    public void SetRouteList(List<Vector3> rt)
    {
        _routeList = rt;
    }
    #endregion
}
