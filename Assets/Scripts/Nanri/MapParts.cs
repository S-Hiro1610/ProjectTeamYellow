using System.Collections;
using System.Collections.Generic;
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

    #region private
    [SerializeField]
    private Texture2D _mapTexture;
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


