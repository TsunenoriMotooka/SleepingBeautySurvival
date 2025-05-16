using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyController : EnemyBase
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;
    public float shootRadius = 9f; //攻撃開始範囲
    private float lastShootTime = 0f;

    protected override void Start()
    {
        base.Start();
        canMove = false; 

    }

    protected override void Update()
    {
        base.Update();

        if(player == null)return;
    
        if (IsVisible && Time.time >= lastShootTime + fireRate)
        {
            ShootBullet();
            lastShootTime = Time.time;
        }

        FlipSprite();
    }

    //弾発射処理
    void ShootBullet()
    {
        Vector3 firePoint = new Vector3(0, 1.1f, 0) + transform.position;

        GameObject bullet = Instantiate(bulletPrefab, firePoint, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.transform.position + new Vector3(0f, 0.6f, 0) - firePoint).normalized;
        bulletRb.velocity = direction * bulletSpeed;
    }

    


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Princess"))
        {
            animator.SetTrigger("AttackTrigger");
        }
    }
}
