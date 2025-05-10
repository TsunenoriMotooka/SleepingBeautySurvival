using System.Reflection;
using UnityEngine;

public class Const : MonoBehaviour
{
    public static int terrainSizeX = 9;
    public static int terrainSizeY = 9;

    public static int chunkMatrixX = 5;
    public static int chunkMatrixY = 5;

    public static int chunkSizeX = terrainSizeX * chunkMatrixX;
    public static int chunkSizeY = terrainSizeY * chunkMatrixY;

    public static int fieldMatrixX = 9;
    public static int fieldMatrixY = 9;

    public static int fieldSizeX = chunkSizeX * fieldMatrixX;
    public static int fieldSizeY = chunkSizeY * fieldMatrixY;

    public static int rockSize = 1;
    public static int tallRockSize = 1;
    public static int bigRockSize = 3;
    public static int treeSize = 3;

    public static int[,] firstSpace = {
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

    public static int clearKeyCount = 10;

    public static (int, int, int, int)[] chunkTypes = {
        //木  小石 中石 大石 
        (100,  30,  5,  3), //森
        ( 90,  60, 12,  8), //森
        ( 80,  90, 20, 13), //森
        ( 50,  50,  5,  3), //林
        ( 50,  50, 10,  5), //林
        ( 20,  40,  8,  6), //草原
        ( 20,  80, 16,  8), //草原
        ( 10, 200, 60, 30), //岩場
    };
}
