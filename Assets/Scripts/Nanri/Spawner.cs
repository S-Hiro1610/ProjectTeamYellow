using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stage テスト用 Spawner
/// Wtanabe/EnemyObjectPoolとセットでSpawnerへアタッチして使う
/// Spawnerの直上(y=0)に SpawnPoint を作成して、スクリプトにアタッチする 
/// </summary>
public class Spawner : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    #endregion

    #region serialize
    // unity inspectorに表示したいものを記述。
    [SerializeField]
    private List<Vector3> _enemyRoute;

    #endregion

    #region private
    // プライベートなメンバー変数。
    private Vector3 _spawnPoint;
    #endregion

    #region Constant
    // 定数をいれる。
    #endregion

    #region Event
    //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #endregion

    #region unity methods
    //
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint = _enemyRoute[0];
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    /// <summary>
    /// Stage Manager が SpawnPoint ごとの進軍ルートを設定する為に呼ぶ。
    /// 呼ばれる都度、上書きする。
    /// </summary>
    /// <param name="rt"></param>    
    public void SetRoute(List<Vector3> rt)
    {
        _enemyRoute = rt;
    }

    /// <summary>
    /// エネミーを生成する
    /// </summary>
    /// <param name="Enemy">エネミー</param>
    /// <param name="Level">レベル</param>
    public void WaveEnemySpawne(GameObject Enemy, int Level)
    {
        var go = UnitObjectPool.Instance.SpawneUnit(Enemy, _spawnPoint, Level);
        go.GetComponent<EnemyMove>().SetRoute(_enemyRoute);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
