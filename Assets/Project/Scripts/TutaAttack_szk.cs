using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutaAttack_szk : MonoBehaviour
{
   public GameObject attackPrefab; //生成オブジェクト
   public GameObject princess; //追従キャラクター
   public float speed = 5f; //回転速度
   public float radius = 3f; //軌道の半径
   public float spawnInterval = 5f; //生成間隔

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAttack());
    }

    IEnumerator SpawnAttack(){
        while(true){
            GameObject attack = Instantiate(attackPrefab,princess.transform.position,Quaternion.identity);
            attack.AddComponent<AttackBehavior>(); //旋回処理を追加
            Destroy(attack,5f);　// 5秒後の削除
            yield return new WaitForSeconds(spawnInterval); // ５秒待つ
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
