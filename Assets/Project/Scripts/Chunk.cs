using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunk
{
    public int[,] terrains{get;}

    public int[,] trees{get;}
    public int[,] rocks{get;}
    public int[,] tallRocks{get;}
    public int[,] bigRocks{get;}    

    public int[,] exists{get;}
    
    public GameObject chunkObject{get; set;}
    public bool IsCreated{get{return this.chunkObject != null;}}

    //TODO: リファクタリング予定　const化
    static int terrainSizeX = 9;
    static int terrainSizeY = 9;

    //TODO: リファクタリング予定 const化
    public static int sizeX = 5;
    public static int sizeY = 5;

    //TODO: リファクタリング予定 プロパティ化
    public static int chunkSizeX = sizeX * terrainSizeX;
    public static int chunkSizeY = sizeY * terrainSizeY;

    //TODO: リファクタリング予定　const化
    static int rockSize = 1;
    static int tallRockSize = 1;
    static int bigRockSize = 3;
    static int treeSize = 3;

    public Chunk(int terrainsTypeCount, 
                 int rockTypeCount,
                 int rockCount,
                 int tallRockTypeCount,
                 int tallRockCount, 
                 int bigRockTypeCount, 
                 int bigRockCount, 
                 int treeTypeCount, 
                 int treeCount)
    {
        terrains = new int[sizeX,sizeY];

        rocks = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];
        tallRocks = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];
        bigRocks = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];
        trees = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];

        exists = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];

        //terrains
        for (int y = 0; y < sizeY; y++) {
            for (int x = 0; x < sizeX; x++) {
                int types = UnityEngine.Random.Range(0, terrainsTypeCount) + 1;
                terrains[x, y] = types;
            }
        }

        //tree
        createPositions(trees, treeCount, treeTypeCount, treeSize);

        //bigRock
        createPositions(bigRocks, bigRockCount, bigRockTypeCount, bigRockSize);

        //tallRock
        createPositions(tallRocks, tallRockCount, tallRockTypeCount, tallRockSize);

        //rock
        createPositions(rocks, rockCount, rockTypeCount, rockSize);
    }

    void createPositions(int [,] positions, int count, int typeCount, int size)
    {
        int halfSize = size / 2;
        for (int i = 0; i < count; i++) {
            for (int j = 0; j < 10; j++) {
                int x = UnityEngine.Random.Range(0, positions.GetLength(1) - (halfSize * 2)) + halfSize;
                int y = UnityEngine.Random.Range(0, positions.GetLength(0) - (halfSize * 2)) + halfSize;
                int type = UnityEngine.Random.Range(0, typeCount) + 1;

                if (!IsExists(x, y, size)) {
                    SetExists(x, y, size);
                    positions[x, y] = type;
                    break;
                }
            }
        }
    }

    bool IsExists(int x, int y, int size)
    {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                int wx = x - (size / 2) + j;
                int wy = y - (size / 2) + i;
                if (exists[wx, wy] != 0) {
                   return true;
                }
            }
        }
        return false;
    }

    void SetExists(int x, int y, int size)
    {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                int wx = x - (size / 2) + j;
                int wy = y - (size / 2) + i;
                exists[wx, wy] = 1;
            }
        }
    }
}
