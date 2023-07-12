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

        foreach(Transform spawner in _spawnerList)
        {
            // この Spawner の座標から SpawnPoint（y+1）を求める。
            Vector3 spawnPointPosition = spawner.position;
            spawnPointPosition.y += 1;

            // この Spawner の routeList を生成して、先頭に初期出現座標(盤面外)とSpawnPointを設定する。
            var routeList = new List<Vector3>();
            routeList.Add(new Vector3(-1,-1,1));
            routeList.Add(spawnPointPosition);

            // EndLine までのルートを探索して、routeList へ追加してゆく。
            Direction nextDirection = spawner.GetComponent<MapParts>().NextDirection;
            Transform nextMapCell = spawner;
            while (nextDirection != Direction.None)
            {
                var nextPosition = nextMapCell.position;
                switch (nextDirection)
                {
                    case Direction.Up:
                        nextPosition.z += 1;
                        break;

                    case Direction.Right:
                        nextPosition.x += 1;
                        break;

                    case Direction.Down:
                        nextPosition.z -= 1;
                        break;

                    case Direction.Left:
                        nextPosition.x -= 1;
                        break;
                }
                nextMapCell = FindAtPosition(_stage.transform, nextPosition);

                // 敵ユニットの進軍位置は、MapCellの直上（ｙ＋１）
                var nextEnemyPosition = nextMapCell.position;
                nextEnemyPosition.y += 1;
                routeList.Add(nextEnemyPosition);
                nextDirection = nextMapCell.GetComponent<MapParts>().NextDirection;
            }

            // routeList を Spawner に SetRoute() する。
            //spawner.GetComponent<Spawner>().SetRoute(routeList);
            Debug.Log("Spawner:"+spawner.position);
            foreach (Vector3 child in routeList)
                Debug.Log(child);
        }
    }

    private void Update()
    {
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private Transform FindAtPosition(Transform parent, Vector3 pos)
    {
        foreach(Transform child in parent)
        {
            if (child.position == pos)
                return child;
        }
        return null;
    }
    #endregion
}
