using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject[] terrainPrefabs;
    public GameObject[] treePrefabs;
    public GameObject[] rockPrefabs;
    public GameObject[] tallRockPrefabs;
    public GameObject[] bigRockPrefabs;
    public GameObject[] lightPrefabs;

    public GameObject enemyGeneratorPrefab;
    public GameObject clearKeyGeneratorPrefab;

    [HideInInspector] //FieldGeneratorから取得
    public DayNightSystem2D dayNightSystem2D;

    public Chunk CreateChunk(int chunkX, int chunkY, int treeCount, int rockCount, int tallRockCount, int bigRockCount, int lightCount) {
        Chunk chunk = new Chunk(chunkX, chunkY, terrainPrefabs.Length, rockPrefabs.Length, rockCount, tallRockPrefabs.Length, tallRockCount, bigRockPrefabs.Length, bigRockCount, treePrefabs.Length, treeCount, lightPrefabs.Length - 1, lightCount);
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
                    position.x = (x - chunk.terrains.GetLength(1) / 2) * Const.terrainSizeX; 
                    position.y = (y - chunk.terrains.GetLength(0) / 2) * Const.terrainSizeY;
                    GameObject prefab = terrainPrefabs[chunk.terrains[x, y] - 1];
                    GameObject terrain = Instantiate(
                    prefab,
                    position,
                    Quaternion.identity);
                    terrain.transform.parent = terrainsObject.transform;
                }
            }
        }

        GameObject lightObject = new GameObject("Lights");
        lightObject.transform.parent = chunkObject.transform;
        addObjects(lightObject.transform, chunk.lights, lightPrefabs);
        //チャンク内のライトをDayAndLightSystem2Dに追加
        for (int i = 0; i < lightObject.transform.childCount; i++) {
            Transform light = lightObject.transform.GetChild(i);
            if (light == null) continue;
            Light2D light2D = light.GetChild(0).gameObject.GetComponent<Light2D>();  
            dayNightSystem2D.addMapLight(light2D);
        }  

        GameObject treesObject = new GameObject("Trees");
        treesObject.transform.parent = chunkObject.transform;
        addObjects(treesObject.transform, chunk.trees, treePrefabs);

        GameObject rocksObject = new GameObject("Rocks");
        rocksObject.transform.parent = chunkObject.transform;
        addObjects(rocksObject.transform, chunk.rocks, rockPrefabs);
        addObjects(rocksObject.transform, chunk.tallRocks, tallRockPrefabs);
        addObjects(rocksObject.transform, chunk.bigRocks, bigRockPrefabs);
        

        return chunkObject;
    }

    void addObjects(Transform parent, int[,] positions, GameObject[] prefabs)
    {
        for (int y = 0; y < positions.GetLength(0); y++) {
            for (int x = 0; x < positions.GetLength(1); x++) {
                if (positions[x, y] != 0) {
                    Vector3 position = new Vector3();
                    position.x = x - (positions.GetLength(0) / 2); 
                    position.y = y - (positions.GetLength(1) / 2);
                    GameObject prefab = prefabs[positions[x, y] - 1];
                    GameObject obj = Instantiate(
                    prefab,
                    position,
                    Quaternion.identity);
                    obj.transform.parent = parent;
                }
            }
        }       
    }

    //TODO: TEST
    void Start()
    {
        Chunk chunk = CreateChunk(0, 0, 120, 30, 5, 3, 10);
        CreateObject(chunk);

        EnemyGenerator enemyGenerator = enemyGeneratorPrefab.GetComponent<EnemyGenerator>();
        enemyGenerator.GenerateEnemies(0, 0);

        ClearKeyGenerator clearKeyGenerator = clearKeyGeneratorPrefab.GetComponent<ClearKeyGenerator>();
        clearKeyGenerator.InitOneChunk();
        clearKeyGenerator.GenerateClearKeys(0, 0);
    }
}
