using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    private Rigidbody2D rb;
    public GameObject bulletObject;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        bulletObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal,vertical) * 6.0f * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        if(Input.GetKeyDown(KeyCode.Space)){
            ShootBullet();
        }
    }

    void ShootBullet(){
        bulletObject.SetActive(true);
        GameObject bullet = Instantiate(bulletObject, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        Vector2 shootDirection = transform.right;
        bulletRb.velocity = shootDirection * bulletSpeed;

        Destroy(bullet, 5f);
    }
}
