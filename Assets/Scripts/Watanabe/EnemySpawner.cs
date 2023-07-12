using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public List<WaveList> Wave => _wave;
    public Transform SpawnePoint => _spawnePoint;
    public bool WaveStart => _waveStart;
    public int ActiveEnemyCount => _activeEnemyCount;
    public bool WaveActive => _waveActive;

    public GameObject UnitPool;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
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
        // Singleton
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    private void Update()
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
        int childCount = UnitPool.transform.childCount;
        int activeEnemy = 0;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                GameObject childgameObject = UnitPool.transform.GetChild(i).gameObject;
                if (childgameObject.activeSelf) activeEnemy++;
            }
            _activeEnemyCount = activeEnemy;
        }

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
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private IEnumerator WaveEnemy(int waveindex)
    {
        _waveActive = true;
        int waveEnemyCount = _wave[waveindex].waveEnemy.Count;

        for (int i = 0;i < waveEnemyCount; i++)
        {
            yield return new WaitForSeconds(_wave[waveindex].waveEnemy[i].SpawneDelay);
            UnitObjectPool.Instance.SpawneUnit(_wave[waveindex].waveEnemy[i].Enemy, _spawnePoint.position, _wave[waveindex].waveEnemy[i].Level);
        }
        _waveActive = false;
    }
    #endregion
}
