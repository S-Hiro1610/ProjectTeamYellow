using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public List<UnitProperty> PlayerPrefab => _playerPrefab;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private List<UnitProperty> _playerPrefab;
    #endregion

    #region private
    // プライベートなメンバー変数。
    private static UnitManager _instance;
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
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public static UnitManager Instance
    {
        get
        {
            return _instance;
        }
    }
    /// <summary>
    /// ユニットの生成
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    public void UnitCreate(UnitType type, Vector3 position)
    {
        int index = (int)type;
        UnitObjectPool.Instance.SpawneUnit(_playerPrefab[index].Unit, position, _playerPrefab[index].Level);
    }

    /// <summary>
    /// ユニットのレベルアップ
    /// </summary>
    /// <param name="type"></param>
    public void LevelUp(UnitType type)
    {
        int index = (int)type;
        _playerPrefab[index].Level++;
        _playerPrefab[index].Cost = (int)(_playerPrefab[index].Cost * 1.5f);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}

[Serializable]
public class UnitProperty
{
    public GameObject Unit;
    public int Level;
    public int Cost;
}

public enum UnitType
{
    Wall,
    Range,
    Area
}
