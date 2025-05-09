using System;
using UnityEngine;

public class Const : MonoBehaviour
{
    public static int terrainSizeX = 9;
    public static int terrainSizeY = 9;

    public static int chunkLengthX = 5;
    public static int chunkLengthY = 5;

    public static int chunkSizeX = terrainSizeX * chunkLengthX;
    public static int chunkSizeY = terrainSizeY * chunkLengthY;

    public static int fieldLengthX = 9;
    public static int fieldLengthY = 9;

    public static int fieldSizeX = chunkSizeX * fieldLengthX;
    public static int fieldSizeY = chunkSizeY * fieldLengthY;

    public static int rockSize = 1;
    public static int tallRockSize = 1;
    public static int bigRockSize = 3;
    public static int treeSize = 3;
}
