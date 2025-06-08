using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuUI;
    // Start is called before the first frame update
    void Start()
    {
        menuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.GetMenuStatus()){
            Pause();
        }
    }

    public void Controls(){
        menuUI.SetActive(true);
    }
    public void Controls_leave(){
        menuUI.SetActive(false);
    }

    public void Pause(){
        Time.timeScale = 0f;
        menuUI.SetActive(true);
        GameManager.instance.SetMenuStatus(true);
    }

    
}
