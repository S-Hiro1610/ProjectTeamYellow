using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Map をエディタ上で生成する自作ウィンドウEditor拡張機能
/// </summary>
public class MapEditorMain : EditorWindow
{
    #region property
    public int Columns => _columns;
    public int Rows => _rows;
    public float GridSize => _gridSize;
    public string StageName => _mapName;
    //public string SelectedPrefabPath => _selectedPrefabPath;
    public bool IsEraserMode => _isEraserMode;
    public bool IsCreatePrefab => _isCreatePrefab;
    #endregion

    #region serialize
    #endregion

    #region private
    /// <summary>マス目パーツを格納しているフォルダ</summary>
    private Object _mapPartsFolder;
    /// <summary>マップの横幅</summary>
    private int _columns = 10;
    /// <summary>マップの縦幅</summary>
    private int _rows = 5;
    /// <summary>エディタ画面のグリッドの大きさ</summary>
    private float _gridSize = 20.0f;
    /// <summary>マップ名</summary>
    private string _mapName = "";
    /// <summary>マップ編集画面</summary>
    private MapEdittingWindow _mapEdittingWindow;
    /// <summary>選択しているPrefabのパス</summary>
    //private string _selectedPrefabPath = "";
    /// <summary>消しゴム状態かどうか</summary>
    private bool _isEraserMode = false;
    /// <summary>prefabを作成するかどうか</summary>
    private bool _isCreatePrefab = false;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void OnGUI()
    {
        //マス目パーツのPrefabが入っているフォルダをセットする枠の作成
        GUILayout.BeginHorizontal();
        GUILayout.Label("Map Parts Folder", GUILayout.Width(120));
        _mapPartsFolder = EditorGUILayout.ObjectField(_mapPartsFolder, typeof(Object), true);
        GUILayout.EndHorizontal();

        //ステージ全体の大きさの値を設定する項目の作成
        GUILayout.BeginHorizontal();
            GUILayout.Label("Map Size", GUILayout.Width(120));
            _columns = EditorGUILayout.IntField("Number of Column",_columns);
        
            GUILayout.FlexibleSpace();

            _rows = EditorGUILayout.IntField("Number of Rows",_rows);
        GUILayout.EndHorizontal();

        //ステージ作成画面のグリッド幅を設定するフィールド
        GUILayout.BeginHorizontal();
            GUILayout.Label("Grid Size", GUILayout.Width(120));
            _gridSize = EditorGUILayout.FloatField(_gridSize);
        GUILayout.EndHorizontal();

        //ステージ名を入力する項目の作成
        GUILayout.BeginHorizontal();
            GUILayout.Label("Map Name", GUILayout.Width(120));
            _mapName = EditorGUILayout.TextField(_mapName);
        GUILayout.EndHorizontal();

        _isCreatePrefab = EditorGUILayout.ToggleLeft("IsCreate Prefab", _isCreatePrefab);

        //パーツ一覧描画


        GUILayout.FlexibleSpace();

        //マップ編集ボタン描画
        DrawMapEditButton();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    [MenuItem("Window/MapEditor")]
    private static void ShowMainWindow()
    {
        GetWindow(typeof(MapEditorMain));
    }

    private void DrawMapEditButton()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Button("マップ編集");  // ★ボタンイベントをどこで拾うか検討中！
        EditorGUILayout.EndVertical();

    }
    #endregion
}

public class MapEdittingWindow : EditorWindow
{
    #region private
    private MapEditorMain _parentWindow;
    #endregion

    #region constant
    /// <summary>ウィンドウの横幅</summary>
    private const float WINDOW_WIDTH = 600.0f;
    /// <summary>ウィンドウの縦幅</summary>
    private const float WINDOW_HEIGHT = 750.0f;
    #endregion

    #region public method
    public static MapEdittingWindow WillAppear(MapEditorMain parent)
    {
        MapEdittingWindow window = (MapEdittingWindow)GetWindow(typeof(MapEdittingWindow), false);
        window.Show();
        window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.SetParent(parent);
        // ここでマップ編集画面の初期化が必要
        return window;
    }
    #endregion

    #region private method
    private void SetParent(MapEditorMain parent)
    {
        _parentWindow = parent;
    }
    #endregion
}