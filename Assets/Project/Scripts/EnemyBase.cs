using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EnemyBase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRadius = 4f; //プレイヤー検知範囲
    public bool canMove = true;
    public float blinkSpeed = 0.1f;
    public int blinkCount = 2;
    public GameObject EnemyDieEffect;

    [SerializeField]public Transform player;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(player == null){
            Debug.LogError("Player is not assigned! Please set it in the Inspector.");
        }
    }

    protected virtual void Update()
    {
        if (player == null) return;

        //プレイヤーとの距離計算
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        if (canMove && rb != null) MoveTowardsPlayer();
        
        if (distanceToPlayer <= detectionRadius)
        {
            Attack(); //攻撃処理(子クラスで実装)
        }

        FlipSprite();

    }

    public void FlipSprite()
    {
        if (player == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        //プレイヤーが左側なら `flipX = true`、右側なら `false`
        sr.flipX = player.transform.position.x < transform.position.x;
    }

    protected virtual void MoveTowardsPlayer()
    {
        if(!canMove)return; //移動不可の敵なら何もしない

        //プレイヤーの方向へ移動
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    protected virtual void OnHit(){
        if (EnemyDieEffect != null)
        {
            //消滅時にエフェクト生成
            GameObject effe = Instantiate(EnemyDieEffect, transform.position, Quaternion.identity); 
            Destroy(effe,0.5f);

        }

        Destroy(gameObject);

    }

    

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Princess") || collision.gameObject.CompareTag("Leaf"))
        {
            OnHit();
        }    
    }


    //各敵の攻撃処理(子クラスでオーバーライド)
    protected abstract void Attack();
}
