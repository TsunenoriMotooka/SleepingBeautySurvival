using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Callbacks;

public class PrincesAttackController_szk : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    public void Attack(Vector3 direction){
        rb.AddForce(direction * 10f ,ForceMode2D.Impulse);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyBase eneCon = other.collider.GetComponent<EnemyBase>();
        Destroy(gameObject);
    }

}
