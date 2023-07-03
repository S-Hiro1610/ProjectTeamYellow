using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// マップエディタデバッグ用
/// このオブジェクトに Texture2D をアタッチして
/// ツールバーに表示させる
/// </summary>

public class MapParts : MonoBehaviour
{
    #region property
    public Texture2D MapTexture => _mapTexture;
    #endregion

    #region private
    [SerializeField]
    Texture2D _mapTexture;
    #endregion

    #region unity methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
