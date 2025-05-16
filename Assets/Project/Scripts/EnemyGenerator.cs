using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Buffers.Text;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    [HideInInspector]
    public Transform princess;    
    public AudioGenerator audioGenerator;

    private Dictionary<(int, int), List<GameObject>> enemyChunks = new Dictionary<(int, int), List<GameObject>>();

    private GameObject enemies;
    private GameObject enemyBullets;

    public void Start()
    {
        enemies = new GameObject("Enemies");
        enemyBullets = new GameObject("EnemyBullets");
        enemyBullets.transform.parent = enemies.transform;
    }

    //敵を生成する処理
    public void GenerateEnemies(int chunkX, int chunkY, int chunkType)
    {
        if (enemyPrefabs.Length == 0) return;

        float baseX = chunkX * Const.chunkSizeX;
        float baseY = chunkY * Const.chunkSizeY;

        List<GameObject> spawnedEnemies = new List<GameObject>();

        //各EnemyPrefabよりEnemyを生成する
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            //EnemyPrefab
            GameObject enemyPrefab = enemyPrefabs[i];

            //Enemyを生成する数を取得
            int spawnCount = Const.enemiesInChunk[chunkType - 1, i];

            //鍵の取得数から生成数の増減を決定
            int level = 5 - (ClearKeyManager.GetInstance().Count + 1) / 2;
            spawnCount = Mathf.Max(0, spawnCount + Random.Range(level - 2, level));

            //生成数分、作成
            for (int j = 0; j < spawnCount; j++)
            {
                // オブジェクトのない場所に配置できるまで繰り返す
                for (int k = 0; k < 20; k++)
                {
                    float xOffset = Random.Range(-Const.chunkSizeX / 2f, Const.chunkSizeX / 2f);
                    float yOffset = Random.Range(-Const.chunkSizeX / 2f, Const.chunkSizeX / 2f);
                    Vector2 spawnPos = new Vector2(baseX + xOffset, baseY + yOffset);
                    // 配置場所にオブジェクトがある場合は、やり直す。
                    if (ExistPositionManager.GetInstance().Contains(spawnPos)) continue;

                    GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                    enemy.transform.parent = enemies.transform;
                    EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
                    enemyBase.player = princess.transform;
                    enemyBase.audioGenerator = audioGenerator;

                    spawnedEnemies.Add(enemy);
                    break;
                }
            }
        }

        //チャンクごとに敵リストを保存
        enemyChunks[(chunkX, chunkY)] = spawnedEnemies;
    }

    //チャンク内の敵を一括削除
    public void ClearEnemies(int chunkX, int chunkY)
    {
        if (!enemyChunks.ContainsKey((chunkX, chunkY))) return;

        foreach (GameObject enemy in enemyChunks[(chunkX, chunkY)])
        {
            if (enemy != null && !enemy.IsDestroyed() && !enemy.GetComponent<EnemyBase>().IsVisible) {
                Destroy(enemy);                    
            }
        }

        //キャッシュされた敵リストを削除
        enemyChunks.Remove((chunkX, chunkY));
    }
}
