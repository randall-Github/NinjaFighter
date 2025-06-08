using System.Collections;
using UnityEngine;


//This will be a flying enemy, Eagle Enemy
//It will fly outside of the Player's view and then dive at the Player
public class FlyingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxhlth = 2;
    public int remhealth;
    [SerializeField] int damage = 1;
    public int trueDamage;
    public float enemySpeed = 5.0f;
    public float trueSpeed;
    private bool isFacingLeft = true;
    [SerializeField] GameObject enemy;
    public Transform groundCheck;
    private Animation anim;
    private bool diving = false;
    private GameObject player;
    private float diveTarget;
    private bool knock;
    private bool hit;
    [SerializeField] Rigidbody2D rigid;
    public SpriteRenderer sprite;
    public GameObject deathEffect;
    private bool attCooldown;
    [SerializeField] float knockbackStrength = 3f;
    public float trueKnockBack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attCooldown = false;
        knock = false;
        hit = false;
        remhealth = maxhlth;
        trueDamage = damage;
        trueSpeed = enemySpeed;
        trueKnockBack = knockbackStrength;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        DetectPlatform();
        if(knock == true){
            StartCoroutine(KnockTimer(1.5f));
        }
        if(hit == true){
            StartCoroutine(DamageBlink());
        }
        if(attCooldown == true){
            StartCoroutine(Cooldown());
        }
    }
    private void Movement(){
        if(diving == false && isFacingLeft == true && !knock){
            this.transform.position = Vector2.MoveTowards(transform.position, new Vector2((diveTarget - 12f),Random.Range(3f,6f)),trueSpeed * Time.deltaTime);
        }
        if(diving == false && isFacingLeft == false && !knock){
            this.transform.position = Vector2.MoveTowards(transform.position, new Vector2((diveTarget + 12f),Random.Range(3f,6f)),trueSpeed * Time.deltaTime);
        }
        if(diving == true){
            this.transform.position = Vector2.MoveTowards(transform.position, new Vector2(diveTarget,0.0f), (trueSpeed * 1.5f) * Time.deltaTime);
            if(this.transform.position.y <= groundCheck.position.y + (groundCheck.localScale.y/2)+2.1f){
                diving = false;
            }
        }
        if (transform.position.x <= (diveTarget - 11.5f) && isFacingLeft && diving == false) 
        {
            if(transform.position.x < player.transform.position.x){
                Flip();
            }
            Diving();
        }
        if (transform.position.x >= (diveTarget + 11.5f)  && !isFacingLeft && diving == false)
        {
            if(transform.position.x > player.transform.position.x){
                Flip();
            }
            Diving();
        }
    }
    private void DetectPlatform(){
        if (transform.position.x <= (player.transform.position.x - 10f) && isFacingLeft) 
        {
            Flip();
            Diving();
        }
        if (transform.position.x >= (player.transform.position.x + 10f)  && !isFacingLeft)
        {
            Flip();
            Diving();
        }
    }
    public float getKnockback(){
        return this.trueKnockBack;
    }
    private void Diving(){
        diveTarget = player.transform.position.x;
        diving = true;
    }
    private void Flip(){
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    public void TakeDamage(int damage)
    {
        remhealth -= damage;
        hit = true;
        if (remhealth <= 0)
        {
            Die();
        }
        KnockBack(player);
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


    public void KnockBack(GameObject other){
        Vector2 direction = new Vector2(transform.position.x - other.transform.position.x,transform.position.y+2f).normalized;
        rigid.AddForce(direction*8,ForceMode2D.Impulse);
        knock = true;
    }
    private IEnumerator KnockTimer(float f){
        yield return new WaitForSeconds(f);
        knock = false;
        diving = false;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0f;
    }
    private IEnumerator DamageBlink(){
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        hit = false;
        sprite.color = Color.white;
    }
    private IEnumerator Cooldown(){
        yield return new WaitForSeconds(1f);
        attCooldown = false;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && diving == true && attCooldown == false)
        {
            other.transform.GetComponent<Player_Control>().CollidedEnemy(this.gameObject);
            other.transform.GetComponent<Player_Control>().TakeDamage(trueDamage);
            attCooldown = true;
        }
    }
    void Die()
    {
        GameObject effect = Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(effect,1f);
        Destroy(this.gameObject);
        GameManager.instance.AddPoints(1);
    }

    public void StatIncrease(){
        maxhlth += 2;
        remhealth = maxhlth;
        //Debug.Log("Health is now " + maxhlth);
        if(enemySpeed <= 8.0f){
        enemySpeed++;
        trueSpeed = enemySpeed;
        //Debug.Log("Move Speed is " + enemySpeed);
        }
    }

    public void reset(){
        maxhlth = 2;
        enemySpeed = 5.0f;
    }
}
