using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    public float speed = 5f;
    public float radius = 3f;
    private GameObject princess;

    void Start()
    {
        princess = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
       if(princess != null){
        float x = radius * Mathf.Sin(Time.time * speed) + princess.transform.position.x;
        float y = radius * Mathf.Cos(Time.time * speed) + princess.transform.position.y;
        transform.position = new Vector3(x,y,transform.position.z);
       } 
    }
}
