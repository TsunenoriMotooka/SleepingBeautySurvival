using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyController : EnemyBase
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;

    protected override void Start()
    {
        base.Start();
        canMove = false; //砲台の為、移動させないようにする 

        //定期的に弾発射
        InvokeRepeating("ShootBullet", 0f, fireRate);
    }

    //直接攻撃じゃない為、空のAttackメソッド
    protected override void Attack(){}

    //弾発射処理
    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - transform.position).normalized;
        bulletRb.velocity = direction * bulletSpeed;

        Destroy(bullet, 5f);
    }
}
