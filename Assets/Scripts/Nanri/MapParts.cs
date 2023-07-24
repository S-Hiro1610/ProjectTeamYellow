using UniRx;
using UnityEngine;

/// <summary>
/// マップエディタ用UI
/// このオブジェクトに Texture2D をアタッチして
/// ツールバーに表示させる
/// </summary>
public enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
    None = -1
}

public class MapParts : MonoBehaviour
{
    #region property
    public Texture2D MapTexture => _mapTexture;
    public Direction NextDirection = Direction.None;
    #endregion

    #region serialize
    [SerializeField]
    private Texture2D _mapTexture;
    [SerializeField]
    private bool _isSelectMode = false;
    #endregion
    #region private
    ParticleSystem _stagingEffect = null;
    private bool _effectStarted = false;
    #endregion

    #region unity methods

    // Start is called before the first frame update
    void Start()
    {
        // Playerユニットをセット可能なマスならパーティクルシステムを有効にする。
        if (GetComponent<StageBlock>().isPlayerUnitSet)
        {
            _stagingEffect = GetComponent<ParticleSystem>();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.SelectMode.Subscribe(selectMode => { _isSelectMode = (selectMode == SELSCT_MODE.SELECT_MOD_NO) ? false : true; });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // UIのモードが「ユニット配置」ならマスのエフェクトを再生
        if (_isSelectMode)
        {
            if (!_effectStarted)
            {
                if (_stagingEffect != null)
                {
                    _effectStarted = true;
                    _stagingEffect.Play();
                }
            }
        }
        else
        {
            // UIのモードが「その他」でエフェクト開始後ならマスのエフェクトを停止

            if (_effectStarted)
            {
                _effectStarted = false;
                _stagingEffect.Stop();
            }
        }
    }
    #endregion
    #region private method
    #endregion
}


