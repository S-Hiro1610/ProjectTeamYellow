using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public List<WaveList> Wave => _wave;
    public Transform SpawnePoint => _spawnePoint;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private List<WaveList> _wave;
    [SerializeField]
    private Transform _spawnePoint;
    #endregion

    #region private
    // プライベートなメンバー変数。
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

    }

    private void Start()
    {
        // Wave1を開始
        StartCoroutine(WaveEnemy(0));
    }

    private void Update()
    {

    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    IEnumerator WaveEnemy(int waveindex)
    {
        int waveEnemyCount = _wave[waveindex].waveEnemy.Count;

        for (int i = 0;i < waveEnemyCount; i++)
        {
            yield return new WaitForSeconds(_wave[waveindex].waveEnemy[i].SpawneDelay);
            EnemyObjectPool.Instance.GetGameObject(_wave[waveindex].waveEnemy[i].Enemy, _spawnePoint.position, _wave[waveindex].waveEnemy[i].Level);
        }
    }
    #endregion
}
