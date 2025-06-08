
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text points;
    public int current_round;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(int score) {
        // current_round = score;
        gameObject.SetActive(true);
        score--;
        points.text = score.ToString() + " Rounds.";
    }

    public void Restart() {
        SceneManager.LoadScene(3);
    }

    public void StartNewGame() {
        SceneManager.LoadScene(3);
    }

    public void MainMenu() {
        SceneManager.LoadScene(1);
    }
}
