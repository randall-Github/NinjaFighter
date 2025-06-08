using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This will be a boss enemy, Viking Boss
//The gimmick of this boss is to try and trap the player to deal lots of damage
//This boss stops the player from moving through it
//And it's attacks are meant to trap the player by having a similar start up in the attacks
//And to catch the Player into a combo
//There are three attacks, a regular slash attack, a charging attack and a jump attack
public class VikingEnemy : MonoBehaviour
{
    public int health = 30;
    public int remhealth;
    public float moveSpeed = 4.0f;
    private float trueSpeed;
    [SerializeField] float knockbackStrength = 18f;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject noti;
    public GameObject player;
    public SpriteRenderer sprite;
    public Animator anim;
    private bool isFacingLeft = false;
    private bool isAttacking = false;
    private bool isMoving = false;
    private bool isIdle = true;
    private bool isJumping = false;
    private bool isDownAttack = false;
    private float attackTimer = 0f;
    private bool hit = false;
    private bool attCooldown = false;
    private bool isCharging = false;
    private bool chargeAttack = false;
    private bool normalAttack = false;
    private bool slashing = false;
    private bool jumpAttack = false;
    private float attacks = 0;

    private int points;

    // Start is called before the first frame update
    void Start()
    {
        trueSpeed = moveSpeed;
        remhealth = health;
        player = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        UpdateAnimation();
        Movement();
        if(isAttacking == true){
            attackTimer += Time.deltaTime;
            if(attackTimer >= 1f){
                isAttacking = false;
                attackTimer = 0;
                StartCoroutine(Attacking());
            }
        }
        if(attCooldown == true){
            StartCoroutine(Cooldown());
        }
        if(chargeAttack == true){
            chargeAttack = false;
            StartCoroutine(Charge());
        }
        if(normalAttack == true){
            normalAttack = false;
            StartCoroutine(Attack());
        }
        if(jumpAttack == true){
            jumpAttack = false;
            StartCoroutine(Jumping());
        }
        if(hit == true){
            StartCoroutine(BlinkRed());
        }
    }
    private void Movement(){
        if(isFacingLeft && (player.transform.position.x < transform.position.x - 1.1f)){
            this.transform.Translate(Vector2.right * trueSpeed * Time.deltaTime);
            isMoving = true;
        }
        if(isFacingLeft && (player.transform.position.x > transform.position.x - 3f)){
            if(isAttacking == false){
                isAttacking = true;
            }
        }
        if(!isFacingLeft && (player.transform.position.x > transform.position.x + 1.1f)){
            this.transform.Translate(Vector2.right * trueSpeed * Time.deltaTime);
            isMoving = true;
        }
        if(!isFacingLeft && (player.transform.position.x < transform.position.x + 3f)){
            if(isAttacking == false){
                isAttacking = true;
            }
        }
    }
    private void DetectPlayer(){
        if(this.isFacingLeft && (player.transform.position.x > this.transform.position.x) && !isCharging){
            Flip();
        }
        if(!isFacingLeft && (player.transform.position.x < transform.position.x) && !isCharging){
            Flip();
        }
    }
    private void Flip(){
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private void UpdateAnimation(){
        anim.SetBool("Moving", isMoving);
        if(slashing){
            anim.ResetTrigger("Thrust");
            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("DownAttack");
            anim.SetTrigger("Slash");
        }
        if(isCharging){
            anim.ResetTrigger("Slash");
            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("DownAttack");
            anim.SetTrigger("Thrust");
        }
        if(isJumping){
            anim.ResetTrigger("Slash");
            anim.ResetTrigger("Thrust");
            anim.ResetTrigger("DownAttack");
            anim.SetTrigger("Jumping");
        }
        if(isDownAttack){
            anim.ResetTrigger("Slash");
            anim.ResetTrigger("Jumping");
            anim.ResetTrigger("Thrust");
            anim.SetTrigger("DownAttack");
        }
    }
    private IEnumerator Charge(){
        attacks = 0;
        isJumping = false;
        if(!isFacingLeft){
            rigid.AddForce(Vector2.right * 800f, ForceMode2D.Impulse);
        }
        if(isFacingLeft){
            rigid.AddForce(Vector2.left * 800f, ForceMode2D.Impulse);
        }
        isCharging = true;
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector2.zero;
        isCharging = false;
    }
    private IEnumerator Attack(){
        isJumping = false;
        slashing = true;
        attacks = 0;
        yield return new WaitForSeconds(0.5f);
        slashing = false;
    }
    private IEnumerator DownAttack(){
        isJumping = false;
        isDownAttack = true;
        rigid.AddForce(Vector2.down * 1500f, ForceMode2D.Impulse);
        attacks = 0;
        yield return new WaitForSeconds(0.5f);
        isDownAttack = false;
    }
    private IEnumerator Attacking(){
        isMoving = false;
        isJumping = true;
        GameObject notification = Instantiate(noti,new Vector2(transform.position.x,transform.position.y + 2f),transform.rotation);
        notification.transform.parent = transform;
        Destroy(notification,0.5f);
        yield return new WaitForSeconds(0.5f);
        attacks = Random.Range(1.0f,4.0f);
        if(attacks < 2.0f){
            normalAttack = true;
        }
        if(attacks > 3.0f){
            chargeAttack = true;
        }
        if(2.0f <= attacks && attacks <= 3.0f){
            jumpAttack = true;
        }
    }
    private IEnumerator Jumping(){
        rigid.AddForce(Vector2.up * 1000f,ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.35f);
        StartCoroutine(DownAttack());
    }
    private IEnumerator Cooldown(){
        yield return new WaitForSeconds(0.5f);
        attCooldown = false;
    }
    private IEnumerator BlinkRed(){
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        hit = false;
    }
    private void KnockBack(GameObject other){
        Vector2 direction = new Vector2(transform.position.x - other.transform.position.x,transform.position.y+1f).normalized;
        rigid.AddForce(direction*400,ForceMode2D.Impulse);
    }
    public void TakeDamage(int damage)
    {
        remhealth -= damage;
        if (remhealth <= 0)
        {
            Die();
        }
        KnockBack(player.gameObject);
        hit = true;
    }
    public void TakeArrowDamage(int damage){
        remhealth -= damage;
        if (remhealth <= 0)
        {
            Die();
        }
        hit = true;
    }
    public float getKnockback(){
        return this.knockbackStrength;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && attCooldown == false)
        {
            if(slashing == true || isDownAttack == true || isCharging == true){
                other.transform.GetComponent<Player_Control>().CollidedEnemy(this.gameObject);
                other.transform.GetComponent<Player_Control>().TakeDamage(1);
                attCooldown = true;
                slashing = false;
                isDownAttack = false;
                isCharging = false;
            }
        }
    }
    private void Die(){
        points = health / 2;
        Debug.Log("Enemy died!");
        GameObject effect = Instantiate(deathEffect,new Vector2(this.transform.position.x,transform.position.y +1f),transform.rotation);
        Destroy(effect,1f);
        Destroy(this.gameObject);
        GameManager.instance.AddPoints(points);
    }

    public void StatIncrease(){
        health += 5;
        Debug.Log("Health is now " + remhealth);
        if(moveSpeed <= 8.0f){
        moveSpeed++;
        Debug.Log("Move Speed is " + moveSpeed);
        }
    }
    public void reset(){
        health = 20;
        moveSpeed = 4f;
    }
}