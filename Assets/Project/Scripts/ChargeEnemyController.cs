using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemyController : EnemyBase
{
    public float chargeSpeed = 6f; 
    public float attackRadius = 8f;
    private bool isCharging = false;

    protected override void Update()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (!isCharging && distanceToPlayer <= attackRadius)
        {
            StartCharge();
        }
    }

    void StartCharge()
    {
        isCharging = true; // 突進フラグをON
        // **プレイヤーの方向を取得し、そのまま突進**
        Vector2 chargeDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed;
    }

    protected override void MoveTowardsPlayer()
    {
        // **通常の移動処理はしない（追尾しない）**
        if (isCharging) return; // 突進中ならそのまま進む

        base.MoveTowardsPlayer(); // 追尾動作をしないようにする
    }

    
    protected override void Attack(){}

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
