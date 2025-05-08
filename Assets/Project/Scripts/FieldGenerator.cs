using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public int fieldSizeX = 9;
    public int fieldSizeY = 9;

    GameObject field;
    public GameObject chunkGeneratorPrefab;
    ChunkGenerator chunkGenerator;

    public Transform princess;
    Rigidbody2D princessRg;

    Chunk[,] chunks;

    void Start()
    {
        chunks = new Chunk[fieldSizeX, fieldSizeY];
        field = new GameObject("Filed");
        princessRg = princess.gameObject.GetComponent<Rigidbody2D>();
        chunkGenerator = chunkGeneratorPrefab.GetComponent<ChunkGenerator>();
        
        InitChunks();
    }

    void Update()
    {
        //自機がいるチャンクの座標を取得
        int px = (int)Math.Round(princessRg.position.x / (float)45);
        int py = (int)Math.Round(princessRg.position.y / (float)45);

        // チャンクの生成
        // チャンクの座標周囲１マスを作成
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                if (x == 0 && y == 0) continue;
                CreateChunk(px + x, py + y);
            }
        }

        // チャンクの削除
        // チャンクの座標周囲２マス目を削除
        for (int y = -2; y <= 2; y++) {
            for (int x = -2; x <= 2; x++) {
                if (x >= -1 && x <= 1 && y >= -1 && y <= 1) continue;
                RemoveChunk(px + x, py + y);
            }
        }
    }

    void InitChunks()
    {
        // Chunkの作成
        for (int y = 0; y < fieldSizeY; y++) {
            for (int x = 0; x < fieldSizeX; x++) {
                ChunkGenerator chunkGenerator = chunkGeneratorPrefab.GetComponent<ChunkGenerator>();
                Chunk chunk = chunkGenerator.CreateChunk(80, 0, 0, 0);
                chunks[x, y] = chunk;
            }
        }

        // 初期画面生成
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                CreateChunk(x, y);
            }
        }
    }

    void CreateChunk(int x, int y)
    {   
        int wx = (x + fieldSizeX) % fieldSizeX;
        int wy = (y + fieldSizeY) % fieldSizeY;
        Chunk chunk = chunks[wx, wy];
        //作成済みの時は作成しない
        if (chunk.IsCreated) return;

        Vector3 position = new Vector3();
        position.x = x * Chunk.chunkSizeX;
        position.y = y * Chunk.chunkSizeY;

        GameObject chunkObject = chunkGenerator.CreateObject(chunk);
        chunkObject.transform.parent = field.transform;
        chunkObject.transform.position = position;
    }

    void RemoveChunk(int x, int y)
    {
        int wx = (x + fieldSizeX) % fieldSizeX;
        int wy = (y + fieldSizeY) % fieldSizeY;
        Chunk chunk = chunks[wx, wy];

        GameObject chunkObject = chunk.chunkObject;
        if (chunkObject == null) return;

        //チャックを削除
        Destroy(chunkObject);
        //保存したチャンクを削除
        chunk.chunkObject = null;
    }
}
