using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RammingEnemyController : EnemyBase
{
    public float chargeSpeed = 6f;
    public float attackRadius = 1.3f;
    private bool hasHitPlayer = false;

    protected override void Attack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius && !hasHitPlayer)
        {
            StartCharge();
        }
    }

    void StartCharge()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chargeSpeed;
    }


}
