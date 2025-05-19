using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    Transform target;
    public float speed = 5f;
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;   
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
                );
            if(Vector2.Distance(transform.position,target.position) < 0.1f){
                Destroy(gameObject);
            }
        }
    }
}
