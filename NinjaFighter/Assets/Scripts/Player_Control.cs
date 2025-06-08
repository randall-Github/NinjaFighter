using System.Collections;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    private float moveDirection;
    private Rigidbody2D rigid;

    //for animations
    private Animator anim;
    [SerializeField] 
    private GameObject 
        arrow,
        spear;

    [SerializeField]
    private bool 
        canAttack,
        canSkill1,
        canSkill2;

    //These are booleans for attacks and skills
    private bool inputPressed;
    private bool inputSkill1;
    private bool inputSkill2; 
    private bool isAttacking;
    private bool isArrowing;
    private bool isSpearing;
    private bool isDashing;

    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canJump;
    private bool isWalking;
    private bool isJumping;
    private bool knockback;

    public int dmg = 2;

    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float atkrange = 0.5f;
    public float groundCheckRadius;
    public float acceleration;
    public float decceleration;
    public float velpower;
    public float frictionAmount;
    public float jumpCoyoteTime;
    public float jumpBufferTime;
    public float strtup;
    public float dashTime;
    public float dashSpeed;
    public float distanceBeweenImg;
    public float dashCooldown;

    private float lastAttackTime = Mathf.NegativeInfinity;
    private float lastSkill1Time = Mathf.NegativeInfinity;
    private float lastSkill2Time = Mathf.NegativeInfinity;


    private float LastOnGroundTime;
    private float lastJumpTime;
    private float jumpTimeCounter;
    private float knockbackStartTime;
    private float horiMove;
    private float dashTimeLeft;
    private float lastImageX;

    private string skill1;
    private string skill2;

    [SerializeField]
    private float
        ArrowRate,
        SpearRate,
        knockbackDur,
        atkTimer;


    public Transform atk;
    public Transform shoot_point;
    public LayerMask elayers;

    [SerializeField]
    private Vector2 knockBackSpe;


    //for the transform you put at the feet during groundcheck
    public Transform groundCheck;

    public LayerMask whatIsGround;
    [SerializeField] public int health = 5;
    public int trueHealth;
    public GameObject enemyCollided;
    public SpriteRenderer sprite;
    private bool isHit;
    public GameObject deathEffect;


    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trueHealth = health;
        canSkill1 = false;
        canSkill2 = false;
        anim.SetBool("canAttack", canAttack);
        anim.SetBool("canSkill1", canSkill1);
        anim.SetBool("canSkill2", canSkill2);
    }

    // Update is called once per frame
    void Update()
    {
        horiMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        LastOnGroundTime -= Time.deltaTime;
        lastJumpTime = Time.deltaTime;
        CheckInput();
        CheckAtkInput();
        CheckMoveDir();
        UpdateAnimations();
        DetectFallingOff();
        if (knockback == true) {
            StartCoroutine(CheckKnockback());
        }
        if (isHit == true) {
            StartCoroutine(DamageBlink());
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        Friction();
        CheckSurroundings();
        CheckKnockback();
        CheckifJump();
        CheckifCanAtk();
        Dash();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckifJump()
    {
        if (isGrounded)
        {
            isJumping = false;
            LastOnGroundTime = jumpCoyoteTime;
        }
        if (LastOnGroundTime > 0 && lastJumpTime > 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

    }

    private void CheckMoveDir()
    {
        if (isFacingRight && moveDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && moveDirection > 0)
        {
            Flip();
        }
        if (Mathf.Abs(horiMove) >= 0.01)
        {
            isWalking = true;
        }
        else
        {

            isWalking = false;
        }
    }

    //This function is to update animations
    private void UpdateAnimations()
    {
        anim.SetFloat("IsWalking", Mathf.Abs(horiMove));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("YVel", rigid.velocity.y);
    }

    private void CheckInput()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            Jump();
        }
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {

            }
            else
            {
                isJumping = false;
            }
        }

    }
    void CheckAtkInput() {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (canAttack)
            {
                inputPressed = true;
                lastAttackTime = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (canSkill1)
            {
                inputSkill1 = true;

                lastSkill1Time = Time.time;

            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (canSkill2)
            {
                inputSkill2 = true;

                lastSkill2Time = Time.time;

            }
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            LastOnGroundTime = 0;
            lastJumpTime = 0;
        }
    }

    //this is to apply knockback to player
    public void KnockBack(GameObject other) {
        Vector2 dir = new Vector2(transform.position.x - other.transform.position.x, transform.position.y+1f).normalized;
        if (other.transform.tag == "Enemy") {
            rigid.AddForce(dir * (other.GetComponent<Enemy>().getKnockback()), ForceMode2D.Impulse);
        }
        if (other.transform.tag == "FlyingEnemy") {
            rigid.AddForce(dir * (other.GetComponent<FlyingEnemy>().getKnockback()), ForceMode2D.Impulse);
        }
        if (other.transform.tag == "FrogEnemy") {
            rigid.AddForce(dir * (other.GetComponent<FrogEnemy>().getKnockback()), ForceMode2D.Impulse);
        }
        if (other.transform.tag == "PossumBoss") {
            rigid.AddForce(dir * (other.GetComponent<BossPossum>().getKnockback()), ForceMode2D.Impulse);
        }
        if(other.transform.tag == "FoxEnemy"){
            rigid.AddForce(dir *(other.GetComponent<FoxEnemy>().getKnockback()), ForceMode2D.Impulse);
        }
        if(other.transform.tag == "VikingBoss"){
            rigid.AddForce(dir *(other.GetComponent<VikingEnemy>().getKnockback()), ForceMode2D.Impulse);
        }
        if(other.transform.tag == "SwordBoss"){
            rigid.AddForce(dir *(other.GetComponent<SwordBoss>().getKnockback()), ForceMode2D.Impulse);
        }
        knockback = true;
    }
    //this is to actually check if you can be knocked back
    private IEnumerator CheckKnockback() {
        yield return new WaitForSeconds(0.25f);
        knockback = false;
        rigid.velocity = new Vector2(0.0f, rigid.velocity.y);
    }
    private IEnumerator DamageBlink() {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sprite.color = Color.white;
        isHit = false;
    }

    //This is so the player actually moves
    private void ApplyMovement()
    {

        //calculate the direction we want to move
        float targetSpeed = moveDirection * moveSpeed;
        //calculates the difference between target speed and current speed
        float speedDif = targetSpeed - rigid.velocity.x;
        //calculates acceleration rate based on these two public floats
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velpower) * Mathf.Sign(speedDif);
        if (!knockback && !isDashing) {
            rigid.AddForce(movement * Vector2.right);
        }
    }

    //for flipping animations
    private void Flip()
    {
        if (!knockback && !isDashing) {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    //This is for the friction
    public void Friction()
    {
        //If character is on the ground and walking dont slow down when turning.
        if (LastOnGroundTime > 0 && isWalking)
        {
            float amount = Mathf.Min(Mathf.Abs(rigid.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Min(Mathf.Abs(rigid.velocity.x), Mathf.Abs(frictionAmount));
            // applies -amount to the direction
            rigid.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    //this checks if you can attack
    private void CheckifCanAtk() {
        if (Time.time >= lastAttackTime + atkTimer)
        {
            inputPressed = false;
        }
        else if (inputPressed && !isArrowing && !isSpearing)
        {
            if (!isAttacking)
            {
                inputPressed = false;
                isAttacking = true;
                anim.SetBool("Attack", true);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if (skill1 == "Arrow")
        {
            if (Time.time >= lastSkill1Time + ArrowRate)
            {
                inputSkill1 = false;
            }
            else if (inputSkill1 && !isAttacking && !isSpearing)
            {
                if (!isArrowing)
                {
                    inputSkill1 = false;
                    isArrowing = true;
                    anim.SetBool("Arrow", true);
                    anim.SetBool("isArrowing", isArrowing);
                }
            }
        }
        else if (skill1 == "Dash")
        {
            if (Time.time >= lastSkill1Time + dashCooldown)
            {
                inputSkill1 = false;
            }
            else if (inputSkill1 && !isAttacking && !isSpearing)
            {
                inputSkill1 = false;
                isDashing = true;
                dashTimeLeft = dashTime;
                AfterImage_pool.instance.GetFromPool();
                lastImageX = transform.position.x;
            }
        }
        else if (skill1 == "Spear")
        {
            if (Time.time >= lastSkill1Time + SpearRate)
            {
                inputSkill1 = false;
            }
            else if (inputSkill1 && !isAttacking && !isArrowing)
            {

                if (!isSpearing)
                {
                    inputSkill1 = false;
                    isSpearing = true;
                    anim.SetBool("Spear", true);
                    anim.SetBool("isSpearing", isSpearing);
                }
            }
        }
        if (skill2 == "Arrow")
        {
            if (Time.time >= lastSkill2Time + ArrowRate)
            {
                inputSkill1 = false;
            }
            else if (inputSkill2 && !isAttacking && !isSpearing)
            {
                if (!isArrowing)
                {
                    inputSkill2 = false;
                    isArrowing = true;
                    anim.SetBool("Arrow", true);
                    anim.SetBool("isArrowing", isArrowing);
                }
            }
        }
        else if (skill2 == "Dash")
        {
            if (Time.time >= lastSkill2Time + dashCooldown)
            {
                inputSkill2 = false;
            }
            else if (inputSkill2 && !isAttacking && !isSpearing)
            {
                inputSkill2 = false;
                isDashing = true;
                dashTimeLeft = dashTime;
                AfterImage_pool.instance.GetFromPool();
                lastImageX = transform.position.x;
            }
        }
        else if (skill2 == "Spear")
        {
            if (Time.time >= lastSkill2Time + SpearRate)
            {
                inputSkill2 = false;
            }
            else if (inputSkill2 && !isAttacking && !isArrowing)
            {

                if (!isSpearing)
                {
                    inputSkill2 = false;
                    isSpearing = true;
                    anim.SetBool("Spear", true);
                    anim.SetBool("isSpearing", isSpearing);
                }
            }
        }

    }


    void Attack()
    {
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(atk.position, atkrange, elayers);
        foreach (Collider2D enemy in hitenemies)
        {
            enemy.SendMessage("TakeDamage",dmg);
        }

    }

    void finish_attack() {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("Attack", false);
    }
    void Arrow()
    {
        Instantiate(arrow, shoot_point.position, shoot_point.rotation);
    }

    void finish_arrow()
    {
        isArrowing = false;
        anim.SetBool("isArrowing", isArrowing);
        anim.SetBool("Arrow", false);
    }

    void Spear()
    {
        Instantiate(spear, shoot_point.position, shoot_point.rotation);
    }

    void finish_spear() {
        isSpearing = false;
        anim.SetBool("isSpearing", isSpearing);
        anim.SetBool("Spear", false);
    }

    void Dash() {
        if (isDashing) {
            if (dashTimeLeft > 0) {
                if (isFacingRight)
                {
                    rigid.velocity = new Vector2(dashSpeed, rigid.velocity.y);
                    dashTimeLeft -= Time.deltaTime;
                }
                else {
                    rigid.velocity = new Vector2(-dashSpeed, rigid.velocity.y);
                    dashTimeLeft -= Time.deltaTime;
                }

                if (Mathf.Abs(transform.position.x - lastImageX) > distanceBeweenImg) {
                    AfterImage_pool.instance.GetFromPool();
                    lastImageX = transform.position.x;
                }
            }
            if (dashTimeLeft<=0) {
                isDashing = false;
            }

        }
    }

    public void DetectFallingOff() {
        if (transform.position.y <= -12f) {
            Die();
        }
    }
    public void CollidedEnemy(GameObject en) {
        enemyCollided = en.gameObject;
    }
    public void TakeDamage(int damage)
    {
        if (!isDashing)
        {
            trueHealth -= damage;
            if (trueHealth <= 0)
            {
                Die();
            }
            KnockBack(enemyCollided);
            isHit = true;
        }
    }
    //Player Upgrades
    public void ReplenishHealth() {
        trueHealth = health;
    }
    public void IncreaseJump() {
        jumpForce += 1;
    }
    public void IncreaseSpeed() {
        moveSpeed += 1;
    }

    public void Add_damage() {
        dmg += 1;
    }

    public void Add_max_hlth()
    {
        health += 1;
        if (health-1 == trueHealth)
        {
            trueHealth+=1;
        }
    }

    public void Add_arrowRate() {
        ArrowRate += 0.01f;
    }

    public void Add_arrowDam()
    {
        arrow.GetComponent<Projectile>().Add_Damage();
    }

    public void Add_spearRate() {
        SpearRate += 0.001f;
    }

    public void Add_spearDam()
    {
        spear.GetComponent<Projectile_knockBack>().Add_Damage();
    }

    public void Add_dash_cooldown() {
        dashCooldown += 0.001f;
    }

    public void Add_DashTime()
    {
        dashTime += 0.1f;
    }

    //Unlockable skills

    private void Unlock_arrow()
    {
        Debug.Log("Unlocked Bow and Arrow");
        if (!canSkill1)
        {
            skill1 = "Arrow";
            canSkill1 = true;
        }
        else {
            skill2 = "Arrow";
            canSkill2 = true;
        }

    }

    private void Unlock_spear()
    {
        Debug.Log("Unlocked Spear");
        if (!canSkill1)
        {
            skill1 = "Spear";
            canSkill1 = true;
        }
        else
        {
            skill2 = "Spear";
            canSkill2 = true;
        }
    }

    private void Unlock_dash()
    {
        Debug.Log("Unlocked dash");
        if (!canSkill1)
        {
            skill1 = "Dash";
            canSkill1 = true;
        }
        else
        {
            skill2 = "Dash";
            canSkill2 = true;
        }
    }

    public void Die(){
        GameManager.instance.Death();
        GameObject effect = Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(effect,1f);
        Destroy(this.gameObject);
    }

    //for Ground check to show
    private void OnDrawGizmos()
    {
        if (atk == null) return;
        Gizmos.DrawWireSphere(atk.position, atkrange);
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}

