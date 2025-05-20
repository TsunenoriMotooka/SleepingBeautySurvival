using UnityEngine;

public class PrincessLeafBulletController : MonoBehaviour
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
        rb.AddForce(direction * 10f, ForceMode2D.Impulse);
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
            Destroy(gameObject);            
        }    
    }

}
