using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    [SerializeField]
    private float activeTime = 0.1f;
    private float activated;
    private float alpha;
    [SerializeField]
    private float alphaset = 0.8f;
    private float alphaMulti = 0.85f;


    private Transform player;
    private SpriteRenderer dash;
    private SpriteRenderer playerdash;

    private Color color;

    private void OnEnable()
    {
        dash = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerdash = player.GetComponent<SpriteRenderer>();

        alpha = alphaset;
        dash.sprite = playerdash.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;

        activated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMulti;
        color = new Color(1f,1f, alpha);
        playerdash.color = color;

        if (Time.time >= (activated + activeTime)) {
            AfterImage_pool.instance.AddtoPool(gameObject);
        }
    }
}
