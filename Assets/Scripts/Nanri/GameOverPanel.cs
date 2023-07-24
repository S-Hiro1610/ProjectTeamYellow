using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    #region property

    // Game Over Jingle のインデクス。GameOverWindowがセットする。
    public int SeIndex
    {
        get { return _seIndex; }
        set { _seIndex = value; }
    }
        
    #endregion

    #region serialize
    [SerializeField]
    private int _seIndex;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if (AudioPlayer.Instance == null)
        {
            Debug.Log("Audio Player is not found!");
        }
        else
        {
            // ゲームオーバージングルを一回流す
            AudioPlayer.Instance.SEPlay(_seIndex);
        }
    }
}
