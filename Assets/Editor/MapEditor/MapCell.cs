using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapCell : ScriptableObject
{
    #region property
    public int Row => _row;
    public int Column => _column;
    public CellStatus Status => _cellStatus;
    public string PrefabName => _prefabName;

    #endregion

    #region serialize
    /// <summary> MapCell の行位置 </summary>
    [SerializeField] private int _row;
    /// <summary> MapCell の列位置 </summary>
    [SerializeField] private int _column;
    /// <summary> MapCell の使用状態/用途 </summary>
    [SerializeField] private CellStatus _cellStatus = CellStatus.Blank;
    /// <summary> MapCell に紐づけているプレファブ </summary>
    [SerializeField] private string _prefabName = "";
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    #endregion

    #region public method
    /// <summary>
    /// マス目オブジェクトの初期化
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void Initialize(int row, int col)
    {
        _row = row;
        _column = col;
        _cellStatus = CellStatus.Blank;
        _prefabName = "";
    }

    /// <summary>
    /// マス目にデータをセットする
    /// </summary>
    /// <param name="map"></param>
    /// <param name="prefabName"></param>
    public void Set(string prefabName) 
    {
        _cellStatus = CellStatus.Occupied;
        _prefabName = prefabName;
    }

    /// <summary>
    /// マス目のデータをクリアする
    /// </summary>
    /// <param name="map"></param>
    public void Reset()
    {
        _cellStatus = CellStatus.Blank;
        _prefabName = "";
    }
    #endregion
    
    #region private method
    #endregion
}
public enum CellStatus
{
    Blank,
    Occupied
}
