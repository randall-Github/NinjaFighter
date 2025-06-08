using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This will be a boss enemy, Sword Boss
//This boss' gimmick is that it will duel the Player
//It will dodge attacks and dash towards or behind the player for a mix up
//There are two attacks, a regular attack that tries to hit the player twice
//And a heavy attack that has longer range
public class SwordBoss : MonoBehaviour
{
    public GameObject player;
    public GameObject deathEffect;
    public GameObject noti;
    public SpriteRenderer sprite;
    public Animator anim;
    public Rigidbody2D rigid;
    public float moveSpeed = 10.0f;
    private float trueSpeed;
    public int health = 20;
    private int remhealth;
    [SerializeField] float knockbackStrength = 8f;
    private bool isFacingLeft = false;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool attacking = false;
    private bool doubleAttack = false;
    private bool isDouble = false;
    private bool doubleDamage = false;
    private bool dodgeTime = true;
    private bool isDodging = false;
    private float dodgeReset = 0f;
    private bool hit = false;
    private bool dashing = false;
    private bool isDashing = false;
    private bool heavyAttack = false;
    private bool isHeavyAttacking = false;
    private bool heavyDamage = false;
    private float attacks = 0f;
    private bool attCooldown = false;

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
        Movement();
        UpdateAnimation();
        DetectPlayer();
        if(dodgeTime == false){
            dodgeReset += Time.deltaTime;
            if(dodgeReset >= 2.0f){
                dodgeReset = 0;
                dodgeTime = true;
            }
        }
        if(attacking == true && !isDodging){
            attacking = false;
            StartCoroutine(Attack());
        }
        if(doubleAttack == true && !isDodging){
            doubleAttack = false;
            StartCoroutine(DoubleAttack());
        }
        if(dashing == true && !isDodging){
            dashing = false;
            StartCoroutine(Charge());
        }
        if(heavyAttack == true && !isDodging){
            heavyAttack = false;
            StartCoroutine(Heavy());
        }
        if(attCooldown == true){
            StartCoroutine(Cooldown());
        }
        if(hit == true){
            StartCoroutine(BlinkRed());
        }
    }
    private void Movement(){
        if(isFacingLeft && (player.transform.position.x < transform.position.x + 1f) && !isDodging &&!isAttacking){
            this.transform.Translate(Vector2.right * trueSpeed * Time.deltaTime);
            isMoving = true;
        }
        if(isFacingLeft && (player.transform.position.x > transform.position.x - 0.8f)){
            if(isAttacking == false){
                isMoving = false;
                isAttacking = true;
                attacking = true;
            }
        }
        if(!isFacingLeft && (player.transform.position.x > transform.position.x - 1f) && !isDodging && !isAttacking){
            this.transform.Translate(Vector2.right * trueSpeed * Time.deltaTime);
            isMoving = true;
        }
        if(!isFacingLeft && (player.transform.position.x < transform.position.x + 0.8f)){
            if(isAttacking == false){
                isMoving = false;
                isAttacking = true;
                attacking = true;
            }
        }
    }
    private void UpdateAnimation(){
        anim.SetBool("Moving", isMoving);
        anim.SetBool("Hit", hit);
        if(isDodging){
            anim.ResetTrigger("Double");
            anim.ResetTrigger("Charging");
            anim.ResetTrigger("Heavy");
            anim.SetTrigger("Dodge");
        }
        if(isDouble){
            anim.ResetTrigger("Dodge");
            anim.ResetTrigger("Charging");
            anim.ResetTrigger("Heavy");
            anim.SetTrigger("Double");
        }
        if(isDashing){
            anim.ResetTrigger("Dodge");
            anim.ResetTrigger("Double");
            anim.ResetTrigger("Heavy");
            anim.SetTrigger("Charging");
        }
        if(isHeavyAttacking){
            anim.ResetTrigger("Dodge");
            anim.ResetTrigger("Double");
            anim.ResetTrigger("Charging");
            anim.SetTrigger("Heavy");
        }
    }
    private IEnumerator DoubleAttack(){
        isDouble = true;
        doubleDamage = true;
        yield return new WaitForSeconds(0.2f);
        isDouble = false;
        StartCoroutine(ResetAttack());
    }
    private IEnumerator Charge(){
        if(!isFacingLeft){
            rigid.AddForce(Vector2.right * 6f, ForceMode2D.Impulse);
        }
        if(isFacingLeft){
            rigid.AddForce(Vector2.left * 6f, ForceMode2D.Impulse);
        }
        isDashing = true;
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
        StartCoroutine(ResetAttack());
    }
    private IEnumerator Heavy(){
        heavyDamage = true;
        isHeavyAttacking = true;
        yield return new WaitForSeconds(0.2f);
        isHeavyAttacking = false;
        StartCoroutine(ResetAttack());
    }
    private IEnumerator ResetAttack(){
        yield return new WaitForSeconds(0.7f);
        heavyDamage = false;
        doubleDamage = false;
        attacks = 0f;
        isAttacking = false;
    }
    private IEnumerator Attack(){
        GameObject notification = Instantiate(noti,new Vector2(transform.position.x,transform.position.y + 0.5f),transform.rotation);
        notification.transform.parent = transform;
        Destroy(notification,0.5f);
        yield return new WaitForSeconds(0.1f);
        attacks = Random.Range(1.0f,4.0f);
        if(attacks < 2.0f){
            doubleAttack = true;
        }
        if(attacks > 3.0f){
            dashing = true;
        }
        if(2.0f <= attacks && attacks <= 3.0f){
            heavyAttack = true;
        }
    }
    private void DetectPlayer(){
        if(this.isFacingLeft && ((player.transform.position.x) > this.transform.position.x) && !isDashing && !isDodging){
            Flip();
        }
        if(!isFacingLeft && ((player.transform.position.x) < transform.position.x) && !isDashing && !isDodging){
            Flip();
        }
    }
    private void Flip(){
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private void KnockBack(GameObject other){
        Vector2 direction = new Vector2(transform.position.x - other.transform.position.x,transform.position.y+1f).normalized;
        rigid.AddForce(direction*4f,ForceMode2D.Impulse);
    }
    public void TakeDamage(int damage)
    {
        if(!dodgeTime){
            remhealth -= damage;
            if (remhealth <= 0)
            {
                Die();
            }
            KnockBack(player.gameObject);
            hit = true;
            heavyDamage = false;
            doubleDamage = false;
        }
        else{
            StartCoroutine(Dodging());
        }
    }
    public void TakeArrowDamage(int damage){
        if(!dodgeTime){
            remhealth -= damage;
            if (remhealth <= 0)
            {
                Die();
            }
            hit = true;
        }
        else{
            StartCoroutine(Dodging());
        }
    }
    private IEnumerator Dodging(){
        if(!isFacingLeft){
            rigid.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
        }
        if(isFacingLeft){
            rigid.AddForce(Vector2.left * 5f, ForceMode2D.Impulse);
        }
        isMoving = false;
        isDodging = true;
        doubleDamage = false;
        heavyDamage = false;
        yield return new WaitForSeconds(0.5f);
        isDodging = false;
        dodgeTime = false;
    }
    public float getKnockback(){
        return this.knockbackStrength;
    }
    private IEnumerator Cooldown(){
        yield return new WaitForSeconds(1.0f);
        attCooldown = false;
    }
    private IEnumerator BlinkRed(){
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        hit = false;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && attCooldown == false)
        {
            other.transform.GetComponent<Player_Control>().CollidedEnemy(this.gameObject);
            if(heavyDamage == true){
                other.transform.GetComponent<Player_Control>().TakeDamage(3);
                attCooldown = true;
                heavyDamage = false;
            }
            if(doubleDamage == true){
                other.transform.GetComponent<Player_Control>().TakeDamage(2);
                attCooldown = true;
                doubleDamage = false;
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
        //Debug.Log("Health is now " + remhealth);
        if(moveSpeed <= 16.0f){
        moveSpeed++;
        //Debug.Log("Move Speed is " + moveSpeed);
        }
    }
    public void reset(){
        health = 30;
        moveSpeed = 10.0f;
    }
}
