using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObjectPool : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public List<GameObject> CreateUnits => _createUnits;
    public int CreateUnitCount => _createUnitCount;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private List<GameObject> _createUnits;
    [SerializeField]
    private int _createUnitCount;
    #endregion

    #region private
    // プライベートなメンバー変数。
    private static UnitObjectPool _instance;
    private Dictionary<int, List<GameObject>> pooleUnits = new Dictionary<int, List<GameObject>>();
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

        // 事前にユニットを指定分生成
        CreateUnitInitilize();
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public static UnitObjectPool Instance
    {
        get
        {
            return _instance;
        }
    }

    // ゲームオブジェクトをpooledGameObjectsから取得する。必要であれば新たに生成する
    public GameObject SpawneUnit(GameObject prefab, Vector3 position, int level)
    {
        // プレハブのインスタンスIDをkeyに設定
        int key = prefab.GetInstanceID();

        // Dictionaryにkeyが存在しなければ作成
        if (pooleUnits.ContainsKey(key) == false)
        {

            pooleUnits.Add(key, new List<GameObject>());
        }

        List<GameObject> gameObjects = pooleUnits[key];

        GameObject unit = null;

        for (int i = 0; i < gameObjects.Count; i++)
        {

            unit = gameObjects[i];

            // 現在非アクティブ（未使用）であれば
            if (unit.activeInHierarchy == false)
            {

                // 位置を設定する
                unit.transform.position = position;

                // 角度を設定する
                unit.transform.rotation = Quaternion.identity;

                // これから使用するのでアクティブにする
                unit.SetActive(true);
                PlayerUnitController enemyUnitController = unit.GetComponent<PlayerUnitController>();
                PlayerRangeUnitController enemyRangeUnitController = unit.GetComponent<PlayerRangeUnitController>();
                PlayerAreaUnitController areaUnitController = unit.GetComponent<PlayerAreaUnitController>();
                if (enemyUnitController != null) enemyUnitController.UnitInitilize(level);
                else if (enemyRangeUnitController != null) enemyRangeUnitController.UnitInitilize(level);
                else if (areaUnitController != null) areaUnitController.UnitInitilize(level);

                return unit;
            }
        }

        // 使用できるものがないので新たに生成する
        unit = Instantiate(prefab, position, Quaternion.identity);
        // Lvが1より大きい場合はパラメータを設定
        if (level > 1)
        {
            PlayerUnitController enemyUnitController = unit.GetComponent<PlayerUnitController>();
            PlayerRangeUnitController enemyRangeUnitController = unit.GetComponent<PlayerRangeUnitController>();
            PlayerAreaUnitController areaUnitController = unit.GetComponent<PlayerAreaUnitController>();
            if (enemyUnitController != null) enemyUnitController.UnitInitilize(level);
            else if (enemyRangeUnitController != null) enemyRangeUnitController.UnitInitilize(level);
            else if (areaUnitController != null) areaUnitController.UnitInitilize(level);
        }

        // ObjectPoolゲームオブジェクトの子要素にする
        unit.transform.parent = transform;

        // リストに追加
        gameObjects.Add(unit);

        return unit;
    }

    // ゲームオブジェクトを非アクティブにする。こうすることで再利用可能状態にする
    public void ReleaseGameObject(GameObject unit)
    {
        // 非アクティブにする
        unit.SetActive(false);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private void CreateUnitInitilize()
    {
        foreach(GameObject createUnit in CreateUnits)
        {
            // 指定数分生成
            for (int i = 0;i < CreateUnitCount;i++)
            {
                // プレハブのインスタンスIDをkeyに設定
                int key = createUnit.GetInstanceID();

                // Dictionaryにkeyが存在しなければ作成
                if (pooleUnits.ContainsKey(key) == false)
                {

                    pooleUnits.Add(key, new List<GameObject>());
                }

                List<GameObject> gameObjects = pooleUnits[key];

                // ユニットを生成し、非アクティブにする
                GameObject unit = Instantiate(createUnit, Vector3.zero, Quaternion.identity);
                unit.SetActive(false);

                // ObjectPoolゲームオブジェクトの子要素にする
                unit.transform.parent = transform;

                // リストに追加
                gameObjects.Add(unit);
            }
        }
    }
    #endregion
}
