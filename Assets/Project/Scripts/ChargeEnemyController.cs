using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeEnemyController : EnemyBase
{
    public float chargeSpeed = 6f;
    private bool isCharging = false;
    private Vector2 lastChargeDirection;

    protected override void Start()
    {
        base.Start();

        canMove = false; 
    }

    protected override void Update()
    {
        base.Update();

        if (!isCharging)
        {
            FlipSprite();
        }

        //プレイヤーが攻撃範囲内にいるときのみ突進開始
        if (IsVisible && !isCharging)
        {

            ChargeAttack();
        }
    }

    void ChargeAttack()
    {
        isCharging = true;
        lastChargeDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = lastChargeDirection * chargeSpeed;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Princess"))
        {

            animator.SetTrigger("AttackTrigger");

            isCharging = false; //突進モード解除
            rb.velocity = Vector2.zero; //完全停止
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            // GetComponent<Collider2D>().enabled = false;
            
            StartCoroutine(ContinueChargeAfterDelay(2f)); //数秒後に動き出す
        }
    }

    IEnumerator ContinueChargeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = lastChargeDirection * chargeSpeed;

        // GetComponent<Collider2D>().enabled = false;

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
