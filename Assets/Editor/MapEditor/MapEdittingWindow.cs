using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEdittingWindow : EditorWindow
{
    #region private
    private Rect[,] _gridRect;
    private float _gridSize;
    private MapCell[,] _mapCells;
    private MapEditorMain _parentWindow;
    private Object _mapSaveFile;
    #endregion

    #region constant
    /// <summary>ウィンドウの横幅</summary>
    private const float WINDOW_WIDTH = 600.0f;
    /// <summary>ウィンドウの縦幅</summary>
    private const float WINDOW_HEIGHT = 500.0f;
    #endregion

    #region unity method
    /// <summary>
    /// MapEdittingWindow の描画/更新
    /// </summary>
    private void OnGUI()
    {
        // 罫線を描画する（毎回更新やめられない？）
        for (int row = 0; row < _parentWindow.Rows; row++)
        {
            for (int col = 0; col < _parentWindow.Columns; col++)
            {
                DrawGrid(_gridRect[row, col]);
            }
        }

        // 左ボタンクリックされたマス目にパーツ画像を設置/消去する
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector2 pos = Event.current.mousePosition;
            int row, col;
            bool found = false;

            for (row = 0; row < _parentWindow.Rows; row++)
            {
                Rect r = _gridRect[row, 0];

                if (r.yMin <= pos.y && pos.y <= r.yMax)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                for (col = 0; col < _parentWindow.Columns; col++)
                {
                    if (_gridRect[row, col].Contains(pos))
                    {
                        if (_parentWindow.IsEraserMode)
                        {
                            // イレイサーモード処理
                            _mapCells[row, col].Reset();
                        }
                        else
                        {
                            if (_parentWindow.SelectedMapPartName != "")
                            {
                                // パーツ設置モード処理
                                _mapCells[row, col].Set(_parentWindow.SelectedMapPartName);
                            }
                        }
                        break;
                    }
                }
            }
        }

        // 配置されたマップパーツの描画
        for (int row = 0; row < _parentWindow.Rows; row++)
        {
            for (int col = 0; col < _parentWindow.Columns; col++)
            {
                if (_mapCells[row, col].Status != CellStatus.Blank)
                {
                    var partPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(_mapCells[row, col].PrefabName).GetComponent<MapParts>();
                    Texture2D texture = partPrefab.MapTexture;
                    GUI.DrawTexture(_gridRect[row, col], texture);
                }
            }
        }
        Repaint();

        // マップ生成ボタンの表示
        GUILayout.FlexibleSpace();
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            if (GUILayout.Button("Push to Generate", GUILayout.MinWidth(120), GUILayout.MinHeight(40)))
            {
                GenerateMapObject(_mapCells);
                //CheckCurrentStageData(_stageCells);
            }
            GUILayout.FlexibleSpace();
        }

        // 区切りの表示
        GUILayout.Space(10);
        using(new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            GUILayout.Label("--- または ---", GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
        }
        GUILayout.Space(10);

        // マップロードボタンの表示
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            if (GUILayout.Button("Push to Load", GUILayout.MinWidth(120)))
            {
                //ファイルがセットされている場合
                if (_mapSaveFile != null)
                {
                    //LoadStageDataFile();
                }
            }
            GUILayout.Space(10);
            GUILayout.Label("Specify Map File to load ->", GUILayout.Width(120));
            GUILayout.Space(5);
            _mapSaveFile = EditorGUILayout.ObjectField(_mapSaveFile, typeof(Object), true);
            GUILayout.FlexibleSpace();
        }

        // 操作説明文の表示
        using (new EditorGUILayout.VerticalScope())
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("【操作方法】", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("・マップパーツ選択後、配置したいマスを「左クリック」でパーツを配置");
            EditorGUILayout.LabelField("・全マス配置完了後、「Push to Generate」ボタン押下で「Hierarchy」に生成されます");
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// MapEdittingWindow インスタンスの生成
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static MapEdittingWindow WillAppear(MapEditorMain parent)
    {
        MapEdittingWindow window = (MapEdittingWindow)GetWindow(typeof(MapEdittingWindow), title: null, utility: false, focus: true);
        window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.SetUp(parent);
        return window;
    }
    #endregion

    #region private method
    /// <summary>
    /// MapEditorMain を親画面として画面初期設定する
    /// </summary>
    /// <param name="parent"></param>
    private void SetUp(MapEditorMain parent)
    {
        _parentWindow = parent;
        _gridSize = _parentWindow.GridSize;
        _gridRect = CreateGrid(_parentWindow.Rows, _parentWindow.Columns);
        _mapCells = new MapCell[_parentWindow.Rows, _parentWindow.Columns];

        for (int row = 0; row < _parentWindow.Rows; row++)
        {
            for (int col = 0; col < _parentWindow.Columns; col++)
            {
                _mapCells[row, col] = ScriptableObject.CreateInstance<MapCell>();
                _mapCells[row, col].Initialize(row, col);
            }
        }
    }

    /// <summary>
    /// 画面表示用の矩形配列を作成する
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    /// <returns></returns>
    private Rect[,] CreateGrid(int rows, int columns)
    {
        float posX = 0f;
        float posY = 0f;
        float height = _gridSize;
        float width = _gridSize;

        Rect[,] newGrid = new Rect[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            posX = 0f;
            for (int col = 0; col < columns; col++)
            {
                newGrid[row, col] = new Rect(new Vector2(posX, posY), new Vector2(width, height));
                posX += width;
            }
            posY += height;
        }
        return newGrid;
    }

    /// <summary>
    /// １マスの描画
    /// </summary>
    /// <param name="r"></param>
    private void DrawGrid(Rect r)
    {
        Handles.color = new Color(1f, 1f, 1f, 1f);

        // 上側罫線
        Handles.DrawLine(new Vector2(r.xMin, r.yMin), new Vector2(r.xMax, r.yMin));

        // 下側罫線
        Handles.DrawLine(new Vector2(r.xMin, r.yMax), new Vector2(r.xMax, r.yMax));

        // 左側罫線
        Handles.DrawLine(new Vector2(r.xMin, r.yMin), new Vector2(r.xMin, r.yMax));

        // 右側罫線
        Handles.DrawLine(new Vector2(r.xMax, r.yMin), new Vector2(r.xMax, r.yMax));
    }

    /// <summary>
    /// マップオブジェクトを生成してプレファブ化する
    /// </summary>
    /// <param name="mapCells"></param>
    private void GenerateMapObject(MapCell[,] mapCells)
    {
        var mapObject = new GameObject(_parentWindow.MapName);
        for(int row=0; row < _parentWindow.Rows; row++)
        {
            for (int col = 0; col < _parentWindow.Columns; col++)
            {
                if (mapCells[row, col].PrefabName == "")
                {
                    continue;
                }
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(mapCells[row, col].PrefabName);
                var partObject = Instantiate(prefab, new Vector3(col, -1, -row),Quaternion.identity);
                partObject.name = partObject.name.Replace("(Clone)", "" );
                partObject.transform.SetParent(mapObject.transform);
            }
        }

        // マップのプレファブ化
        if (_parentWindow.IsCreatePrefab)
        {
            string mapPrefabAbsName = "Assets/Prefabs/Stages/" + mapObject.name + ".prefab";
            var prefab = PrefabUtility.SaveAsPrefabAsset(mapObject, mapPrefabAbsName);
            DestroyImmediate(mapObject);
            var mapIns = Instantiate(prefab);
            mapIns.name = mapIns.name.Replace("(Clone)", "");
        }
    }
    #endregion
}