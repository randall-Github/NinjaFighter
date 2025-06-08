using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15.0f;
    public int damage = 1;
    private Rigidbody2D rigid;
    public GameObject fox;
    public GameObject platform;
    public GameObject player;
    public GameObject deathEffect;
    public float targetX;
    public float targetY;
    private float startingY;
    private bool isFacingLeft = false;
    private bool isJump = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody2D>();
        targetX = player.transform.position.x;
        targetY = player.transform.position.y;
        startingY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
        DetectPlatform();
        if(rigid.velocity.x == 0 && !isFacingLeft){
            targetY -= Time.deltaTime * moveSpeed;
            targetX += Time.deltaTime * moveSpeed;
        }
        if(rigid.velocity.x == 0 && isFacingLeft){
            targetY -= Time.deltaTime * moveSpeed;
            targetX -= Time.deltaTime * moveSpeed;
        }
    }
    private void DetectPlatform(){
        if(transform.position.y <= (platform.transform.position.y + (platform.transform.localScale.y/2))+1.8f){
            DeleteFire();
        }
        if(transform.position.x <= (player.transform.position.x - 30f)){
            DeleteFire();
        }
        if(transform.position.x >= (player.transform.position.x + 30f)){
            DeleteFire();
        }
    }
    private void MoveProjectile()
    {
        if(isJump == false){
            if(startingY >= 1f){
                isJump = true;
                this.transform.Rotate(new Vector3(transform.rotation.x,transform.rotation.y,-30f));
            }
        }
        if(startingY >= 1f){
                isJump = true;
                rigid.velocity = Vector2.zero;
                this.transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX,targetY),moveSpeed * Time.deltaTime);
            }
        else{
            this.transform.Translate(Vector2.right * moveSpeed *  Time.deltaTime);
        }

    }
    public void SetDirection(bool x){
        isFacingLeft = x;
    }
    public void Flip(){
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0.0f,180.0f,0.0f);
    }
    private void DeleteFire(){
        GameObject effect = Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(effect, 1f);
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player") {
            collision.transform.GetComponent<Player_Control>().CollidedEnemy(fox.gameObject);
            DeleteFire();
            collision.transform.GetComponent<Player_Control>().TakeDamage(1);
        }
    }
}
