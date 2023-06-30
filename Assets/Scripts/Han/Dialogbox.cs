using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialogbox : MonoBehaviour
{
    public Button ExitButton;

    public UnityEvent OnOpenEvent;
    public UnityEvent OnCloseEvent;
    // Start is called before the first frame update
    void Start()
    {
        ExitButton.onClick.AddListener(() => { SetActive(false); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActive(bool onoff)
    {
        //gameObject.transform.parent.gameObject.SetActive(onoff);

        if(onoff)
        {
            Open();
            return;
        }
        Close();
    }

    public void Open()
    {
        Debug.Log("Open");
        gameObject.transform.parent.gameObject.SetActive(true);
        OnOpenEvent.Invoke();
    }

    public void Close()
    {
        Debug.Log("Close");
        gameObject.transform.parent.gameObject.SetActive(false);
        OnCloseEvent.Invoke();
    }
}
