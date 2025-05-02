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

    public Rigidbody2D rb;
    public float speed = 5f;
    Animator anim;

    public GameObject prefab;
    Vector2 lookDirection = new Vector2(1f,0);

    IEnumerator PrincesAttack(){
        GameObject Leaf = Instantiate(
            prefab,
            rb.position + Vector2.up*0.5f,
            Quaternion.identity
        );
            while(true){
                if(prefab != null){
                    prefab.GetComponent<PrincesAttackController_szk>().Attack(lookDirection);
                }
                yield return new WaitForSeconds(5f);
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

        Vector3 movement = new Vector2(moveX,moveY) * speed;
        rb.velocity = movement;

        anim.SetFloat("moveX",moveX);
        anim.SetFloat("moveY",moveY);
    }

    void HandleTouchInput(){
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved){
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,10));
                rb.DOMove(new Vector2(newPosition.x,newPosition.y),0.5f);

                Vector2 direction = (new Vector2(newPosition.x,newPosition.y) - rb.position).normalized;
                anim.SetFloat("moveX",direction.x);
                anim.SetFloat("moveY",direction.y);
                
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
