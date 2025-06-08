
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public int health;
    [SerializeField]public int numHearts;

    public Player_Control player;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Start(){
        health = player.trueHealth;
        numHearts = player.health;
    }
    void Update() {
        health = player.trueHealth;
        numHearts = player.health;

        for (int i = 0; i < hearts.Length; i++) {
            if (i<numHearts){
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }


            if (i<health) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

        }
    }

}
