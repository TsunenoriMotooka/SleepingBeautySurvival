using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EnemyBase : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRadius = 5f;
    public bool canMove = true;
    public float fadeTime = 1f;
    public float blinkSpeed = 0.1f;
    public int blinkCount = 2;

    public Transform player;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRadius)
        {
            if (canMove) MoveTowardsPlayer();
            Attack();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    protected virtual void OnHit(){
        StartCoroutine(BlinkDestroy());
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("TestPrincess") || other.CompareTag("TestBullet")){
            OnHit();
        }
        
    }

    IEnumerator BlinkDestroy(){
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for(int i = 0; i < blinkCount; i++){
            sr.DOFade(0f, blinkSpeed);
            yield return new WaitForSeconds(blinkSpeed);
            sr.DOFade(1f, blinkSpeed);
            yield return new WaitForSeconds(blinkSpeed);
            
        }

        Destroy(gameObject);
        // sr.DOFade(0f, fadeTime).OnComplete(()=> Destroy(gameObject));

    }

    protected abstract void Attack();
}
