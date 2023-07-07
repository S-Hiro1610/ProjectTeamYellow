using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    private static EnemyObjectPool _instance;
    private Dictionary<int, List<GameObject>> pooledGameObjects = new Dictionary<int, List<GameObject>>();
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

    private void Start()
    {

    }

    private void Update()
    {

    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    public static EnemyObjectPool Instance
    {
        get
        {
            return _instance;
        }
    }

    // ゲームオブジェクトをpooledGameObjectsから取得する。必要であれば新たに生成する
    public GameObject GetGameObject(GameObject prefab, Vector3 position, int level)
    {
        // プレハブのインスタンスIDをkeyとする
        int key = prefab.GetInstanceID();

        // Dictionaryにkeyが存在しなければ作成する
        if (pooledGameObjects.ContainsKey(key) == false)
        {

            pooledGameObjects.Add(key, new List<GameObject>());
        }

        List<GameObject> gameObjects = pooledGameObjects[key];

        GameObject go = null;

        for (int i = 0; i < gameObjects.Count; i++)
        {

            go = gameObjects[i];

            // 現在非アクティブ（未使用）であれば
            if (go.activeInHierarchy == false)
            {

                // 位置を設定する
                go.transform.position = position;

                // 角度を設定する
                go.transform.rotation = Quaternion.identity;

                // これから使用するのでアクティブにする
                go.SetActive(true);
                PlayerUnitController enemyUnitController = go.GetComponent<PlayerUnitController>();
                PlayerRangeUnitController enemyRangeUnitController = go.GetComponent<PlayerRangeUnitController>();
                if (enemyUnitController != null) enemyUnitController.EnemyInitilize(level);
                else if (enemyRangeUnitController != null) enemyRangeUnitController.EnemyInitilize(level);

                return go;
            }
        }

        // 使用できるものがないので新たに生成する
        go = Instantiate(prefab, position, Quaternion.identity);
        if (level > 1) go.GetComponent<CharactorBase>().SetMaxHP(level);

        // ObjectPoolゲームオブジェクトの子要素にする
        go.transform.parent = transform;

        // リストに追加
        gameObjects.Add(go);

        return go;
    }

    // ゲームオブジェクトを非アクティブにする。こうすることで再利用可能状態にする
    public void ReleaseGameObject(GameObject go)
    {
        // 非アクティブにする
        go.SetActive(false);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
