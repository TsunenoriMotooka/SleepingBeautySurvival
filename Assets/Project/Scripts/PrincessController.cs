using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEditor.Search;
using DG.Tweening;
using Unity.VisualScripting;

public class PrincessController : MonoBehaviour
{
    int currentHealth;
    public int health { get { return currentHealth; } }

    bool isInvinsible;
    float invinsibleTimer;

    Rigidbody2D rb;
    public float speed = 5f;
    Animator anim;

    public GameObject[] prefabs;
    Vector2 lookDirection = new Vector2(1f, 0);

    [HideInInspector] //GameDirectorから取得
    public AudioGenerator audioGenerator;

    private GameObject princessBullets;

    public GameObject detectClearKeyPrefab;
    private GameObject detectClearKey;

    public GameObject princessDownPrefab;

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

    IEnumerator PrincessRoseBulletAttack()
    {
        while (true)
        {
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
        currentHealth = Const.maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        princessBullets = new GameObject("PrincessBullets");
        StartCoroutine(PrincessLeafBulletAttack());
        StartCoroutine(PrincessRoseBulletAttack());

        detectClearKey = Instantiate(detectClearKeyPrefab, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster") || other.CompareTag("MonsterBullet"))
        {
            ChangeHealth(-1);
        }
        if (other.CompareTag("ClearKey"))
        {
            other.enabled = false;

            other.transform.DOMoveY(other.transform.position.y + 2f, 1f)
            .SetEase(Ease.OutQuad).OnComplete(() => Destroy(other.gameObject, 0.5f));

            ClearKeyManager.GetInstance().Found();
            audioGenerator.PlaySE(SE.GetClearKey, transform);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("MonsterBullet"))
        {
            ChangeHealth(-1);
        }
    }

    public void ChangeHealth(int amount, Transform other)
    {
        if (amount < 0)
        {
            if (isInvinsible) return;
            isInvinsible = true;
            invinsibleTimer = Const.timeInvincible;
            anim.SetTrigger("hit");

            //ダメージ時の効果音再生
            if (currentHealth + amount > 0)
            {
                audioGenerator.PlaySEDamagePrincess();
            }
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, Const.maxHealth);
        // Debug.Log(currentHealth + "/" + maxHealth);
        HealthUIController.instance.SetValue(currentHealth / (float)Const.maxHealth);
        if (currentHealth <= 0)
        {
            isInvinsible = true;
            rb.simulated = false;
            anim.enabled = false;
            StopAllCoroutines();

            GameObject go = Instantiate(princessDownPrefab);
            go.transform.position = transform.position;
            PrincessDown princessDown = go.GetComponent<PrincessDown>();
            princessDown.princess = transform;
            if (other.position.x < 0)
            {
                princessDown.RigthDown();
            }
            else
            {
                princessDown.LeftDown();
            }
            gameObject.SetActive(false);
            audioGenerator.PlaySE(SE.PrincessDead);
        }
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        HandleMouseClickMovement();
        //HandleTouchInput();

        HandleKeyInput();

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


    void HandleMouseClickMovement()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetDirection = (mousePosition - transform.position).normalized;

            rb.velocity = Vector2.Lerp(rb.velocity, targetDirection * speed * 2f, Time.deltaTime * 5f);

            lookDirection.Set(targetDirection.x, targetDirection.y);
            lookDirection.Normalize();

            anim.SetFloat("moveX", lookDirection.x);
            anim.SetFloat("moveY", lookDirection.y);
            anim.SetFloat("speed", targetDirection.magnitude);
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetFloat("speed", 0f);
        }
    }
    
    void HandleKeyInput()
    {
        if (Input.GetMouseButton(0)) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(moveX, moveY);
        if (move.sqrMagnitude > 0f)
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        anim.SetFloat("moveX", lookDirection.x);
        anim.SetFloat("moveY", lookDirection.y);
        anim.SetFloat("speed", move.magnitude);
        Vector2 position = rb.position;
        position.x = position.x + speed * moveX * Time.deltaTime;
        position.y = position.y + speed * moveY * Time.deltaTime;
        rb.MovePosition(position);
    }
}
