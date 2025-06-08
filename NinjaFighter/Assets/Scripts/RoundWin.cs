
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;

public class RoundWin : MonoBehaviour
{
    public Text levels;
    public Text tips;

    public GameObject ShopUI;
    public GameObject player;

    public Text upgrade_name;
    public Text upgrade_name2;


    public bool optionSelected = false;

    private int rand1;
    private int rand2;

    private int cur_round;

    //randomly index through this list to get random upgrades
    List<string> upgrades = new List<string>();
    List<string> skills = new List<string>();
    List<string> up_names = new List<string>();
    List<string> skill_names = new List<string>();
    // GameManager gameManager;

    void Start()
    {
        ShopUI.SetActive(false);
        ChangeText();
        upgrades.Add("Add_max_hlth");
        upgrades.Add("Add_damage");
        upgrades.Add("IncreaseJump");
        upgrades.Add("IncreaseSpeed");
        skills.Add("Unlock_arrow");
        skills.Add("Unlock_spear");
        skills.Add("Unlock_dash");
        up_names.Add("Increase Max Health");
        up_names.Add("Increase Damage");
        up_names.Add("Increase Jump");
        up_names.Add("Increase Speed");
        skill_names.Add("Unlock Arrow");
        skill_names.Add("Unlock Spear");
        skill_names.Add("Unlock Dash");
        cur_round = GameManager.instance.getRound();
        rand1 = Random.Range(0, upgrades.Count);
        rand2 = Random.Range(0, upgrades.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.getRound() > 1)
        {
            if (optionSelected == false)
            {
                Pause();
            }
        }

        if (GameManager.instance.getRound() > cur_round)
        {
            optionSelected = false;
        }


    }
    public void Resume()
    {
        if (GameManager.instance.getRound() % 5 != 0 || skills.Count == 1)
        {
            rand1 = Random.Range(0, upgrades.Count);
            rand2 = Random.Range(0, upgrades.Count);
        }
        else
        {
            rand1 = Random.Range(0, skills.Count);
            rand2 = Random.Range(0, skills.Count);
        }
        ShopUI.SetActive(false);
        Time.timeScale = 1f;

    }
    public void ChangeText()
    {
        levels.text = "Completed Round " + GameManager.instance.getRound();
    }
    public void ChangeTips()
    {
        tips.text = "YE 5 More";
    }
    public void MainMenu()
    {
        GameManager.instance.SetMenuStatus(true);
    }

    public void Pause()
    {
        ShopUI.SetActive(true);
        Time.timeScale = 0f;
        if ((GameManager.instance.getRound() - 1) % 5 != 0)
        {
            upgrade_name.text = up_names[rand1];
            upgrade_name2.text = up_names[rand2];
        }
        else
        {
            if (skill_names[rand1] == skill_names[rand2]) {
                rand2 = Random.Range(0, skills.Count);
            }
            upgrade_name.text = skill_names[rand1];
            upgrade_name2.text = skill_names[rand2];
        }
        ChangeText();
    }

    public void NextLevel()
    {
        optionSelected = true;
        cur_round = GameManager.instance.getRound();
        Resume();
    }

    public void ShopUpgrade1()
    {
        player.transform.GetComponent<Player_Control>().ReplenishHealth();
        NextLevel();
    }

    public void ShopUpgrade2()
    {
        if ((GameManager.instance.getRound() - 1) % 5 != 0 || skills.Count == 1)
        {
            player.SendMessage(upgrades[rand1]);
            NextLevel();
        }
        else
        {
            player.SendMessage(skills[rand1]);
            switch (skills[rand1])
            {
                case "Unlock_arrow":
                    upgrades.Add("Add_arrowRate");
                    upgrades.Add("Add_arrowDam");
                    up_names.Add("Faster Arrows");
                    up_names.Add("Add Arrow Damage");
                    skills.Remove("Unlock_arrow");
                    skill_names.Remove("Unlock Arrow");
                    break;
                case "Unlock_spear":
                    upgrades.Add("Add_spearRate");
                    upgrades.Add("Add_spearDam");
                    up_names.Add("Faster Spears");
                    up_names.Add("Add Spear Damage");
                    skills.Remove("Unlock_spear");
                    skill_names.Remove("Unlock Spear");

                    break;
                case "Unlock_dash":
                    upgrades.Add("Add_dash_cooldown");
                    upgrades.Add("Add_DashTime");
                    up_names.Add("Lower Dash Cool Down");
                    up_names.Add("More Dash Time");
                    skills.Remove("Unlock_dash");
                    skill_names.Remove("Unlock Dash");

                    break;
            }
            Debug.Log("Lengh of skill list = " + skills.Count);
            NextLevel();
        }
    }

    public void ShopUpgrade3()
    {
        if ((GameManager.instance.getRound() - 1) % 5 != 0 || skills.Count == 1)
        {
            player.SendMessage(upgrades[rand2]);
            NextLevel();
        }
        else
        {
            player.SendMessage(skills[rand2]);
            switch (skills[rand2])
            {
                case "Unlock_arrow":
                    upgrades.Add("Add_arrowRate");
                    upgrades.Add("Add_arrowDam");
                    up_names.Add("Faster Arrows");
                    up_names.Add("Add Arrow Damage");
                    skills.Remove("Unlock_arrow");
                    skill_names.Remove("Unlock Arrow");
                    break;
                case "Unlock_spear":
                    upgrades.Add("Add_spearRate");
                    upgrades.Add("Add_spearDam");
                    up_names.Add("Faster Spears");
                    up_names.Add("Add Spear Damage");
                    skills.Remove("Unlock_spear");
                    skill_names.Remove("Unlock Spear");

                    break;
                case "Unlock_dash":
                    upgrades.Add("Add_dash_cooldown");
                    upgrades.Add("Add_DashTime");
                    up_names.Add("Lower Dash Cool Down");
                    up_names.Add("More Dash Time");
                    skills.Remove("Unlock_dash");
                    skill_names.Remove("Unlock Dash");

                    break;
            }
        }
        Debug.Log("Lengh of skill list = " + skills.Count);
        NextLevel();
    }
}


