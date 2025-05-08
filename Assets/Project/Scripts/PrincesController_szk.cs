using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Rendering;

public class PrincesController_szk : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth;
    public int health{get{return currentHealth;}}

    public float timeInvincible = 2.0f;
    bool isInvinsible;
    float invinsibleTimer;

    Rigidbody2D rb;
    public float speed = 5f;
    Animator anim;

    public GameObject prefab;
    Vector2 lookDirection = new Vector2(1f,0);

    IEnumerator PrincesAttack(){
        
        while(true){
            GameObject leaf = Instantiate(
            prefab,
            rb.position + Vector2.up*0.5f,
            Quaternion.identity);
                if(prefab != null){
                    leaf.GetComponent<PrincesAttackController_szk>().Attack(lookDirection);
                }
                yield return new WaitForSeconds(2f);
            }
        }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(PrincesAttack());
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
    public void ChangeHealth(int amount){
        if(amount < 0){
            if(isInvinsible)return;
            isInvinsible = true;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount,0,maxHealth);
    }

    void Update()
    {
        HandleKeyInput();
        HandleTouchInput();
    }
}
