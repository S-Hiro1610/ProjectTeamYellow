using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickA(string name)
    {
        SceneController.LoadScene(name);
    }

    public void ClickB(string name)
    {
        SceneController.ChangeActiveScene(name);
    }
}
