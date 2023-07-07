using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    public float Speed => _speed;
    public bool MoveFlg => _moveFlag;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _moveFlag = true;
    #endregion

    #region private
    // プライベートなメンバー変数。
    // ルート座標リスト(仮)
    private List<Vector3> _rootList = new List<Vector3>();
    // 現在地座標番号
    private int _rootIndex = 0;
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
        // 初期値設定
        _rootIndex = 0;
        _rootList.Add(new Vector3(0, 1, 0));
        // 初期位置
        transform.position = _rootList[_rootIndex];

        // ルート設定(仮で手動設定)
        _rootList.Add(new Vector3(-2, 1, 0));
        _rootList.Add(new Vector3(-2, 1, 3));
        _rootList.Add(new Vector3(-4, 1, 3));
        _rootList.Add(new Vector3(-4, 1, 1));
        _rootList.Add(new Vector3(-6, 1, 1));
        _rootList.Add(new Vector3(-6, 1, 4));
        _rootList.Add(new Vector3(-8, 1, 4));
        _rootList.Add(new Vector3(-8, 1, 1));
        _rootList.Add(new Vector3(-9, 1, 1));

        // 向きの設定
        _rootIndex++;
        transform.LookAt(_rootList[_rootIndex]);
    }

    private void Update()
    {
        // 移動フラグを確認
        if (_moveFlag)
        {
            // 目的地へ移動
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _rootList[_rootIndex], step);

            // 目的地に到達後、次のルート先を設定
            if (transform.position == _rootList[_rootIndex])
            {
                // 現在地座標番号がルートリスト数より小さければ、次の座標番号に移る
                if (_rootIndex < (_rootList.Count -1))
                {
                    _rootIndex++;
                    transform.LookAt(_rootList[_rootIndex]);
                }
                else
                {
                    _moveFlag = false;
                }
 
            }
        }
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    // アクティブになった際、位置の初期化を行う(オブジェクトプール用)
    private void OnEnable()
    {
        // 初期値設定
        _rootIndex = 0;
        // 初期位置
        transform.position = _rootList[_rootIndex];
        // 向きの設定
        _rootIndex++;
        transform.LookAt(_rootList[_rootIndex]);
    }

    #endregion
}
