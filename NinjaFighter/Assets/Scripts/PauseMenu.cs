
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    private bool gameIsPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            if(gameIsPaused == true){
                Resume();
            }
            else{
                Pause();
            }
        }
    }
    void Pause(){
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        // Debug.Log(Time.timeScale);
        gameIsPaused = true;
    }
    void Resume(){
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void ResumeButton(){
        Resume();
    }
    public void QuitButton(){
        SceneManager.LoadScene(0);

    }
}
