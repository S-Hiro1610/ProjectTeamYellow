using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        // マップ生成ボタンの表示（IsCreatePrefab のチェックが入った時だけ生成可能）
        EditorGUI.BeginDisabledGroup(!_parentWindow.IsCreatePrefab);

        GUILayout.FlexibleSpace();
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            if (GUILayout.Button("Create Prefab", GUILayout.MinWidth(120), GUILayout.MinHeight(40)))
            {
                GenerateMapObject(_mapCells);
            }
            GUILayout.Space(20);
            GUILayout.Label("If create Prefab, IsCreatePrefab must be checked.", GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
        }

        // 区切りの表示
        GUILayout.Space(10);
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            GUILayout.Label("--- または ---", GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);

        // Save マップデザインボタンの表示
        //using (new EditorGUILayout.HorizontalScope())
        //{
        //    GUILayout.Space(20);
        //    if (GUILayout.Button("Save", GUILayout.MinWidth(120)))
        //    {
        //        //ファイルがセットされている場合
        //        if (_mapSaveFile != null)
        //        {
        //            //LoadStageDataFile();
        //        }
        //    }
        //    GUILayout.Space(10);
        //    GUILayout.Label("Specify folder to save ->", GUILayout.Width(200));
        //    GUILayout.Space(5);
        //    _mapSaveFile = EditorGUILayout.ObjectField(_mapSaveFile, typeof(Object), false);
        //    GUILayout.FlexibleSpace();
        //}
        // マップデザインロードボタンの表示
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Space(20);
            if (GUILayout.Button("Load", GUILayout.MinWidth(120)))
            {
                //ファイルがセットされている場合
                if (_mapSaveFile != null)
                {
                    //LoadStageDataFile();
                }
            }
            GUILayout.Space(10);
            GUILayout.Label("Specify file from load ->", GUILayout.Width(200));
            GUILayout.Space(5);
            _mapSaveFile = EditorGUILayout.ObjectField(_mapSaveFile, typeof(Object), false);
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
        List<GameObject> spawnerList = new List<GameObject>();

        for (int row = 0; row < _parentWindow.Rows; row++)
        {
            for (int col = 0; col < _parentWindow.Columns; col++)
            {
                if (mapCells[row, col].PrefabName == "")
                {
                    continue;
                }
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(mapCells[row, col].PrefabName);
                var partObject = Instantiate(prefab, new Vector3(col, -1, -row), Quaternion.identity);
                partObject.name = partObject.name.Replace("(Clone)", "");
                partObject.transform.SetParent(mapObject.transform);

                // Spawnerマスへの進行方向設定
                if (partObject.name.Contains("Spawn"))
                {
                    spawnerList.Add(partObject);
                    if (row > 0)
                    {
                        if (mapCells[row - 1, col].PrefabName != "")
                        {
                            var upPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(mapCells[row - 1, col].PrefabName);
                            if (upPrefab.GetComponent<StageBlock>().isEnemyRoute)
                            {
                                partObject.GetComponent<MapParts>().NextDirection = Direction.Up;
                            }
                        }
                    }
                    if (row < _parentWindow.Rows - 1)
                    {
                        if (mapCells[row + 1, col].PrefabName != "")
                        {
                            var upPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(mapCells[row + 1, col].PrefabName);
                            if (upPrefab.GetComponent<StageBlock>().isEnemyRoute)
                            {
                                partObject.GetComponent<MapParts>().NextDirection = Direction.Down;
                            }
                        }
                    }
                    if (col > 0)
                    {
                        if (mapCells[row, col - 1].PrefabName != "")
                        {
                            var upPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(mapCells[row, col - 1].PrefabName);
                            if (upPrefab.GetComponent<StageBlock>().isEnemyRoute)
                            {
                                partObject.GetComponent<MapParts>().NextDirection = Direction.Left;
                            }
                        }
                    }
                    if (col < _parentWindow.Columns - 1)
                    {
                        if (mapCells[row, col + 1].PrefabName != "")
                        {
                            var upPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(mapCells[row, col + 1].PrefabName);
                            if (upPrefab.GetComponent<StageBlock>().isEnemyRoute)
                            {
                                partObject.GetComponent<MapParts>().NextDirection = Direction.Right;
                            }
                        }
                    }
                }
            }
        }

        // 総ての spawner に進軍リストを追加する。
        foreach (GameObject spawner in spawnerList)
        {
            // 進軍ルートの探索
            // この Spawner の座標から SpawnPoint（y+1）を求める。
            Vector3 spawnPointPosition = spawner.transform.position;
            spawnPointPosition.y += 1;

            // この Spawner の routeList を生成して、先頭に初期出現座標(盤面外)とSpawnPointを設定する。
            var routeList = new List<Vector3>();
            routeList.Add(spawnPointPosition);

            // EndLine までのルートを探索して、routeList へ追加してゆく。
            Direction nextDirection = spawner.GetComponent<MapParts>().NextDirection;
            Transform nextMapCell = spawner.transform;
            while (nextDirection != Direction.None)
            {
                var nextPosition = nextMapCell.position;
                switch (nextDirection)
                {
                    case Direction.Up:
                        nextPosition.z += 1;
                        break;

                    case Direction.Right:
                        nextPosition.x += 1;
                        break;

                    case Direction.Down:
                        nextPosition.z -= 1;
                        break;

                    case Direction.Left:
                        nextPosition.x -= 1;
                        break;
                }
                nextMapCell = FindAtPosition(mapObject.transform, nextPosition);

                // 敵ユニットの進軍位置は、MapCellの直上（ｙ＋１）
                var nextEnemyPosition = nextMapCell.position;
                nextEnemyPosition.y += 1;
                routeList.Add(nextEnemyPosition);
                nextDirection = nextMapCell.GetComponent<MapParts>().NextDirection;
            }

            // routeList を Spawner に SetRoute() する。
            spawner.GetComponent<Spawner>().SetRoute(routeList);
        }

        // マップオブジェクトをFrefab化して、Hierarchy上にクローンを表示する。 
        string mapPrefabAbsName = "Assets/Prefabs/Stages/" + mapObject.name + ".prefab";
        var mapPrefab = PrefabUtility.SaveAsPrefabAsset(mapObject, mapPrefabAbsName);
        DestroyImmediate(mapObject);
        var mapIns = Instantiate(mapPrefab);
        mapIns.name = mapIns.name.Replace("(Clone)", "");
    }

    /// <summary>
    /// Stage Objcet 子の中から 指定された座標のものを見つけて返す
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Transform FindAtPosition(Transform parent, Vector3 pos)
    {
        foreach (Transform child in parent)
        {
            if (child.position == pos)
                return child;
        }
        return null;
    }

    /// <summary>
    /// 生成前のマップ構造を Scriptable Object として保存する
    /// あとでロードして編集画面を再表示できる
    /// </summary>
    private void SaveMapDesign()
    {

    }

    /// <summary>
    /// Scriptable Object としてセーブされているマップ構造をロードして編集画面に反映する
    /// </summary>
    private void LoadMapDesign()
    {

    }
    #endregion
}