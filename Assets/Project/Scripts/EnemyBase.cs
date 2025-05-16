using System.Collections;
using UnityEngine;
using System;

public abstract class EnemyBase : MonoBehaviour
{
    public bool canMove = true;
    public float stopDuration = 2f;
    public GameObject EnemyDieEffect;
    public AudioGenerator audioGenerator;
    protected bool hasHitPlayerAll = false;
    protected Animator animator;

    [SerializeField]public Transform player;
    protected Rigidbody2D rb;

    public Camera mainCamera;
    public bool IsVisible = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if(player == null){
            Debug.LogError("Player is not assigned! Please set it in the Inspector.");
        }
    }

    protected virtual void Update()
    {
        if (player == null) return;

        //カメラの視野に入った時
        Vector2 fromPoint = mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 toPoint = mainCamera.ViewportToWorldPoint(Vector2.one);
        if (transform.position.x >= fromPoint.x && transform.position.x <= toPoint.x &&
            transform.position.y >= fromPoint.y && transform.position.y <= toPoint.y) {
            IsVisible = true;
        } else {
            IsVisible = false;
        }
    }

    public void FlipSprite()
    {
        if (player == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        //プレイヤーが左側なら `flipX = true`、右側なら `false`
        sr.flipX = player.transform.position.x < transform.position.x;
    }

    protected virtual void OnHit(){
        if (EnemyDieEffect != null)
        {
            //消滅時にエフェクト生成
            GameObject effe = Instantiate(EnemyDieEffect, transform.position, Quaternion.identity); 
            Destroy(effe,0.5f);
            audioGenerator.PlaySE(SE.HitLeaf, transform);
            audioGenerator.PlaySE(SE.DamageEnemy, transform);
        }

        Destroy(gameObject);
    }

    public void PlayAttackSE() {
        if (gameObject.name.Contains("ChargeEnemy")) {
            audioGenerator.PlaySE(SE.HitChargeEnemy, transform);
        } else if (gameObject.name.Contains("RunningEnemy")) {
            audioGenerator.PlaySE(SE.HitRunningEnemy, transform);
        } else {
            audioGenerator.PlaySE(SE.HitTurretEnemy, transform);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Leaf"))
        {
            OnHit();
        }    


        if (collision.gameObject.CompareTag("Princess"))
        {
            hasHitPlayerAll = true;
            canMove = false;
            rb.velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(RestartAfterDelay(stopDuration));

            PlayAttackSE();
        }    
    }

    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        yield return new WaitForSeconds(0.1f);
        hasHitPlayerAll = false;
        canMove = true;

        GetComponent<Collider2D>().enabled = true;
    }
}
