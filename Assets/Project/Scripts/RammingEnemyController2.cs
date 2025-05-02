using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class RammingEnemyController2 : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public Transform PlayerBullet;
    public float moveSpeed = 2f; // 通常移動速度
    public float chargeSpeed = 6f; // 体当たり速度
    public float detectionRadius = 5f; // 追尾開始距離
    public float attackRadius = 1.5f; // 体当たり開始距離

    private bool isChasing = false;
    private bool isCharging = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
            isCharging = false; // 追尾範囲外に出たら攻撃も解除
        }

        if (isChasing)
        {
            if (distanceToPlayer <= attackRadius)
            {
                // 体当たり開始！
                isCharging = true;
            }
            else
            {
                isCharging = false;
            }
        }

        if (isCharging)
        {
            ChargeAtPlayer();
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            rb.velocity = Vector2.zero; // 追尾しないときは停止
        }
    }

    void ChasePlayer()
    {
        // 通常の追尾
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void ChargeAtPlayer()
    {
        // 体当たり（加速）
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chargeSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("TestBullet")){
            GetComponent<SpriteRenderer>().DOFade(0f,1f).OnComplete(()=>Destroy(gameObject));
        }
        
    }

}
