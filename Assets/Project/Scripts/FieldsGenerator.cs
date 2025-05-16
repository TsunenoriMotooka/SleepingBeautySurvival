using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FieldsGenerator : MonoBehaviour
{
    public GameObject chunkGeneratorPrefab;
    ChunkGenerator chunkGenerator;
    public GameObject enemyGeneratorPrefab;
    EnemyGenerator enemyGenerator;
    public GameObject clearKeyPregab;
    ClearKeyGenerator clearKeyGenerator;

    [HideInInspector] //GameDirectorから取得
    public AudioGenerator audioGenerator;
    [HideInInspector] //GameDirectorから取得
    public Transform princess;
    [HideInInspector] //GameDirectorから取得
    public DayNightSystem2D dayNightSystem2D;

    Rigidbody2D princessRg;
    PrincessController princessController;

    GameObject field;
    Chunk[,] chunks;
    Dictionary<(int, int), GameObject> chunkObjects = new Dictionary<(int, int), GameObject>();

    public void Start()
    {
        princessRg = princess.gameObject.GetComponent<Rigidbody2D>();
        princessController = princess.GetComponent<PrincessController>();
        princessController.audioGenerator = audioGenerator;

        chunkGenerator = chunkGeneratorPrefab.GetComponent<ChunkGenerator>();
        chunkGenerator.dayNightSystem2D = dayNightSystem2D;

        enemyGenerator = enemyGeneratorPrefab.GetComponent<EnemyGenerator>();
        enemyGenerator.princess = princess;
        enemyGenerator.audioGenerator = audioGenerator;
        enemyGenerator.Start();

        clearKeyGenerator = clearKeyPregab.GetComponent<ClearKeyGenerator>();
        clearKeyGenerator.audioGenerator = audioGenerator;

        InitChunks();
    }

    public void Update()
    {
        //自機がいるチャンクの座標を取得
        int px, py;
        Vector2 position = princessRg.position; 
        (px, py) = Utils.PositionToChunkMatrix(position);

        // チャンクの生成
        // チャンクの座標周囲１マスを作成
        for (int ay = -1; ay <= 1; ay++)
        {
            for (int ax = -1; ax <= 1; ax++)
            {
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

                //キーの作成
                CreateClearKeys(x, y);
            }
        }

        // チャンクの削除
        // チャンクの座標周囲２マス目を削除
        for (int ay = -2; ay <= 2; ay++)
        {
            for (int ax = -2; ax <= 2; ax++)
            {
                if (ax >= -1 && ax <= 1 && ay >= -1 && ay <= 1) continue;

                //チャンクの配列座標
                int x = px + ax;
                int y = py + ay;

                //未作成の場合は無視
                if (!chunkObjects.ContainsKey((x, y))) continue;

                //チャンクを削除
                RemoveChunk(x, y);

                //モンスターを削除
                RemoveEnemies(x, y);

                //キーの作成
                RemoveClearKeys(x, y);
            }
        }
    }

    void InitChunks()
    {
        field = new GameObject("Fileds");
        chunks = new Chunk[Const.fieldMatrixX, Const.fieldMatrixY];

        // Chunkの作成
        for (int y = 0; y < Const.fieldMatrixY; y++)
        {
            for (int x = 0; x < Const.fieldMatrixX; x++)
            {
                int chunkX = x % Const.fieldMatrixX - Const.fieldMatrixX / 2;
                int chunkY = y % Const.fieldMatrixY - Const.fieldMatrixY / 2;

                int index = UnityEngine.Random.Range(0, Const.chunkTypes.Length);
                (int treeCount, int rockCount, int tallRockCount, int bigRockCount, int lightCount) = Const.chunkTypes[index];

                Chunk chunk = chunkGenerator.CreateChunk(chunkX, chunkY, treeCount, rockCount, tallRockCount, bigRockCount, lightCount);
                int wx = (Const.fieldMatrixX + chunkX) % Const.fieldMatrixX;
                int wy = (Const.fieldMatrixY + chunkY) % Const.fieldMatrixY;
                chunks[wx, wy] = chunk;
            }
        }

        // 鍵の配置設定
        clearKeyGenerator.Init();

        // 初期画面生成
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                CreateChunk(x, y);

                if (x != 0 || y != 0)
                {
                    CreateEnemys(x, y);
                }

                //キーの作成
                CreateClearKeys(x, y);
            }
        }
    }

    void CreateChunk(int x, int y)
    {   
        //チャンク情報を取得
        int wx = (Const.fieldMatrixX + x % Const.fieldMatrixX) % Const.fieldMatrixX;
        int wy = (Const.fieldMatrixY + y % Const.fieldMatrixY) % Const.fieldMatrixX;
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

        //チャンク内のライトをDayAndLightSystem2Dから削除
        for (int i = 0; i < chunkObject.transform.childCount; i++) {
            Transform lights = chunkObject.transform.GetChild(i);
            if (lights.name.Equals("Lights")) {
                for (int j = 0; j < lights.childCount; j++) {
                    Light2D light2d = lights.GetChild(j).GetChild(0).gameObject.GetComponent<Light2D>();
                    dayNightSystem2D.removeMapLight(light2d);
                }
                break;
            }
        }

        //チャックを削除
        Destroy(chunkObject);

        //キャッシュしたチャンクを削除
        chunkObjects.Remove((x, y));
    }

    void CreateEnemys(int chunkX, int chunkY)
    {
        //enemyGenerator.GenerateEnemies(chunkX, chunkY);
    }

    void RemoveEnemies(int chunkX, int chunkY)
    {
        enemyGenerator.ClearEnemies(chunkX, chunkY);
    }

    void CreateClearKeys(int chunkX, int chunkY)
    {
        clearKeyGenerator.GenerateClearKeys(chunkX, chunkY);
    }

    void RemoveClearKeys(int chunkX, int chunkY)
    {
        clearKeyGenerator.ClearClearKeys(chunkX, chunkY);
    }
}
