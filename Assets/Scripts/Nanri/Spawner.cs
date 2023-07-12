using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stage テスト用 Spawner
/// Wtanabe/EnemyObjectPoolとセットでSpawnerへアタッチして使う
/// Spawnerの直上(y=0)に SpawnPoint を作成して、スクリプトにアタッチする 
/// </summary>
public class Spawner : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public List<WaveList> Wave => _wave;
    public Transform SpawnePoint => _spawnePoint;
    public bool WaveStart => _waveStart;
    public int ActiveEnemyCount => _activeEnemyCount;
    public bool WaveActive => _waveActive;
    #endregion

    #region serialize
    // unity inspectorに表示したいものを記述。
    [SerializeField]
    private List<WaveList> _wave;
    [SerializeField]
    private Transform _spawnePoint;
    [SerializeField]
    private bool _waveStart;
    [SerializeField]
    private int _activeEnemyCount = 0;
    [SerializeField]
    private bool _waveActive;
    #endregion

    #region private
    // プライベートなメンバー変数。
    private static EnemySpawner _instance;
    private int _waveIndex = 0;
    private List<Vector3> _enemyRoute;
    #endregion

    #region Constant
    // 定数をいれる。
    #endregion

    #region Event
    //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #endregion

    #region unity methods
    //
    private void Awake()
    {
        // テスト用の Default 値
        _enemyRoute = new List<Vector3>();
        _enemyRoute.Add(new Vector3(2, 0, 1));
        _enemyRoute.Add(new Vector3(2, 0, 0));
        _enemyRoute.Add(new Vector3(2, 0, -1));
        _enemyRoute.Add(new Vector3(2, 0, -2));
        _enemyRoute.Add(new Vector3(2, 0, -3));
        _enemyRoute.Add(new Vector3(3, 0, -3));
        _enemyRoute.Add(new Vector3(4, 0, -3));
        _enemyRoute.Add(new Vector3(5, 0, -3));
        _enemyRoute.Add(new Vector3(5, 0, -4));
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_waveStart && !_waveActive)
        {
            _waveStart = false;
            // Waveカウント数チェック
            if (_waveIndex < _wave.Count)
            {
                // Waveを開始
                StartCoroutine(WaveEnemy(_waveIndex));
                _waveIndex++;
            }
        }

        // 現在ActiveなEnemyの数を求める
        int childCount = transform.childCount;
        int activeEnemy = 0;
        for (int i = 0; i < childCount; i++)
        {
            GameObject childgameObject = transform.GetChild(i).gameObject;
            if (childgameObject.activeSelf) activeEnemy++;
        }
        _activeEnemyCount = activeEnemy;

        // WaveのEnemyがすべてスポーン後かつActiveのEnemyの数が0になったらWaveのフラグを立てる
        if (_activeEnemyCount == 0 && !_waveActive) _waveStart = true;

    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public static EnemySpawner Instance
    {
        get
        {
            return _instance;
        }
    }
    
    /// <summary>
    /// Stage Manager が SpawnPoint ごとの進軍ルートを設定する為に呼ぶ。
    /// 呼ばれる都度、上書きする。
    /// </summary>
    /// <param name="rt"></param>    
    public void SetRoute(List<Vector3> rt)
    {
        _enemyRoute = rt;
        _spawnePoint.transform.position = rt[0];
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private IEnumerator WaveEnemy(int waveindex)
    {

        _waveActive = true;
        int waveEnemyCount = _wave[waveindex].waveEnemy.Count;

        for (int i = 0; i < waveEnemyCount; i++)
        {
            yield return new WaitForSeconds(_wave[waveindex].waveEnemy[i].SpawneDelay);
            var go = EnemyObjectPool.Instance.GetGameObject(_wave[waveindex].waveEnemy[i].Enemy, _spawnePoint.position, _wave[waveindex].waveEnemy[i].Level);
            go.GetComponent<EnemyMove>().SetRoute(_enemyRoute);
        }
        _waveActive = false;
    }
    #endregion
}
