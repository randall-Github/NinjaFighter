using System.Collections;
using UnityEngine;

//This will be the basic enemy, the Possum Enemy
//It will simply move forward but once the player enters its range,
//It will charge 
public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxhlth = 1;
    public int remhealth;
    [SerializeField] int damage = 1;
    public int trueDamage;
    public float enemySpeed = 3.0f;
    public float trueSpeed;
    [SerializeField] float knockbackStrength = 5f;
    public float trueKnockBack;
    public bool isFacingLeft = true;
    public Animator anim;
    [SerializeField] GameObject enemy;
    public Transform groundCheck;
    private bool knock;
    private bool hit;
    private bool attCooldown;
    [SerializeField] Rigidbody2D rigid;
    public GameObject deathEffect;
    public SpriteRenderer sprite;
    public GameObject player;
    public bool charging = false;
    public bool isLunging = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        knock = false;
        hit = false;
        attCooldown = false;
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
        anim.SetBool("Charging", isLunging);
        if(knock == true){ 
            StartCoroutine(timer(2f));
        }
        if(hit == true){
            StartCoroutine(damageBlink());
        }
        if(attCooldown == true){
            StartCoroutine(Cooldown());
        }
        if(charging == true){
            charging = false;
            StartCoroutine(Charge());
        }
    }

    private void Movement()
    {
        if(!isLunging){
            this.transform.Translate(Vector2.left * trueSpeed * Time.deltaTime);
        }
        if(transform.position.x <= (player.transform.position.x + 3f) && isFacingLeft && !hit){
            if(!isLunging){
                charging = true;
            }
        }
        if(transform.position.x >= (player.transform.position.x - 3f) && !isFacingLeft && !hit){
            if(!isLunging){
                charging = true;
            }
        }
    }
    private IEnumerator Charge(){
        isLunging = true;
        yield return new WaitForSeconds(1f);
        if(isFacingLeft){
            rigid.AddForce(Vector2.left * 8f,ForceMode2D.Impulse);
        }
        if(!isFacingLeft){
            rigid.AddForce(Vector2.right * 8f,ForceMode2D.Impulse);
        }
        sprite.color = Color.gray;
        yield return new WaitForSeconds(2f);
        sprite.color = Color.white;
        isLunging = false;
    }
    private void DetectPlatform()
    {
        if (transform.position.x <= (player.transform.position.x) && isFacingLeft && !isLunging) 
        {
            Flip();
        }
        if (transform.position.x >= (player.transform.position.x) && !isFacingLeft && !isLunging)
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
        KnockBack(other.gameObject);
        hit = true;
    }

    public void TakeArrowDamage(int damage) { 
        remhealth -= damage;
        if (remhealth <= 0)
        {
            Die();
        }
        hit = true;
    }

    public void KnockBack(GameObject other)
    {
        Vector2 direction = new Vector2(transform.position.x - other.transform.position.x,transform.position.y+1f).normalized;
        rigid.AddForce(direction*8,ForceMode2D.Impulse);
        knock = true;
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
    private IEnumerator timer(float f){
        knock = false;
        yield return new WaitForSeconds(f);
        charging = false;
        isLunging = false;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0f;
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        GameObject effect = Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(effect,1f);
        Destroy(this.gameObject);
        GameManager.instance.AddPoints(1);
    }

    public void StatIncrease(){
        maxhlth += 2;
        remhealth = maxhlth;
        //Debug.Log("Health is now " + maxhlth);
        if(enemySpeed <= 6.0f){
            enemySpeed++;
            trueSpeed = enemySpeed;
            //Debug.Log("Move Speed is " + enemySpeed);
        }
    }

    public void reset(){
        maxhlth = 1;
        enemySpeed = 3.0f;
    }
}


