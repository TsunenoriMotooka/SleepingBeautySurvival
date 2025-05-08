using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject[] terrainPrefabs;
    public GameObject[] treePrefabs;
    public GameObject[] rock1Prefabs;
    public GameObject[] rock2Prefabs;
    public GameObject[] rock3Prefabs;

    static int terrainSizeX = 9;
    static int terrainSizeY = 9;

    public Chunk CreateChunk(int treeCount, int rock1Count, int rock2Count, int rock3Count) {
        Chunk chunk = new Chunk(terrainPrefabs.Length, rock1Prefabs.Length, rock1Count, rock2Prefabs.Length, rock2Count, rock3Prefabs.Length, rock3Count, treePrefabs.Length, treeCount);
        return chunk;
    }
 
    public GameObject CreateObject(Chunk chunk){

        GameObject chunkObject = new GameObject("Chunk");
        
        GameObject terrainsObject = new GameObject("Terrains");
        terrainsObject.transform.parent = chunkObject.transform;
        for (int y = 0; y < chunk.terrains.GetLength(0); y++) {
            for (int x = 0; x < chunk.terrains.GetLength(1); x++) {
                if (chunk.terrains[x, y] != 0) {
                    Vector3 position = new Vector3();
                    position.x = (x - chunk.terrains.GetLength(0)/2) * terrainSizeX; 
                    position.y = (y - chunk.terrains.GetLength(0)/2) * terrainSizeY;
                    GameObject prefab = terrainPrefabs[chunk.terrains[x, y] - 1];
                    GameObject terrain = Instantiate(
                    prefab,
                    position,
                    Quaternion.identity);
                    terrain.transform.parent = terrainsObject.transform;
                }
            }
        }

        GameObject treesObject = new GameObject("Trees");
        treesObject.transform.parent = chunkObject.transform;
        for (int y = 0; y < chunk.trees.GetLength(0); y++) {
            for (int x = 0; x < chunk.trees.GetLength(1); x++) {
                if (chunk.trees[x, y] != 0) {
                    Debug.Log(chunk.trees[x, y]);
                    Vector3 position = new Vector3();
                    position.x = x - chunk.trees.GetLength(0)/2; 
                    position.y = y - chunk.trees.GetLength(1)/2;
                    GameObject prefab = treePrefabs[chunk.trees[x, y] - 1];
                    GameObject tree = Instantiate(
                    prefab,
                    position,
                    Quaternion.identity);
                    tree.transform.parent = treesObject.transform;
                }
            }
        }

        GameObject rocksObject = new GameObject("Rocks");
        rocksObject.transform.parent = chunkObject.transform;

        return chunkObject;
    }

    //TODO: TEST
    void Start()
    {
        Chunk chunk = CreateChunk(80, 0, 0, 0);
        CreateObject(chunk);
    }
}
