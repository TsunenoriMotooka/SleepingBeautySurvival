using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Chunk
{
    public int chunkX;
    public int chunkY;
    public int chunkType;

    public int[,] terrains { get; }

    public int[,] rocks{get;}
    public int[,] tallRocks{get;}
    public int[,] bigRocks{get;}    
    public int[,] trees{get;}
    public int[,] lights{get;}

    //偏りがあるシステムの乱数を使用
    System.Random random = new System.Random();

    public Chunk(int chunkX,
                 int chunkY,
                 int chunkType,
                 int terrainsTypeCount,
                 int rockTypeCount,
                 int rockCount,
                 int tallRockTypeCount,
                 int tallRockCount, 
                 int bigRockTypeCount, 
                 int bigRockCount, 
                 int treeTypeCount, 
                 int treeCount,
                 int lightTypeCount,
                 int lightCount)
    {
        this.chunkX = chunkX;
        this.chunkY = chunkY;
        this.chunkType = chunkType;

        terrains = new int[Const.chunkMatrixX, Const.chunkMatrixY];
        
        rocks = new int[Const.chunkSizeX, Const.chunkSizeY];
        tallRocks = new int[Const.chunkSizeX, Const.chunkSizeY];
        bigRocks = new int[Const.chunkSizeX, Const.chunkSizeY];
        trees = new int[Const.chunkSizeX, Const.chunkSizeY];
        lights = new int[Const.chunkSizeX, Const.chunkSizeY]; 

        //terrains
        createTerrains(terrains, terrainsTypeCount);

        //light
        //開始位置に2本立てる
        if (chunkX == 0 && chunkY == 0) {
            for (int i = 0; i < Const.firstLights.GetLength(0); i++) {
                for (int j = 0; j < Const.firstLights.GetLength(1); j++) {
                    if (Const.firstLights[i, j] != 0) {
                        int x = i - Const.firstLights.GetLength(1) / 2;
                        int y = j - Const.firstLights.GetLength(0) / 2;
                        lights[x + Const.chunkSizeX / 2, y + Const.chunkSizeX / 2] = lightTypeCount + 1;
                    }
                }
            }
        }
        createObjects(lights, lightCount, lightTypeCount, Const.lightSize);

        //tree
        createObjects(trees, treeCount, treeTypeCount, Const.treeSize);

        //bigRock
        createObjects(bigRocks, bigRockCount, bigRockTypeCount, Const.bigRockSize);

        //tallRock
        createObjects(tallRocks, tallRockCount, tallRockTypeCount, Const.tallRockSize);

        //rock
        createObjects(rocks, rockCount, rockTypeCount, Const.rockSize);
    }

    void createTerrains(int[,] terrains, int terrainsTypeCount)
    {
        for (int y = 0; y < Const.chunkMatrixY; y++) {
            for (int x = 0; x < Const.chunkMatrixX; x++) {
                int types = random.Next(terrainsTypeCount) + 1;
                terrains[x, y] = types;
            }
        }
    }

    void createObjects(int [,] objects, int count, int typeCount, int size)
    {
        int halfSize = size / 2;
        count = (int)Mathf.Ceil(UnityEngine.Random.Range(count * 0.9f, count * 1.1f));
        for (int i = 0; i < count; i++) {
            int type = random.Next(typeCount) + 1;
            for (int j = 0; j < 20; j++) {
                int x = random.Next(objects.GetLength(1) - (halfSize * 2)) + halfSize;
                int y = random.Next(objects.GetLength(0) - (halfSize * 2)) + halfSize;
                if (!IsExists(x, y, size)) {
                    SetExists(x, y, size);
                    objects[x, y] = type;
                    break;
                }
            }
        }
    }

    bool IsExists(int x, int y, int size)
    {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                int cx = x - Const.chunkSizeX / 2;
                int cy = y - Const.chunkSizeY / 2;
                int px = chunkX * Const.chunkSizeX + cx + j - (size / 2);
                int py = chunkY * Const.chunkSizeY + cy + i - (size / 2);
                bool isExsits = ExistPositionManager.GetInstance().Contains(px, py);
                if (isExsits) {
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
                int cx = x - Const.chunkSizeX / 2;
                int cy = y - Const.chunkSizeY / 2;
                int px = chunkX * Const.chunkSizeX + cx + j - (size / 2);
                int py = chunkY * Const.chunkSizeY + cy + i - (size / 2);

                ExistPositionManager.GetInstance().Put(px, py);
            }
        }
    }
}
