using System.Collections;
using UnityEngine;


//This will be a regular enemy, Frog Enemy
//It will continually hop at the Player
public class FrogEnemy : MonoBehaviour
{
    public int maxhlth = 1;
    public int remhealth;
    [SerializeField] int enDamage = 1;
    public int trueDamage;
    public float enemySpeed = 3.0f;
    public float trueSpeed;
    [SerializeField] float knockbackStrength = 20f;
    public float trueKnockBack;
    public Animator anim;
    [SerializeField] GameObject enemy;
    public Transform groundCheck;
    private bool isJumping;
    private bool facingLeft;
    private bool knock;
    private bool hit;
    private bool attCooldown;
    [SerializeField] Rigidbody2D rigid;
    public GameObject deathEffect;
    public SpriteRenderer sprite;
    public bool jumpAnim;
    private GameObject player;
    private float jumpTimer;
    // Start is called before the first frame update
    void Start()
    {
        facingLeft = true; 
        isJumping = true;
        knock = false;
        hit = false;
        attCooldown = false;
        jumpAnim = false;
        remhealth = maxhlth;
        trueDamage = enDamage;
        trueSpeed = enemySpeed;
        trueKnockBack = knockbackStrength;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlatform();
        UpdateAnim();
        if(isJumping == false){
            jumpTimer += Time.deltaTime;
            if(jumpTimer >= 3f){
                jumpTimer = 0;
                StartCoroutine(JumpCheck());
            }
        }
        if(hit == true){
            StartCoroutine(HitCheck());
        }
        if(knock == true){
            StartCoroutine(KnockCheck());
        }
        if(attCooldown == true){
            StartCoroutine(AttCheck());
        }
    }
    public void UpdateAnim(){
        if(enemy.GetComponent<Rigidbody2D>().velocity.y != 0){
            isJumping = true;
            jumpAnim = true;
            anim.SetBool("isJumping",true);
        }
        if(enemy.GetComponent<Rigidbody2D>().velocity.y == 0){
            isJumping = false;
            jumpAnim = false;
            anim.SetBool("isJumping",false);
        }
    }
    public void Movement(){
        if(facingLeft == true && isJumping == false && knock == false){
            Vector2 direction = new Vector2(-2f,4f).normalized;
            rigid.AddForce(direction*7f,ForceMode2D.Impulse);
        }
        if(facingLeft == false && isJumping == false && knock == false){
            Vector2 direction = new Vector2(2f,4f).normalized;
            rigid.AddForce(direction*7f,ForceMode2D.Impulse);
        }
    }
    public void DetectPlatform(){
        if(transform.position.x <= (player.transform.position.x) && facingLeft == true && !jumpAnim){
            Flip();
        }
        if(transform.position.x >= (player.transform.position.x) && facingLeft == false && !jumpAnim){
            Flip();
        }
    }
    public void Flip(){
        facingLeft = !facingLeft;
        transform.Rotate(0f,180f,0f);
    }
    public void DetectFallingOff(){
        if(this.transform.position.y <= -5){
            this.Die();
        }
    }
    public void TakeDamage(int damage){
        remhealth -= damage;
        if (remhealth <= 0)
        {
            Die();
        }
        KnockBack(player);
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
    public void KnockBack(GameObject other){
        Vector2 direction = new Vector2(transform.position.x - other.transform.position.x,transform.position.y+1f).normalized;
        rigid.AddForce(direction*6,ForceMode2D.Impulse);
        knock = true;
        isJumping = false;
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
    private IEnumerator JumpCheck(){
        yield return new WaitForSeconds(1f);
        Movement();
    }
    private IEnumerator HitCheck(){
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sprite.color = Color.white;
        hit = false;
    }
    private IEnumerator KnockCheck(){
        yield return new WaitForSeconds(2f);
        knock = false;
        isJumping = true;
    }
    private IEnumerator AttCheck(){
        yield return new WaitForSeconds(2f);
        attCooldown = false;
    }
    public void Die(){
        GameObject effect = Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(effect,1f);
        Destroy(this.gameObject);
        GameManager.instance.AddPoints(1);
    }

    public void StatIncrease(){
        maxhlth += 2;
        remhealth = maxhlth;
        Debug.Log("Health is now " + maxhlth);
        if(enemySpeed <= 6.0f){
            enemySpeed++;
            trueSpeed = enemySpeed;
            Debug.Log("Move Speed is " + enemySpeed);
        }
    }

    public void reset(){
        maxhlth = 1;
        enemySpeed = 3.0f;
    }
}
