
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] private Text round_text; // display current game round
    //[SerializeField] private Text enemy_Count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRound(int new_round)
    {
        round_text.text = "Round " + new_round;
    }
}
