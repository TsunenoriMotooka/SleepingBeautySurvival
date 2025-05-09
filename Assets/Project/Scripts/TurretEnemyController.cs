using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyController : EnemyBase
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;
    public float shootRadius = 9f; //攻撃開始範囲
    private float lastShootTime = 0f;

    protected override void Start()
    {
        base.Start();
        canMove = false; //砲台の為、移動させないようにする 

        // //定期的に弾発射
        // InvokeRepeating("ShootBullet", 0f, fireRate);
    }

    protected override void Update()
    {
        if(player == null)return;
    
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= shootRadius && Time.time >= lastShootTime + fireRate)
        {
            ShootBullet();
            lastShootTime = Time.time;
        }

    }

    //直接攻撃じゃない為、空のAttackメソッド
    protected override void Attack(){}

    //弾発射処理
    void ShootBullet()
    {

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.transform.position - transform.position).normalized;
        bulletRb.velocity = direction * bulletSpeed;


    }
}
