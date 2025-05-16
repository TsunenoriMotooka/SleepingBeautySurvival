using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] Enemys;

    public Transform princess;
    public AudioGenerator audioGenerator;

    private Dictionary<(int, int), List<GameObject>> enemyChunks = new Dictionary<(int, int), List<GameObject>>();
    private Vector2Int currentChunk = new Vector2Int(0, 0);

    private GameObject enemys;
    private GameObject enemyBullets;

    public void Start()
    {
        enemys = new GameObject("Enemys");
        enemyBullets = new GameObject("EnemyBullets");
        enemyBullets.transform.parent = enemys.transform;
    }

    //敵を生成する処理
    public void GenerateEnemies(int chunkX, int chunkY)
    {
        if (Enemys.Length == 0) return;

        float baseX = chunkX * Const.chunkSizeX;
        float baseY = chunkY * Const.chunkSizeY;

        List<GameObject> spawnedEnemies = new List<GameObject>();

        float enemyCount = Random.Range(10f, 20f);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyPrefab = Enemys[Random.Range(0, Enemys.Length)];

            // オブジェクトのない場所に配置できるまで繰り返す
            for (int j = 0; j < 20; j++)
            {
                float xOffset = Random.Range(-Const.chunkSizeX / 2f, Const.chunkSizeX / 2f);
                float yOffset = Random.Range(-Const.chunkSizeX / 2f, Const.chunkSizeX / 2f);
                Vector2 spawnPos = new Vector2(baseX + xOffset, baseY + yOffset);
                // 配置場所にオブジェクトがある場合は、やり直す。
                if (ExistPositionManager.GetInstance().Contains(spawnPos)) continue;

                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                enemy.transform.parent = enemys.transform;
                EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
                enemyBase.player = princess.transform;
                enemyBase.audioGenerator = audioGenerator;

                spawnedEnemies.Add(enemy);
                break;
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
            if (enemy != null) Destroy(enemy);
        }

        //キャッシュされた敵リストを削除
        enemyChunks.Remove((chunkX, chunkY));
    }
}
