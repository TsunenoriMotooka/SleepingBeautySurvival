using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEditor.Search;

public class PrincesController_szk : MonoBehaviour
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

    public AudioGenerator audioGenerator;

    IEnumerator PrincesAttack(){
        
        while(true){
            GameObject leaf = Instantiate(
            prefabs[0],
            rb.position,
            Quaternion.identity);
                if(prefabs[0] != null){
                    leaf.GetComponent<PrincesAttackController_szk>().Attack(lookDirection);
                }
                yield return new WaitForSeconds(1f);
            }
        }
    IEnumerator SpawnAttack(){
        while(true){
        GameObject attack = Instantiate(
            prefabs[1],
            rb.position,
            Quaternion.identity);
                if(prefabs[1] != null){
                    attack.AddComponent<AttackBehavior>();
                }
            Destroy(attack,3f);
            yield return new WaitForSeconds(5f);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(PrincesAttack());
        StartCoroutine(SpawnAttack());
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
        HealthUI_Controller.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Update()
    {
        if(currentHealth == 0){
            anim.enabled = false;
            GetComponent<PrincesController_szk>().enabled = false;
            audioGenerator.PlaySE(SE.PrincessDead);
        }
        HandleKeyInput();
        HandleTouchInput();

        if(isInvinsible){
            invinsibleTimer -= Time.deltaTime;
            if(invinsibleTimer < 0){
                isInvinsible = false;
            }
        }
    }
}
