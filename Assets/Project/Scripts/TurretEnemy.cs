using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretEnemy : MonoBehaviour
{

    public Transform Player;
    public Transform PlayerBullet;
    public GameObject EnemyBullet;
    public float bulletSpeed = 5f;
    private bool hasShot = false;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()

    {
       rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float ditanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if(ditanceToPlayer <= 10f && !hasShot){
            Debug.Log("player IN");
            ShootBullet();
            hasShot = true;
        } 

    }

    void ShootBullet(){
        EnemyBullet.SetActive(true); // 事前に非表示にしておいた弾を表示
        Rigidbody2D rb = EnemyBullet.GetComponent<Rigidbody2D>();

        Vector2 direction = (Player.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed; // 弾をプレイヤー方向へ飛ばす

        EnemyBullet.GetComponent<SpriteRenderer>().DOFade(0f, 1f).OnComplete(() => EnemyBullet.SetActive(false));

    }


    //DOTweenで消える
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerBullet")){
            GetComponent<SpriteRenderer>().DOFade(0f,1f).OnComplete(()=>Destroy(gameObject));
        }
        
    }
}
