using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_knockBack : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    public int damage = 2;
    private Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }
    private void MoveProjectile()
    {
        rigid.AddForce(transform.right * moveSpeed);
    }

    public void Add_Damage()
    {
        damage += 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Player")
        {
            collision.SendMessage("TakeDamage", damage);
        }
    }
}
