using UnityEngine;

public class Chunk
{
    public int[,] terrains{get;}

    public int[,] trees{get;}
    public int[,] rocks{get;}
    public int[,] tallRocks{get;}
    public int[,] bigRocks{get;}    

    public int[,] exists{get;}
    System.Random rand = new System.Random();

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
        terrains = new int[Const.chunkLengthX, Const.chunkLengthY];

        rocks = new int[Const.chunkSizeX, Const.chunkSizeY];
        tallRocks = new int[Const.chunkSizeX, Const.chunkSizeY];
        bigRocks = new int[Const.chunkSizeX, Const.chunkSizeY];
        trees = new int[Const.chunkSizeX, Const.chunkSizeY];

        exists = new int[Const.chunkSizeX, Const.chunkSizeY];

        //terrains
        createTerrains(terrains, terrainsTypeCount);

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
        for (int y = 0; y < Const.chunkLengthY; y++) {
            for (int x = 0; x < Const.chunkLengthX; x++) {
                int types = rand.Next(terrainsTypeCount) + 1;
                terrains[x, y] = types;
            }
        }
    }

    void createObjects(int [,] objects, int count, int typeCount, int size)
    {
        int halfSize = size / 2;
        for (int i = 0; i < count; i++) {
            for (int j = 0; j < 10; j++) {
                int x = rand.Next(objects.GetLength(1) - (halfSize * 2)) + halfSize;
                int y = rand.Next(objects.GetLength(0) - (halfSize * 2)) + halfSize;
                int type = rand.Next(typeCount) + 1;

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
