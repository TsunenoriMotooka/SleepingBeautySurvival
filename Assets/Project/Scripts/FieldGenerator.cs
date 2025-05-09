using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public int fieldSizeX = 9;
    public int fieldSizeY = 9;

    GameObject field;
    public GameObject chunkGeneratorPrefab;
    ChunkGenerator chunkGenerator;
    public GameObject enemyGeneratorPrefab;
    EnemyGenerator enemyGenerator;

    public Transform princess;
    Rigidbody2D princessRg;

    Chunk[,] chunks;
    Dictionary<(int, int), GameObject> chunkObjects = new Dictionary<(int, int), GameObject>();

    void Start()
    {
        chunks = new Chunk[fieldSizeX, fieldSizeY];
        field = new GameObject("Filed");
        princessRg = princess.gameObject.GetComponent<Rigidbody2D>();
        chunkGenerator = chunkGeneratorPrefab.GetComponent<ChunkGenerator>();
        enemyGenerator = enemyGeneratorPrefab.GetComponent<EnemyGenerator>();

        InitChunks();
    }

    void Update()
    {
        //自機がいるチャンクの座標を取得
        int px = (int)Math.Round(princessRg.position.x / (float)45);
        int py = (int)Math.Round(princessRg.position.y / (float)45);

        // チャンクの生成
        // チャンクの座標周囲１マスを作成
        for (int ay = -1; ay <= 1; ay++) {
            for (int ax = -1; ax <= 1; ax++) {
                if (ax == 0 && ay == 0) continue;
                
                //チャンクの座標
                int x = px + ax;
                int y = py + ay;
                
                //作成済みの場合は無視
                if (chunkObjects.ContainsKey((x, y))) continue;

                //チャンクの作成
                CreateChunk(x, y);

                //モンスターの作成
                CreateEnemys(x, y);
            }
        }

        // チャンクの削除
        // チャンクの座標周囲２マス目を削除
        for (int ay = -2; ay <= 2; ay++) {
            for (int ax = -2; ax <= 2; ax++) {
                if (ax >= -1 && ax <= 1 && ay >= -1 && ay <= 1) continue;

                //チャンクの配列座標
                int x = px + ax;
                int y = py + ay;

                //チャンクを削除
                RemoveChunk(x, y);
            
                //モンスターを削除
                RemoveEnemies(x, y);
            }
        }
    }

    void InitChunks()
    {
        // Chunkの作成
        for (int y = 0; y < fieldSizeY; y++) {
            for (int x = 0; x < fieldSizeX; x++) {
                ChunkGenerator chunkGenerator = chunkGeneratorPrefab.GetComponent<ChunkGenerator>();
                Chunk chunk = chunkGenerator.CreateChunk(60, 30, 10, 5);
                chunks[x, y] = chunk;
            }
        }

        // 初期画面生成
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                CreateChunk(x, y);

                if (x != 0 || y != 0) {
                    CreateEnemys(x, y);
                }
            }
        }
    }

    void CreateChunk(int x, int y)
    {   
        //チャンク情報を取得
        int wx = (x + fieldSizeX) % fieldSizeX;
        int wy = (y + fieldSizeY) % fieldSizeY;
        Chunk chunk = chunks[wx, wy];

        //チャンクのワールド座標を設定
        Vector3 position = new Vector3();
        position.x = x * Chunk.chunkSizeX;
        position.y = y * Chunk.chunkSizeY;

        //チャンクを生成
        GameObject chunkObject = chunkGenerator.CreateObject(chunk);
        chunkObject.transform.parent = field.transform;
        chunkObject.transform.position = position;

        //生成したチャンクをキャッシュ
        RemoveChunk(x, y);
        chunkObjects[(x, y)] = chunkObject;
    }

    void RemoveChunk(int x, int y)
    {
        if (!chunkObjects.ContainsKey((x, y))) return;

        //キャッシュしたチャンクを取得
        GameObject chunkObject = chunkObjects[(x, y)];

        //チャックを削除
        Destroy(chunkObject);

        //キャッシュしたチャンクを削除
        chunkObjects.Remove((x, y));
    }

    void CreateEnemys(int x, int y)
    {
        enemyGenerator.GenerateEnemies(x, y);
    }

    void RemoveEnemies(int x, int y)
    {
        enemyGenerator.ClearEnemies(x, y);
    }
}
