using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleWindow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("GameManager Instance Not Found at GameExitButton.Click()");
            return;
        }
        GameManager.Instance.StartGame();
    }

    public void ExitButton()
    {

    }
}
