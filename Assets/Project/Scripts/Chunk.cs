using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public int[,] terrains{get;}
    public int[,] rock1s{get;}
    public int[,] rock2s{get;}
    public int[,] rock3s{get;}    
    public int[,] trees{get;}

    int[,] exists;

    int sizeX = 5;
    int sizeY = 5;
    int terrainSizeX = 10;
    int terrainSizeY = 10;
    int rock1Size = 1;
    int rock2Size = 1;
    int rock3Size = 3;
    int treeSize = 3;

    public Chunk(int terrainsTypeCount, 
                 int rock1TypeCount,
                 int rock1Count,
                 int rock2TypeCount,
                 int rock2Count, 
                 int rock3TypeCount, 
                 int rock3Count, 
                 int treeTypeCount, 
                 int treeCount)
    {
        exists = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];

        //terrains
        terrains = new int[sizeX,sizeY];
        for (int y = 0; y < sizeY; y++) {
            for (int x = 0; x < sizeX; x++) {
                int types = Random.Range(0, terrainsTypeCount) + 1;
                terrains[x, y] = types;   
            }
        }

        //rock3
        rock3s = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];
        for (int i = 0; i < rock3Count; i++) {
            int x = Random.Range(0, sizeX * terrainSizeX - 2) + 1;
            int y = Random.Range(0, sizeY * terrainSizeY - 2) + 1;
            int type = Random.Range(0, rock3TypeCount) + 1;

            if (IsExists(x, y, rock3Size)) continue;
            SetExists(x, y, rock3Size);
            rock3s[x, y] = type;
        } 

        //tree
        trees = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];
        for (int i = 0; i < treeCount; i++) {
            int x = Random.Range(0, sizeX * terrainSizeX - 2) + 1;
            int y = Random.Range(0, sizeY * terrainSizeY - 2) + 1;
            int type = Random.Range(0, treeTypeCount) + 1;

            if (IsExists(x, y, treeSize)) continue;
            SetExists(x, y, treeSize);
            trees[x, y] = type;
        }

        //rock2
        rock2s = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];
        for (int i = 0; i < rock2Count; i++) {
            int x = Random.Range(0, sizeX * terrainSizeX);
            int y = Random.Range(0, sizeY * terrainSizeY);
            int type = Random.Range(0, rock2TypeCount) + 1;

            if (IsExists(x, y, rock2Size)) continue;
            SetExists(x, y, rock2Size);
            rock2s[x, y] = type;
        } 

        //rock1
        rock1s = new int[sizeX * terrainSizeX, sizeY * terrainSizeY];
        for (int i = 0; i < rock1Count; i++) {
            int x = Random.Range(0, sizeX * terrainSizeX);
            int y = Random.Range(0, sizeY * terrainSizeY);
            int type = Random.Range(0, rock1TypeCount) + 1;

            if (IsExists(x, y, rock1Size)) continue;
            SetExists(x, y, rock1Size);
            rock1s[x, y] = type;
        }
    }

    bool IsExists(int x, int y, int size)
    {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                int _x = x - 1 + size + i;
                int _y = y - 1 + size + j;
                Debug.Log($"{_x},{_y}");
                //if (exists[x - 1 + size + i, y - 1 + size + j] != 0) {
                //    return true;
                //}
            }
        }
        return false;
    }

    void SetExists(int x, int y, int size)
    {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                exists[x - 1 + size + i, y - 1 + size + j] = 1;
            }
        }
    }
}
