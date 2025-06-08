
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject player;
    public int current_round = 1;
    public Text roundTrack;

    //Tracking boss rounds
    public bool death;
    public bool menuStatus;

    // Keeps track and update score
    public Text scoreText;
    private int score = 0;

    // Awake is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }
    void Start() {
        SetText();
        menuStatus = true;

        SetScoreText();
    }

    public bool GetMenuStatus(){
        return this.menuStatus;
    }

    public void SetMenuStatus(bool status){
        menuStatus = status;
    }

    public void SetText(){
        roundTrack.text = "Completed Rounds: " + getRound();
    }

    private void SetScoreText(){
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddPoints(int scoreToAdd){
        score += scoreToAdd;
        //Debug.Log("current score is " + score);
        SetScoreText();
    }

    public void NextRound(int newRound){
        current_round = newRound;
        SetText();
    }

    public void Death() {
        death = true;
    }

    public bool getDeath(){
        return this.death;
    }

    public int getRound()
    {
        return this.current_round;
    }
}
