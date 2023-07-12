using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public GameObject Stage => _stage;
    public List<Transform> SpawnerList => _spawnerList;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private GameObject _stagePrefab;
    [SerializeField]
    private Vector3 _stagePosition = new Vector3(0,0,0);
    [SerializeField]
    private GameObject _stage;
    [SerializeField]
    private List<Transform> _spawnerList;
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
        // Stage インスタンス作成
        _stage = Instantiate(_stagePrefab, _stagePosition, Quaternion.identity);

        // Spawner LIst作成
        foreach(Transform child in _stage.transform)
        {
            if(child.name.Contains("Spawner"))
            {
                _spawnerList.Add(child);
            }
        }

        foreach(Transform s in _spawnerList)
        {
            // この Spawner の座標から SpawnPoint（y+1）を求める。
            Vector3 sp = s.position;
            sp.y += 1;

            // この Spawner の routeList を生成して、先頭に初期出現座標(盤面外)とSpawnPointを設定する。
            var rt = new List<Vector3>();
            rt.Add(new Vector3(-1,-1,1));
            rt.Add(sp);

            // EndLine までのルートを探索して、routeList へ追加してゆく。
            Direction _nextDirection = s.GetComponent<MapParts>().NextDirection;
            while (_nextDirection != Direction.None)
            {
                switch (_nextDirection)
                {
                    case Direction.Up:
                        break;

                    case Direction.Right:
                        break;

                    case Direction.Down:
                        break;

                    case Direction.Left:
                        break;
                }
            }

            // routeList を Spawner に SetRoute() する。
            s.GetComponent<Spawner>().SetRoute(rt);
        }
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
