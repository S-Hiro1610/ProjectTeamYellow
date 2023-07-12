using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public GameState CurrentGameState => _currentGameState;
    public static GameManager Instance => _instance;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    [SerializeField]
    private GameState _currentGameState = GameState.Title;
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
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
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
    public void SetGameState(GameState state)
    {
        _currentGameState = state;
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
public enum GameState 
{
    Title,
    Stage1,
    Stage2
    //ステージ追加によって追加で記述
}

