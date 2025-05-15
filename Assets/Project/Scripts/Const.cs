using System;
using System.Reflection;
using UnityEngine;

public class Const : MonoBehaviour
{
    public static int terrainSizeX = 9;
    public static int terrainSizeY = 9;

    public static int chunkMatrixX = 3;
    public static int chunkMatrixY = 3;

    public static int chunkSizeX = terrainSizeX * chunkMatrixX;
    public static int chunkSizeY = terrainSizeY * chunkMatrixY;

    public static int fieldMatrixX = 5;
    public static int fieldMatrixY = 5;

    public static int fieldSizeX = chunkSizeX * fieldMatrixX;
    public static int fieldSizeY = chunkSizeY * fieldMatrixY;

    public static int rockSize = 1;
    public static int tallRockSize = 1;
    public static int bigRockSize = 3;
    public static int treeSize = 3;
    public static int lightSize = 1;

    public static int[,] firstSpaces = {
        {0,0,0,1,1,1,0,0,0},
        {0,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,0},
        {1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1},
        {0,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,0},
        {0,0,0,1,1,1,0,0,0},
        };

    public static int [,] firstLights = {
        {0,0,0,1,0,0,0},
        {0,1,0,0,0,1,0},
        {0,0,0,0,0,0,0},
        {1,0,0,0,0,0,1},
        {0,0,0,0,0,0,0},
        {0,1,0,0,0,1,0},
        {0,0,0,1,0,0,0}
    };

    public static int clearKeyCount = 10;

    public static (int, int, int, int, int)[] chunkTypes = {
        //木  小石 中石 大石 街灯
        ( 40,  10,  4,  2,  4), //森
        ( 40,  20,  6,  4,  4), //森
        ( 20,  30,  6,  4,  4), //林
        (  6,  40,  4,  2,  2), //草原
        (  6,  60, 10,  4,  2), //草原
        (  2,  60, 24, 12,  1), //岩場
    };

    public static String sePrefix = "SE";
    public static String bgmPrefix = "BGM";
}
