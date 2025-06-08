using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject menuUI;
    // Start is called before the first frame update
    void Start()
    {
        menuUI.SetActive(false);
        if (GameManager.instance.GetMenuStatus() == false){
            Resume();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(GameManager.instance.GetMenuStatus());
        if(GameManager.instance.GetMenuStatus()){
            Pause();
        }
        
    }
    public void Pause(){
        Time.timeScale = 0f;
        menuUI.SetActive(true);
        GameManager.instance.SetMenuStatus(true);
    }
    public void Resume(){
        Time.timeScale = 1f;
        menuUI.SetActive(false);
        GameManager.instance.SetMenuStatus(false);
    }

    
}
