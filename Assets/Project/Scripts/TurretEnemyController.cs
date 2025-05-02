using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyController : EnemyBase
{
    public GameObject bulletObject;
    public float bulletSpeed = 5f;
    private bool hasShot = false;

    protected override void Start()
    {
        base.Start();
        canMove = false; // 移動しないようにする！
    }

    protected override void Attack()
    {
        if (!hasShot)
        {
            ShootBullet();
            hasShot = true;
        }
    }

    void ShootBullet()
    {
        bulletObject.SetActive(true);
        Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }
}
