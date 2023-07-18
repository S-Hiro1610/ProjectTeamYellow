using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class WaveManager : MonoBehaviour
{
    #region property
    // プロパティを入れる
    public List<WaveList> Wave => _wave;
    public List<Spawner> Spawner => _spawner;
    public ReactiveProperty<int> WaveCount => _waveCount;
    public ReactiveProperty<int> WaveEnemyCount => _waveEnemyCount;
    public bool WaveStart => _waveStart;
    public float WaveDelayTime => _waveDelayTime;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private List<WaveList> _wave;
    [SerializeField]
    private List<Spawner> _spawner;
    [SerializeField]
    private ReactiveProperty<int> _waveCount = new ReactiveProperty<int>(0);
    [SerializeField]
    private ReactiveProperty<int> _waveEnemyCount = new ReactiveProperty<int>(0);
    [SerializeField]
    private bool _waveStart;
    [SerializeField]
    private float _waveDelayTime;
    #endregion

    #region private
    // プライベートなメンバー変数。
    private static WaveManager _instance;
    private int _currentwave;
    private bool _waveActive;
    // エンドレスでの周回カウント用　初期値:0
    private int _weekCount;
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

        WaveInitilize();
    }

    private void Start()
    {

    }

    private void Update()
    {
        // WaveStartフラグかつWaveActiveでなければWaveを開始
        if (WaveStart && !_waveActive)
        {
            // Wave表示カウントアップ、敵総数カウント初期化
            _waveCount.Value++;
            _waveEnemyCount.Value = 0;
            // Wave中はWaveStartフラグを止めて、WaveActiveフラグを立てる
            _waveStart = false;
            _waveActive = true;
            // Waveエネミーを生成
            StartCoroutine(StartEnemySpawne(_currentwave));
        }
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public static WaveManager Instance
    {
        get
        {
            return _instance;
        }
    }


    /// <summary>
    /// Waveマネージャー初期化
    /// </summary>
    public void WaveInitilize()
    {
        _waveStart = false;
        _waveActive = false;
        _currentwave = 1;
        _waveCount.Value = 0;
        _weekCount = 0;
        _waveEnemyCount.Value = 0;
    }

    /// <summary>
    /// WaveStartフラグのセット
    /// </summary>
    public void SetWaveStart()
    {
        _waveStart = true;
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private IEnumerator StartEnemySpawne(int waveindex)
    {
        int index = waveindex - 1;
        int waveEnemyNum = Wave[index].waveEnemy.Count;
        // 1Waveの敵総数カウント
        for (int i = 0; i < waveEnemyNum; i++)
        {
            Wave WaveEnemys = Wave[index].waveEnemy[i];
            int waveEnemyUnits = WaveEnemys.SpawnUnits;
            _waveEnemyCount.Value += waveEnemyUnits;
        }

        // エネミー生成
        for (int i = 0; i < waveEnemyNum; i++)
        {
            Wave WaveEnemys = Wave[index].waveEnemy[i];
            int waveEnemyUnits = WaveEnemys.SpawnUnits;
            int spawnerCount = 0;
            for (int j = 0; j < waveEnemyUnits; j++)
            {
                if (spawnerCount == Spawner.Count) spawnerCount = 0;

                GameObject Enemy = WaveEnemys.Enemy;
                int Level = WaveEnemys.Level;
                float SpawneDelay = WaveEnemys.SpawneDelay;
                // スポナーにエネミー生成をしてもらう。Lvは周回ごとに1Lv上昇
                Spawner[spawnerCount].WaveEnemySpawne(Enemy, (Level + _weekCount));
                yield return new WaitForSeconds(SpawneDelay);
                spawnerCount++;
            }
        }
        // 現在のWave状況更新
        _currentwave++;
        if (_currentwave > Wave.Count)
        {
            _currentwave = 1;
            _weekCount++;
        }
        // 次のWaveまでWaveDelayTime待機
        yield return new WaitForSeconds(WaveDelayTime);
        // WaveActiveフラグを止める
        _waveActive = false;
    }
    #endregion
}
