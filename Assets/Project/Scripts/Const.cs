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
}
