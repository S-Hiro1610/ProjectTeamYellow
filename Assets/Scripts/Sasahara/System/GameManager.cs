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
    public GameState CurrentState => _currentState;
    public IObservable<Unit> OnStop => _stopEvent;
    public IObservable<Unit> OnStart => _startEvent;
    public IObservable<Unit> OnChangeTitle => _changeTitleEvent;
    public IObservable<Unit> OnChangeInitialize => _changeInitializeEvent;
    public IObservable<Unit> OnChangeInGame => _changeInGameEvent;
    public IObservable<Unit> OnChangeGameOver => _changeGameOverEvent;
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
    /// <summary>倒した敵のカウント</summary>
    private ReactiveProperty<int> _enemyCount = new ReactiveProperty<int>(0);

    private Subject<Unit> _stopEvent = new Subject<Unit>();
    private Subject<Unit> _startEvent = new Subject<Unit>();

    private Subject<Unit> _levelUpEvent = new Subject<Unit>();

    private Subject<Unit> _changeTitleEvent = new Subject<Unit>();
    private Subject<Unit> _changeInitializeEvent = new Subject<Unit>();
    private Subject<Unit> _changeInGameEvent = new Subject<Unit>();
    private Subject<Unit> _changeGameOverEvent = new Subject<Unit>();

    private int _levelUpIndex = 0;
    private GameState _currentState = GameState.Title;
    private bool _isPlay = false;
    private static GameManager _instance;
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
            return;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        _stopEvent.Subscribe(_ => ChangePlayFrag());
        _startEvent.Subscribe(_ => ChangePlayFrag());



        _changeTitleEvent.Subscribe(_ => SetCurrentState(GameState.Title));
        _changeInitializeEvent.Subscribe(_ => SetCurrentState(GameState.Initialize));
        _changeInGameEvent.Subscribe(_ => SetCurrentState(GameState.InGame));
        _changeGameOverEvent.Subscribe(_ => SetCurrentState(GameState.GameOver));
    }

    private void Start()
    {

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
            _levelUpEvent.OnNext(Unit.Default);
            if (_levelUpIndex < _levelUpList.Count)
            {
                _levelUpIndex++;
            }
        }
        //UI側で EnemyCount.Subscribe(x => [ここに表示を更新する処理(xが値)])
    }

    public void TimerStop()
    {
        _stopEvent.OnNext(Unit.Default);
    }

    public void TimerStart()
    {
        _startEvent.OnNext(Unit.Default);
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
                yield return new WaitForSeconds(1);
                _resouce.Value += _addResouce;
                //UI側で Resouce.Subscribe(x => [ここに表示を更新する処理(xが値)])
            }
        }
    }

    private void ChangePlayFrag()
    {
        _isPlay = !_isPlay;
    }

    private void SetCurrentState(GameState state)
    {
        _currentState = state;
    }
    #endregion
}

public enum GameState
{
    Title,
    Initialize,
    InGame,
    GameOver
}