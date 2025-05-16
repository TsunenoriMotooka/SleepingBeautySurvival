using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEditor.Search;
using DG.Tweening;
using Unity.VisualScripting;

public class PrincessController : MonoBehaviour
{
    public int maxHealth = 10;
    int currentHealth;
    public int health{get{return currentHealth;}}

    public float timeInvincible = 2.0f;
    bool isInvinsible;
    float invinsibleTimer;

    Rigidbody2D rb;
    public float speed = 5f;
    Animator anim;

    public GameObject[] prefabs;
    Vector2 lookDirection = new Vector2(1f,0);

    [HideInInspector] //GameDirectorから取得
    public AudioGenerator audioGenerator;

    private GameObject princessBullets;

    public GameObject detectClearKeyPrefab;
    private GameObject detectClearKey;

    IEnumerator PrincessLeafBulletAttack()
    {
        while (true)
        {
            GameObject leafBullet = Instantiate(
                prefabs[0],
                rb.position,
                Quaternion.identity);
            leafBullet.transform.parent = princessBullets.transform;
            leafBullet.GetComponent<PrincessLeafBulletController>().Attack(lookDirection);

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator PrincessRoseBulletAttack(){
        while(true){
            GameObject roseBullet = Instantiate(
                prefabs[1],
                rb.position,
                Quaternion.identity);
            roseBullet.transform.parent = princessBullets.transform;
            roseBullet.GetComponent<PrincessRoseBulletController>().princess = transform;

            //3秒後に消滅
            Destroy(roseBullet, 3f);

            //5秒後に再出現
            yield return new WaitForSeconds(5f);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        princessBullets = new GameObject("PrincessBullets");
        StartCoroutine(PrincessLeafBulletAttack());
        StartCoroutine(PrincessRoseBulletAttack());

        detectClearKey = Instantiate(detectClearKeyPrefab, transform.position, Quaternion.identity);
    }

    void HandleKeyInput(){
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(moveX,moveY);
        if(move.sqrMagnitude > 0f){
            lookDirection.Set(move.x,move.y);
            lookDirection.Normalize();
        }
        anim.SetFloat("moveX",lookDirection.x);
        anim.SetFloat("moveY",lookDirection.y);
        anim.SetFloat("speed",move.magnitude);
        Vector2 position = rb.position;
        position.x = position.x + speed * moveX * Time.deltaTime;
        position.y = position.y + speed * moveY * Time.deltaTime;
        rb.MovePosition(position);

    }

    void HandleTouchInput(){
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved){
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,10));
                Vector2 targetPosition = new Vector2(newPosition.x,newPosition.y);

                Vector2 move = (targetPosition - rb.position).normalized;

                if(move.sqrMagnitude > 0f){
                    lookDirection.Set(move.x,move.y);
                    lookDirection.Normalize();
                }

                anim.SetFloat("moveX",lookDirection.x);
                anim.SetFloat("moveY",lookDirection.y);
                anim.SetFloat("speed",move.magnitude);

                Vector2 position = rb.position;
                position.x = position.x + speed * move.x * Time.deltaTime;
                position.y = position.y + speed * move.y * Time.deltaTime;
                rb.MovePosition(position);
                
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Monster") || other.CompareTag("MonsterBullet")){
            ChangeHealth(-1);
        }
        if(other.CompareTag("ClearKey")){
            other.enabled = false;
            
            other.transform.DOMoveY(other.transform.position.y +2f,1f)
            .SetEase(Ease.OutQuad).OnComplete(() => Destroy(other.gameObject,0.5f));

            ClearKeyManager.GetInstance().Found();
            audioGenerator.PlaySE(SE.GetClearKey, transform);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {    
        if(other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("MonsterBullet")){
            ChangeHealth(-1);
        }
    }

    public void ChangeHealth(int amount){
        if(amount < 0){
            if(isInvinsible)return;
            isInvinsible = true;
            invinsibleTimer = timeInvincible;
            anim.SetTrigger("hit");
            
            //ダメージ時の効果音再生
            if (currentHealth + amount > 0) {
                audioGenerator.PlaySEDamagePrincess();
            }
        }
        currentHealth = Mathf.Clamp(currentHealth + amount,0,maxHealth);
        // Debug.Log(currentHealth + "/" + maxHealth);
        HealthUIController.instance.SetValue(currentHealth / (float)maxHealth);
        if (currentHealth <= 0){
            isInvinsible = true;
            rb.simulated = false;
            anim.enabled = false;
            StopAllCoroutines();

            audioGenerator.PlaySE(SE.PrincessDead);
        }
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        HandleKeyInput();
        HandleTouchInput();

        if (isInvinsible)
        {
            invinsibleTimer -= Time.deltaTime;
            if (invinsibleTimer < 0)
            {
                isInvinsible = false;
            }
        }

        detectClearKey.transform.position = transform.position;
    }
}
