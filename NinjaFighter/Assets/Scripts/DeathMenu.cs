using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathUI;
    public Text tRound;
    private static bool gameIsPaused = false;
    public bool deathCheck;
    // public GameObject menuUI;

    // Start is called before the first frame update
    void Start()
    {
        deathUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.getDeath() == true){
            Die();
        }
    }
    public void Die(){
        deathUI.SetActive(true);
        gameIsPaused = true;
    }
    public void Restart(){
        deathUI.SetActive(false);
        SceneManager.LoadScene(0);
        GameManager.instance.SetMenuStatus(false);
        // Debug.Log(GameManager.instance.GetMenuStatus());
    }

    public void QuitButton(){
        deathUI.SetActive(false);
        SceneManager.LoadScene(0);
        GameManager.instance.SetMenuStatus(true);
    }
}
