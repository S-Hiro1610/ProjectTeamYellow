using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameManager : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public static GameManager Instance => _instance;
    public ReactiveProperty<int> Resouce => _resouce;
    public ReactiveProperty<int> EnemyCount => _enemyCount;
    public ReactiveProperty<int> RemainingEnemies => _remainingEnemies;
    public IObservable<Unit> OnStop => _stopEvent;
    public IObservable<Unit> OnStart => _startEvent;
    public IObservable<Unit> OnLevelUp => _levelUpEvent;
    public IObservable<Unit> OnChangeTitle => _changeTitleEvent;
    public IObservable<Unit> OnChangeInitialize => _changeInitializeEvent;
    public IObservable<Unit> OnChangeInGame => _changeInGameEvent;
    public IObservable<Unit> OnChangeGameOver => _changeGameOverEvent;
    public int LevelUpIndex => _levelUpIndex;
    public List<int> LevelUpList => _levelUpList;
 
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    /// <summary>1秒間に加算されるリソースの数</summary>
    [SerializeField]
    private int _addResouce = 100;
    /// <summary>手持ちの配置リソース</summary>
    private ReactiveProperty<int> _resouce = new ReactiveProperty<int>(0);

    /// <summary>レベルアップの基準となる敵を倒す要求値のリスト</summary>
    [SerializeField]
    private List<int> _levelUpList = new List<int>();
    private int _levelUpIndex = 0;
    /// <summary>倒した敵のカウント</summary>
    private ReactiveProperty<int> _enemyCount = new ReactiveProperty<int>(0);

    private Subject<Unit> _stopEvent = new Subject<Unit>();
    private Subject<Unit> _startEvent = new Subject<Unit>();

    private Subject<Unit> _levelUpEvent = new Subject<Unit>();
    private Subject<Unit> _changeTitleEvent = new Subject<Unit>();
    private Subject<Unit> _changeInitializeEvent = new Subject<Unit>();
    private Subject<Unit> _changeInGameEvent = new Subject<Unit>();
    private Subject<Unit> _changeGameOverEvent = new Subject<Unit>();

    private bool _isPlay = false;
    private static GameManager _instance;
    private Coroutine _addResouceCoroutine;
    private ReactiveProperty<int> _remainingEnemies = new ReactiveProperty<int>(0);
    private int _waveEnemyCount = 0;
    private int _remainingEnemyCount = 0;
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
        if (_instance == null)
        {
            _instance = this;
            //return;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        _startEvent.Subscribe(_ => ChangePlayStart());
        _stopEvent.Subscribe(_ => ChangePlayStop());

        _changeInitializeEvent.Subscribe(_ => Initialize());
        _changeInGameEvent.Subscribe(_ =>
        {
            TimerStop();
            WaitTrail();
            TimerStart();
            _addResouceCoroutine = StartCoroutine(AddResouce());
        });

    }

    private void Start()
    {
        // Game 起動直後は、Title画面なのでタイマーを停止する。 
        TimerStop();

        CardInfo[] testCardInfo = new CardInfo[3];
        testCardInfo[0].coolTime = 0;
        testCardInfo[0].LVUIString = "2";
        testCardInfo[0].costUIString = "20";

        testCardInfo[1].coolTime = 0;
        testCardInfo[1].LVUIString = "8";
        testCardInfo[1].costUIString = "40";

        testCardInfo[2].coolTime = 0;
        testCardInfo[2].LVUIString = "1";
        testCardInfo[2].costUIString = "100";

        EnemyCount.Subscribe(count =>
        {
            _remainingEnemyCount = count; 
            CalcRemainingEnemies(_remainingEnemyCount, _waveEnemyCount);
        });

        WaveManager.Instance.WaveEnemyCount.Subscribe(waveEnemy =>
        {
            _waveEnemyCount = waveEnemy;
            CalcRemainingEnemies(_remainingEnemyCount, _waveEnemyCount);
        });
    }

    private void Update()
    {

    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    /// <summary>倒した敵をカウントする</summary>
    public void AddEnemyCount()
    {
        _enemyCount.Value++;
        if (_enemyCount.Value >= _levelUpList[_levelUpIndex])
        {
            LevelUp();
            _enemyCount.Value = 0;
            if (_levelUpIndex < _levelUpList.Count)
            {
                _levelUpIndex++;
            }
        }
    }

    public void TimerStop()
    {
        _stopEvent.OnNext(Unit.Default);

    }

    public void TimerStart()
    {
        _startEvent.OnNext(Unit.Default);
    }

    public void StartGame()
    {
        _changeInitializeEvent.OnNext(Unit.Default);
        _changeInGameEvent.OnNext(Unit.Default);
    }

    public void EndGame()
    {
        StopCoroutine(_addResouceCoroutine);
        _changeGameOverEvent.OnNext(Unit.Default);
    }
    public void ReturnTitle()
    {
        _changeInitializeEvent.OnNext(Unit.Default);
        _changeTitleEvent.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private IEnumerator AddResouce()
    {
        while (true)
        {
            if (_isPlay)
            {
                _resouce.Value += _addResouce;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void ChangePlayStart()
    {
        _isPlay = true;
    }
    private void ChangePlayStop()
    {
        _isPlay = false;
    }

    /// <summary>初期化処理</summary>
    private void Initialize()
    {
        _resouce.Value = 0;
        _enemyCount.Value = 0;
        _levelUpIndex = 0;
    }

    private void LevelUp()
    {
        TimerStop();
        _levelUpEvent.OnNext(Unit.Default);
    }

    /// <summary>
    /// ゲーム開始時の敵ユニット侵攻ルート表示処理待ち
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitTrail()
    {
        yield return new WaitForSeconds(3.0f);
    }

    private void CalcRemainingEnemies(int enemyCount, int waveEnemyCount)
    {
        _remainingEnemies.Value = waveEnemyCount - enemyCount;
    }
    #endregion
}