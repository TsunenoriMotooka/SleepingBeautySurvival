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

    }

    protected virtual void MoveTowardsPlayer()
    {
        if(!canMove)return; //移動不可の敵なら何もしない

        //プレイヤーの方向へ移動
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    protected virtual void OnHit(){
        StartCoroutine(BlinkDestroy());
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        //姫と弾との当たったら、OnHit()実行(姫と弾はテスト的に作成したオブジェクト)
        if(other.CompareTag("Princess") || other.CompareTag("Leaf")){
            OnHit();
        }
        
    }

    //敵点滅後、消滅処理
    IEnumerator BlinkDestroy(){
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for(int i = 0; i < blinkCount; i++){
            sr.DOFade(0f, blinkSpeed);
            yield return new WaitForSeconds(blinkSpeed);
            sr.DOFade(1f, blinkSpeed);
            yield return new WaitForSeconds(blinkSpeed);
            
        }

        Destroy(gameObject);

    }

    //各敵の攻撃処理(子クラスでオーバーライド)
    protected abstract void Attack();
}
