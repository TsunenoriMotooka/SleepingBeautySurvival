using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] Enemys;
    Dictionary<GameObject,InstantiateParameters> dic = new Dictionary<GameObject, InstantiateParameters>();

    void Start()
    {
        RandomEnemy();
    }

    void Update()
    {
        
        
    }

    void RandomEnemy(){

        // float x = Random.Range(-22f,22f);
        // float y = Random.Range(-22f,22f);
        float ran = Random.Range(1f,10f);
        
        for(int i=0;i<ran;i++){
            for(int j=0;j<Enemys.Length;j++){
                float x = Random.Range(-22f,22f);
                float y = Random.Range(-22f,22f);
                Vector2 Pos = new Vector2(x,y);
                Instantiate(Enemys[i], Pos,Quaternion.identity);
                dic.Add(Enemys[i],);
            }    
        }
    }


}
