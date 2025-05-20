using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    Transform target;
    float speed = 10f;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector2 targetPosition = new Vector2(
                target.position.x,
                target.position.y + 1f
                ); // 着弾位置を調整
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
                );
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject,2f);
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Monster"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Destroy(gameObject); // 敵に当たったら消す
        }
    }
}
