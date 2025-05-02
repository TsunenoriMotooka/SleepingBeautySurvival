using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class RammingEnemyController : MonoBehaviour
{
    public Transform Player;
    public Transform PlayerBullet;
    public float moveSpeed = 2f;
    public float chargSpeed = 6f;
    public float detectionRadius = 5f;
    public float attackRadius = 1.5f;

    private bool isChasing = false;
    private bool isCharging = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float ditanceToPlayer = Vector2.Distance(transform.position, Player.position);
        
        if(ditanceToPlayer <= detectionRadius){
            isChasing = true;
        }else{
            isChasing = false;
            isChasing = false;
        }
        if(isChasing){
            if(ditanceToPlayer <= attackRadius){
                isCharging = true;
            }
        }else{
            isCharging = false;
        }

        if(isCharging){
            ChargeAtPlayer(); 
        }else if(isChasing){
            ChasePlayer();
        }else{
            rb.velocity = Vector2.zero;
        }
        
    }

    void ChasePlayer(){
        Vector2 direction = (Player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void ChargeAtPlayer(){
        Vector2 direction = (Player.position - transform.position).normalized;
        rb.velocity = direction * chargSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerBullet")){
            GetComponent<SpriteRenderer>().DOFade(0f,1f).OnComplete(()=>Destroy(gameObject));
        }
        
    }
/*
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerBullet")){
            Destroy(gameObject);
        }
    }
*/
}
