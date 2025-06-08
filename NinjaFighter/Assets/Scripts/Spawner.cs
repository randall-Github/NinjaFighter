using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // prefabs for enemies
    public GameObject BasicRatEnemy;
    public GameObject BasicFrogEnemy;
    public GameObject FlyingEnemy;
    public GameObject FoxBossEnemy;
    public GameObject SwordBossEnemy;
    public GameObject VikingBossEnemy;
    public GameObject RatBossEnemy;

    public GameObject player;

    private int enemy_Count;
    private int spawnRate = 2;
    private int Boss_spawnRate = 1;
    private int Boss_Count;

    private int next_Round = 0;
    private int round;
    private int current_round = 1;

    [SerializeField]
    private UIUpdater ui_updater;

    // for statIncrease function
    [SerializeField] private Enemy ratEnemy;
    [SerializeField] private FrogEnemy frogEnemy;
    [SerializeField] private FlyingEnemy flyingEnemy;
    [SerializeField] private FoxEnemy foxEnemy;
    [SerializeField] private SwordBoss swordBoss;
    [SerializeField] private VikingEnemy vikingEnemy;
    [SerializeField] private BossPossum bossPossum;

    private float firstCall = 1f;
    private float repeatCall = 1f;
    private int rando;
    private int randBasic; // 1 = Rat, 2 = Frog
    private int randBoss; // 1 = Fox, 2 = Sword, 3 = Viking, 4 = RatBoss
    private int randPos; // 1 = left spawn, 2 = right spawn
    
    // summoned bool for bosses
    private bool foxSummoned;
    private bool swordSummoned;
    private bool vikingSummoned;
    private bool bossSummoned;

    void Start(){
        next_Round = 1;
        enemy_Count = 2;
        Boss_Count = 1;
        spawnRate = 2;

        foxSummoned = false;
        swordSummoned = false;
        vikingSummoned = false;
        bossSummoned = false;

        ratEnemy.reset();
        frogEnemy.reset();
        flyingEnemy.reset();
        foxEnemy.reset();
        swordBoss.reset();
        vikingEnemy.reset();
        bossPossum.reset();

        rngSpawn();

        InvokeRepeating("SpawnRatEnemy", firstCall, repeatCall);
        InvokeRepeating("SpawnFrogEnemy", firstCall, repeatCall);
        InvokeRepeating("SpawnFlyingEnemy", firstCall, repeatCall + 2);
        InvokeRepeating("SpawnBoss", firstCall, repeatCall + 2);
    }

    void Update(){    }

    private bool EnemyIsAlive(){
        if(GameObject.FindGameObjectWithTag("Enemy") == null && GameObject.FindGameObjectWithTag("FrogEnemy") == null &&
        GameObject.FindGameObjectWithTag("FlyingEnemy") == null && GameObject.FindGameObjectWithTag("FoxEnemy") == null &&
        GameObject.FindGameObjectWithTag("SwordBoss") == null && GameObject.FindGameObjectWithTag("VikingBoss") == null &&
        GameObject.FindGameObjectWithTag("PossumBoss") == null){
            Debug.Log("All Enemies dead");
            return false;
            }       
        return true;
    }

    public void SpawnRatEnemy(){
        if(randPos == 1){
            rando = Random.Range(-16, -10);
        }
        if(randPos == 2){
            rando = Random.Range(10, 16);
        }

        Vector2 spawnPosition = new Vector2(player.transform.position.x + rando, 0f);

        if(randBasic == 1){
            if(enemy_Count != 0 && current_round % 5 != 0.0f){
                Debug.Log("Spawning Rat Enemy");
                Instantiate(BasicRatEnemy, spawnPosition, transform.rotation);
                enemy_Count--;
                rngSpawn();
            }
        }   

        if(enemy_Count == 0 && !EnemyIsAlive()){
            //Debug.Log("Starting next round" + current_round);
            repeatCall = 2;
            nextRound();
            if(current_round % 5 == 0.0f){
                //Debug.Log("Increasing stat");
                basicStatIcrease();
            }

        }
    }

    public void SpawnFrogEnemy(){
        if(randPos == 1){
            rando = Random.Range(-10, -7);
        }
        if(randPos == 2){
            rando = Random.Range(7, 10);
        }
        Vector2 spawnPosition = new Vector2(player.transform.position.x + rando, 0f);

        if(randBasic == 2){
            if(enemy_Count != 0 && current_round % 5 != 0.0f){
                Debug.Log("Spawning Frog Enemy");
                Instantiate(BasicFrogEnemy, spawnPosition, transform.rotation);
                enemy_Count--;
                rngSpawn();
            }
        }

        if(enemy_Count == 0 && !EnemyIsAlive()){
            //Debug.Log("Starting next round");
            repeatCall = 2;
            nextRound();
            if(current_round % 5 == 0.0f){
                Debug.Log("Increasing stat");
                basicStatIcrease();
            }
        }  
    }

    public void SpawnFlyingEnemy(){
        if(randPos == 1){
            rando = Random.Range(-24, -12);
        }
        if(randPos == 2){
            rando = Random.Range(12, 24);
        }
        Vector2 spawnPosition = new Vector2(player.transform.position.x + rando, 1f);

        if(enemy_Count != 0 && next_Round >= 3 && current_round % 5 != 0.0f){
            Debug.Log("Spawning Flying Enemy");
            Instantiate(FlyingEnemy, spawnPosition, transform.rotation);
            enemy_Count--;
        }
        
        if(enemy_Count == 0 && !EnemyIsAlive()){
            Debug.Log("Starting next round");
            repeatCall = 2;
            nextRound();
            if(current_round % 5 == 0.0f){
                basicStatIcrease();
            }
        }
    }

    public void SpawnBoss(){
        // spawns fox
        if(randPos == 1){
            rando = Random.Range(-10, -5);
        }
        if(randPos == 2){
            rando = Random.Range(6, 11);
        }
        Vector2 spawnPosition = new Vector2(player.transform.position.x + rando, 1.0f);

        if((randBoss == 2 || randBoss == 3) && next_Round == 5){
            rngSpawn();
        }

        // spawns fox boss
        if(randBoss == 1){
            if(Boss_Count != 0 && current_round % 5 == 0.0f){
                Debug.Log("Spawning BossEnemy");
                Instantiate(FoxBossEnemy, spawnPosition, transform.rotation);
                foxSummoned = true;
                Boss_Count--;
                rngSpawn();
            }
        }

        // spawns sword boss
        else if(randBoss == 2 && next_Round > 5){
            if(Boss_Count != 0 && current_round % 5 == 0.0f){
                Debug.Log("Spawning BossEnemy");
                Instantiate(SwordBossEnemy, spawnPosition, transform.rotation);
                swordSummoned = true;
                Boss_Count--;
                rngSpawn();
            }
        }

        // spawns viking boss
        else if(randBoss == 3 && next_Round > 5){
            spawnPosition = new Vector2(player.transform.position.x + rando, 0.0f);
            if(Boss_Count != 0 && current_round % 5 == 0.0f){
                Debug.Log("Spawning BossEnemy");
                Instantiate(VikingBossEnemy, spawnPosition, transform.rotation);
                vikingSummoned = true;
                Boss_Count--;
                rngSpawn();
            }    
        }

        // spawns Rat boss
        else if(randBoss == 4){
            if(Boss_Count != 0 && current_round % 5 == 0.0f){
                Debug.Log("Spawning BossEnemy");
                Instantiate(RatBossEnemy, spawnPosition, transform.rotation);
                bossSummoned = true;
                Boss_Count--;
                rngSpawn();
            }    
        }

        if(Boss_Count == 0 && !EnemyIsAlive()){
            Debug.Log("Starting next round");
            repeatCall = 2;
            nextRound();
            if(current_round % 5 == 0.0f && next_Round > 5){
                bossStatIncrease();
            }
        }    
    }

    private void nextRound(){
        next_Round++;
        current_round++;
        spawnRate++;
        enemy_Count = current_round + 1;
        if(current_round % 5 == 0 && next_Round > 5){
            Boss_spawnRate++;
            Boss_Count = Boss_spawnRate - 1;
        }
        Debug.Log("new round");
        round = next_Round;
        ui_updater.UpdateRound(round);
        GameManager.instance.NextRound(round);
    }

    private void rngSpawn(){
        randBasic = Random.Range(1,3);
        randBoss = Random.Range(1,5);
        randPos = Random.Range(1,3);
    }

    private void basicStatIcrease(){
        ratEnemy.StatIncrease();
        frogEnemy.StatIncrease();
        flyingEnemy.StatIncrease();
    }

    private void bossStatIncrease(){
        foxEnemy.StatIncrease();
        swordBoss.StatIncrease();
        vikingEnemy.StatIncrease();
        bossPossum.StatIncrease();
    }
}
