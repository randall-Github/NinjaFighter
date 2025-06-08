using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a boss enemy, Boss Possum/Mother Possum
//This boss will do the same thing that a regular Possum does except for charging at the player
//It's main gimmick is that it will spawn Possums
public class BossPossum : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxhlth = 10;
    public int remhealth;
    [SerializeField] int damage = 1;
    public int trueDamage;
    public float enemySpeed = 1.0f;
    public float trueSpeed;
    [SerializeField] float knockbackStrength = 20f;
    public float trueKnockBack;
    private bool isFacingLeft = true;
    public Animator anim;
    [SerializeField] GameObject enemy;
    public Transform groundCheck;
    private bool hit;
    private bool attCooldown;
    [SerializeField] Rigidbody2D rigid;
    public GameObject deathEffect;
    public SpriteRenderer sprite;
    private bool spawnRate;
    private string BossName;
    public GameObject possums;
    public GameObject player;
    public float spawnTimer;
    public GameObject noti;

    private int points;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hit = false;
        attCooldown = false;
        spawnRate = false;
        BossName = "Mother of All Possum";
        remhealth = maxhlth;
        trueSpeed = enemySpeed;
        trueDamage = damage;
        trueKnockBack = knockbackStrength;
    }
    void Update()
    {
        DetectPlatform();
        Movement();
        DetectFallingOff();
        anim.SetBool("Spawning", spawnRate);
        if(hit == true){
            StartCoroutine(damageBlink());
        }
        if(attCooldown == true){
            StartCoroutine(Cooldown());
        }
        if(spawnRate == false){
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= 3f){
                spawnTimer = 0;
                spawnRate = true;
                StartCoroutine(SpawnCooldown());
            }
        }
    }

    private void Movement()
    {
        if(spawnRate == false){
            this.transform.Translate(Vector2.left * trueSpeed * Time.deltaTime);
        }
        if(spawnRate == true){
            rigid.velocity = Vector2.zero;
        }
    }
    private void DetectPlatform()
    {
        if (transform.position.x <= (player.transform.position.x) && isFacingLeft) 
        {
            Flip();
        }
        if (transform.position.x >= (player.transform.position.x) && !isFacingLeft)
        {
            Flip();
        }
    }
    private void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    public void TakeDamage(int damage)
    {
        GameObject other = GameObject.FindGameObjectWithTag("Player");
        remhealth -= damage;
        if (remhealth <= 0)
        {
            Die();
        }
        hit = true;
    }

    public void TakeArrowDamage(int damage)
    {
        remhealth -= damage;
        if (remhealth <= 0)
        {
            Die();
        }
        hit = true;
    }

    public void DetectFallingOff(){
        if(this.transform.position.y <= -5){
            this.Die();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && attCooldown == false)
        {
            other.transform.GetComponent<Player_Control>().CollidedEnemy(this.gameObject);
            other.transform.GetComponent<Player_Control>().TakeDamage(trueDamage);
            attCooldown = true;
        }
    }
    public float getKnockback(){
        return this.trueKnockBack;
    }
    public void SpawnPossums(){
        possums.GetComponent<Enemy>().isFacingLeft = this.isFacingLeft;
        GameObject effect = Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(effect,1f);
        GameObject spawner = Instantiate(possums,transform.position,transform.rotation);
        spawnRate = false;
    }
    private IEnumerator Cooldown(){
        yield return new WaitForSeconds(1f);
        attCooldown = false;
    }
    private IEnumerator damageBlink(){
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        hit = false;
        sprite.color = Color.white;
    }
    private IEnumerator SpawnCooldown(){
        GameObject notification = Instantiate(noti,new Vector2(transform.position.x,transform.position.y + 0.5f),transform.rotation);
        notification.transform.parent = transform;
        Destroy(notification,1f);
        yield return new WaitForSeconds(1f);
        SpawnPossums();
    }

    void Die()
    {
        points = maxhlth / 2;
        Debug.Log("Enemy died!");
        GameObject effect = Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(effect,1f);
        Destroy(this.gameObject);
        GameManager.instance.AddPoints(points);
    }

    public void StatIncrease(){
        maxhlth += 5;
        Debug.Log("Health is now " + remhealth);
        if(enemySpeed <= 4.0f){
        enemySpeed++;
        Debug.Log("Move Speed is " + enemySpeed);
        }
    }
    public void reset(){
        maxhlth = 10;
        enemySpeed = 1.0f;
    }
}


