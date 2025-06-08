using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This will be a boss enemy, Fox Boss
//It will move towards the player then attack
//Its main gimmick is that it will shoot projectiles at the Player
//There are two attacks, a horizontal fireball and a jumping fireball
public class FoxEnemy : MonoBehaviour
{
    public int health = 15;
    public int remhealth;
    public float moveSpd = 2.0f;
    private float trueSpeed;
    [SerializeField] float knockbackStrength = 20f;
    private bool hit = false;
    private bool knock = false;
    private bool moving = false;
    private bool isIdle = true;
    private bool jumping = false;
    private bool attacking = false;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject noti;
    [SerializeField] GameObject fireBall;
    [SerializeField] GameObject player;
    public SpriteRenderer sprite;
    public Animator anim;
    private bool isFacingLeft = false;
    private float attackTimer = 0f;
    private bool normalAtt = false;
    private bool jumpAtt = false;
    private bool attCooldown = false;
    private bool isJumping = false;
    private bool isAttacking = false;
    private float attacks = 0;

    private int points;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        remhealth = health;
        trueSpeed = moveSpd;
    }

    // Update is called once per frame
    void Update()
    {
        if(attacking == false){
            attackTimer += Time.deltaTime;
            moving = true;
            if(attackTimer >= 2f){
                attackTimer = 0;
                attacking = true;
                StartCoroutine(AttackTimer());
            }
        }
        if(normalAtt == true){
            normalAtt = false;
            isAttacking = true;
            StartCoroutine(Attack());
        }
        if(jumpAtt == true){
            jumpAtt = false;
            isJumping = true;
            StartCoroutine(JumpTimer());
        }
        if(hit == true){
            StartCoroutine(BlinkRed());
        }
        if(attCooldown == true){
            StartCoroutine(Cooldown());
        }
        Movement();
        DetectPlayer();
        UpdateAnimation();
        //Find player direction, face player
    }
    private void UpdateAnimation(){
        anim.SetBool("Moving", moving);
        if(isAttacking){
            anim.ResetTrigger("Jumping");
            anim.SetTrigger("Attack");
        }
        if(isJumping){
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Jumping");
        }
    }
    private void Movement(){
        if(attacking == false){
            this.transform.Translate(Vector2.right * trueSpeed * Time.deltaTime);
        }
        if(attacking == true){
            rigid.velocity = Vector2.zero;
        }
    }
    private void Jump(){
        attacks = 0f;
        GameObject fire = Instantiate(fireBall,transform.position,transform.rotation); 
        fire.GetComponent<EnemyProjectile>().SetDirection(isFacingLeft);
        isJumping = false;       
    }
    private void DetectPlayer(){
        if(this.isFacingLeft && (player.transform.position.x > this.transform.position.x)){
            Flip();
        }
        if(!isFacingLeft && (player.transform.position.x < transform.position.x)){
            Flip();
        }
    }
    private IEnumerator JumpTimer(){
        attacking = false;
        rigid.AddForce(Vector2.up * 8.0f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        Jump();
    }
    private IEnumerator AttackTimer(){
        moving = false;
        GameObject notification = Instantiate(noti,new Vector2(transform.position.x,transform.position.y + 0.5f),transform.rotation);
        notification.transform.parent = transform;
        Destroy(notification,1f);
        yield return new WaitForSeconds(1f);
        attacks = Random.Range(1.0f,2.0f);
        if(attacks <= 1.5f){
            normalAtt = true;
        }
        else{
            jumpAtt = true;
        }
    }
    private IEnumerator BlinkRed(){
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sprite.color = Color.white;
        hit = false;
    }
    private IEnumerator Cooldown(){
        yield return new WaitForSeconds(1f);
        attCooldown = false;
    }
    private void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private IEnumerator Attack(){
        attacking = false;
        attacks = 0f;
        yield return new WaitForSeconds(0.5f);
        GameObject fire = Instantiate(fireBall,new Vector2(transform.position.x,transform.position.y - 0.1f),transform.rotation);
        fire.GetComponent<EnemyProjectile>().SetDirection(isFacingLeft);
        isAttacking = false;
    }
    //Add on trigger stay
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
    public void KnockBack(GameObject other)
    {
        Vector2 direction = new Vector2(transform.position.x - other.transform.position.x,transform.position.y+1f).normalized;
        rigid.AddForce(direction*6,ForceMode2D.Impulse);
        knock = true;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && attCooldown == false)
        {
            other.transform.GetComponent<Player_Control>().CollidedEnemy(this.gameObject);
            other.transform.GetComponent<Player_Control>().TakeDamage(1);
            attCooldown = true;
        }
    }
    void Die()
    {
        points = health / 2;
        Debug.Log("Enemy died!");
        GameObject effect = Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(effect,1f);
        Destroy(this.gameObject);
        GameManager.instance.AddPoints(points);
    }

    public void StatIncrease(){
        health += 5;
        remhealth = health;
        Debug.Log("Health is now " + remhealth);
        if(moveSpd <= 4.0f){
        moveSpd++;
        Debug.Log("Move Speed is " + moveSpd);
        }
    }
    public void reset(){
        health = 10;
        moveSpd = 2.0f;
    }
}
