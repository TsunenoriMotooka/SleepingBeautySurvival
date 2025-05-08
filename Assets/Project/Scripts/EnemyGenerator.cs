using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] Enemys;

    private Dictionary<(int, int), List<GameObject>> enemyChunks = new Dictionary<(int, int), List<GameObject>>();

    //敵を生成する処理
    public void GenerateEnemies(int chunkX, int chunkY)
    {
        if (Enemys.Length == 0) return;

        float baseX = chunkX * 45;
        float baseY = chunkY * 45;

        List<GameObject> spawnedEnemies = new List<GameObject>();

        float enemyCount = Random.Range(10f, 20f);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyPrefab = Enemys[Random.Range(0, Enemys.Length)];

            float xOffset = Random.Range(-22f, 22f);
            float yOffset = Random.Range(-22f, 22f);
            Vector2 spawnPos = new Vector2(baseX + xOffset, baseY + yOffset);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            enemy.GetComponent<EnemyBase>().player = GameObject.Find("Princess")?.transform;

            spawnedEnemies.Add(enemy);
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
