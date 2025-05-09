using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject[] terrainPrefabs;
    public GameObject[] treePrefabs;
    public GameObject[] rockPrefabs;
    public GameObject[] tallRockPrefabs;
    public GameObject[] bigRockPrefabs;

    public GameObject enemyGeneratorPrefab;

    public Chunk CreateChunk(int treeCount, int rockCount, int tallRockCount, int bigRockCount) {
        Chunk chunk = new Chunk(terrainPrefabs.Length, rockPrefabs.Length, rockCount, tallRockPrefabs.Length, tallRockCount, bigRockPrefabs.Length, bigRockCount, treePrefabs.Length, treeCount);
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
                    position.x = (x - chunk.terrains.GetLength(0) / 2) * Const.terrainSizeX; 
                    position.y = (y - chunk.terrains.GetLength(0) / 2) * Const.terrainSizeX;
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
                    GameObject tree = Instantiate(
                    prefab,
                    position,
                    Quaternion.identity);
                    tree.transform.parent = parent;
                }
            }
        }       
    }

    //TODO: TEST
    void Start()
    {
        Chunk chunk = CreateChunk(60, 30, 10, 5);
        CreateObject(chunk);

        EnemyGenerator enemyGenerator = enemyGeneratorPrefab.GetComponent<EnemyGenerator>();
        enemyGenerator.GenerateEnemies(0, 0);
    }
}
