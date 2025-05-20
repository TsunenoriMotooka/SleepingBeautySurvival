using UnityEngine;

public class PrincessRoseBulletController : MonoBehaviour
{
    public float speed = 5f;
    public float radius = 3f;
    
    [HideInInspector] //PrincessControllerから取得
    public Transform princess;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       if(princess != null){
        float x = radius * Mathf.Sin(Time.time * speed) + princess.position.x;
        float y = radius * Mathf.Cos(Time.time * speed) + princess.position.y;
        transform.position = new Vector3(x,y,transform.position.z);
       } 
    }
}
