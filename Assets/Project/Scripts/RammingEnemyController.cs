using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammingEnemyController : EnemyBase
{
    public float chargeSpeed = 5.5f; //体当たり速度
    private bool hasHitPlayer = false; //体たり成功判定フラグ

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (IsStoped) return;

        base.Update();
    
        if (player == null) return;
        
        if(!canMove){
            return;
        }
        
        if (IsVisible)
        {
            SeekAttack(); //追跡攻撃処理
        }

        FlipSprite();
    }

    protected void SeekAttack()
    {
        //体当たり開始距離計算
        float attackToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        //プレイヤーが攻撃開始範囲にいて、まだ体当たりが成功していない場合、体当たり
        if (IsVisible && !hasHitPlayer)
        {
            StartCharge();
        }
    }

    //体当たり処理
    void StartCharge()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * chargeSpeed;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Princess"))
        {
            animator.SetTrigger("AttackTrigger"); // ✅ **トリガーを発動**
        }
    }
}
