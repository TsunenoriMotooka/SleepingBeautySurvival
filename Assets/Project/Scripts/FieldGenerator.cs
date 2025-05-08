using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    static int fieldSizeX = 5;
    static int fieldSizeY = 5;

    GameObject field;
    public GameObject chunkGeneratorPrefab;

    Chunk[,] chunks = new Chunk[fieldSizeX, fieldSizeY];

    void Start()
    {
        field = new GameObject("Filed");
        
        InitChunks();
        
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

        Vector3 position = new Vector3();
        position.x = x * Chunk.chunkSizeX;
        position.y = y * Chunk.chunkSizeY;

        ChunkGenerator chunkGenerator = chunkGeneratorPrefab.GetComponent<ChunkGenerator>();
        GameObject chunkObject = chunkGenerator.CreateObject(chunk);
        chunkObject.transform.parent = field.transform;
        chunkObject.transform.position = position;
    }
}
