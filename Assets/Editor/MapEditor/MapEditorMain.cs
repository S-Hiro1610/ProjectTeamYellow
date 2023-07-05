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
    public string SelectedMapPartName => _selectedMapPartName;
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
    private string _selectedMapPartName = "";
    /// <summary>消しゴム状態かどうか</summary>
    private bool _isEraserMode = false;
    /// <summary>prefabを作成するかどうか</summary>
    private bool _isCreatePrefab = false;
    #endregion

    #region Constant
    /// <summary>ウィンドウの横幅</summary>
    private const float WINDOW_WIDTH = 560.0f;
    /// <summary>ウィンドウの縦幅</summary>
    private const float WINDOW_HEIGHT = 200.0f;
    /// <summary>パーツアイコンの横幅</summary>
    private const float PARTS_ICON_WIDTH = 50.0f;
    /// <summary>パーツアイコンの縦幅</summary>
    private const float PARTS_ICON_HEIGHT = 50.0f;
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void OnGUI()
    {
        // マス目パーツのPrefabが入っているフォルダをセットする枠の作成
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Map Parts Folder", GUILayout.Width(120));
            _mapPartsFolder = EditorGUILayout.ObjectField(_mapPartsFolder, typeof(Object), true);
        }

        // ステージ全体の大きさの値を設定する項目の作成
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Map Size", GUILayout.Width(120));
            _columns = EditorGUILayout.IntField("Number of Columns", _columns);

            GUILayout.FlexibleSpace();

            _rows = EditorGUILayout.IntField("Number of Rows", _rows);
        }

        // ステージ作成画面のグリッド幅を設定するフィールド
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Grid Size", GUILayout.Width(120));
            _gridSize = EditorGUILayout.FloatField(_gridSize);
        }

        // ステージ名を入力する項目の作成
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Map Name", GUILayout.Width(120));
            _mapName = EditorGUILayout.TextField(_mapName);
        }

        _isCreatePrefab = EditorGUILayout.ToggleLeft("IsCreate Prefab", _isCreatePrefab);

        // イレイサーモード用トグルボタンの作成
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Eraser Mode", GUILayout.Width(120));
            _isEraserMode = GUILayout.Toggle(_isEraserMode, _isEraserMode ? "ON" : "OFF", "Button");
            GUILayout.FlexibleSpace();
        }

        // パーツ選択バー描画
        DrawPartsSelector();

        GUILayout.FlexibleSpace();

        // マップ編集ボタン描画
        DrawMapEditButton();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    [MenuItem("Window/MapEditor")]
    private static void ShowMainWindow()
    {
        var window = GetWindow(typeof(MapEditorMain));
        window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
    }
    private void DrawPartsSelector()
    {
        if(_mapPartsFolder != null)
        {
            EditorGUILayout.LabelField("マップパーツ一覧");

            // マップパーツアセットフォルダのパスを取得
            string folderPath = AssetDatabase.GetAssetPath(_mapPartsFolder);
            // .metaを含むフォルダ内の全ファイル名を取得
            string[] allFileList = Directory.GetFiles(folderPath);
            // .metaを除いたファイルリストを取得
            string[] partsList = allFileList.Where(f => !f.Contains(".meta")).ToArray();
            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (var part in partsList)
                {
                    var partPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(part).GetComponent<MapParts>();
                    Texture2D texture = partPrefab.MapTexture;

                    // ボタンの描画
                    if(GUILayout.Button(texture, GUILayout.MaxWidth(PARTS_ICON_WIDTH), GUILayout.MaxHeight(PARTS_ICON_HEIGHT)))
                    {
                        _isEraserMode = false;
                        _selectedMapPartName = part;
                        Debug.Log("SelectedMapPartName = "+_selectedMapPartName);
                    }
                }
            }
        }
    }

    private void DrawMapEditButton()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("マップ編集"))
        {
            _mapEdittingWindow = MapEdittingWindow.WillAppear(this);
        }
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
    private const float WINDOW_HEIGHT = 700.0f;
    #endregion

    #region public method
    public static MapEdittingWindow WillAppear(MapEditorMain parent)
    {
        MapEdittingWindow window = (MapEdittingWindow)GetWindow(typeof(MapEdittingWindow), title:null, utility:false, focus:true);
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