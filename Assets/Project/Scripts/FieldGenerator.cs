using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
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
        chunks = new Chunk[Const.fieldLengthX, Const.fieldLengthY];
        field = new GameObject("Filed");
        princessRg = princess.gameObject.GetComponent<Rigidbody2D>();
        chunkGenerator = chunkGeneratorPrefab.GetComponent<ChunkGenerator>();
        enemyGenerator = enemyGeneratorPrefab.GetComponent<EnemyGenerator>();

        InitChunks();
    }

    void Update()
    {
        //自機がいるチャンクの座標を取得
        int px = Utils.GetChunkX(princessRg.position.x);
        int py = Utils.GetChunkY(princessRg.position.y);

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
        for (int y = 0; y < Const.fieldLengthY; y++) {
            for (int x = 0; x < Const.fieldLengthX; x++) {
                Chunk chunk = chunkGenerator.CreateChunk(60, 30, 10, 5);
                chunks[x, y] = chunk;

                //ExistManagerに作成済みの座標を更新
                int px = (x - Const.fieldLengthX / 2) * Const.chunkSizeX;
                int py = (y - Const.fieldLengthY / 2) * Const.chunkSizeY;
                for (int i = 0; i < chunk.exists.GetLength(0); i++) {
                    for (int j = 0; j < chunk.exists.GetLength(1); j++) {
                        int ax = px + (j - chunk.exists.GetLength(1) / 2);
                        int ay = py + (i - chunk.exists.GetLength(0) / 2);
                        ExistManager.GetInstance().Put(ax, ay);
                    }
                }
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
        int wx = (x + Const.fieldLengthX) % Const.fieldLengthX;
        int wy = (y + Const.fieldLengthY) % Const.fieldLengthY;
        Chunk chunk = chunks[wx, wy];

        //チャンクのワールド座標を設定
        Vector3 position = new Vector3();
        position.x = x * Const.chunkSizeX;
        position.y = y * Const.chunkSizeY;

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
