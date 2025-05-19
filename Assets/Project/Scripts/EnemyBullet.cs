using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Stop()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }
}
