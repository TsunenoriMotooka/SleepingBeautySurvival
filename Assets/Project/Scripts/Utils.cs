using UnityEngine;
using System;

public class Utils
{
    public static (int, int) PositionToChunkMatrix(float px, float py)
    {
        int x = (int)Math.Round(px / (float)Const.chunkSizeX);
        int y = (int)Math.Round(py / (float)Const.chunkSizeY); 
        return (x, y);
    }
    public static (int, int) PositionToChunkMatrix(Vector2 position)
    {
        return PositionToChunkMatrix(position.x, position.y);
    }
    public static (int, int) PositionToChunkMatrix(Vector3 position)
    {
        return PositionToChunkMatrix(position.x, position.y); 
    }
  
    public static Vector2 chunkMatrixToPosition(int chunkX, int chunkY)
    {
        return new Vector2(chunkX * Const.chunkSizeX, chunkY * Const.chunkSizeY);
    }

    public static Vector2 ToFieldPosition(float pxf, float pyf)
    {
        int px = (int)Math.Round(pxf);
        int py = (int)Math.Round(pyf);
        return ToFieldPosition(px, py); 
    }
    public static Vector2 ToFieldPosition(int px, int py)
    {
        float x = (Const.fieldSizeX / 2 + px) % Const.fieldSizeX - (Const.fieldSizeX / 2); 
        float y = (Const.fieldSizeY / 2 + py) % Const.fieldSizeY - (Const.fieldSizeY / 2);
        return new Vector2(x, y);
    }
    public static Vector2 ToFieldPosition(Vector2 position)
    {
        return ToFieldPosition(position.x, position.y); 
    }
    public static Vector2 ToFieldPosition(Vector3 position)
    {
        return ToFieldPosition(position.x, position.y);
    }

    public static int cycle1(int cycle, int cycleSize)
    {
        return (cycleSize + (cycle + cycleSize / 2) % cycleSize) % cycleSize - cycleSize / 2;
    }

    public static int cycle2(int cycle, int cycleSize)
    {
        if (cycle >= 0) {
            return (cycle + cycleSize / 2) / cycleSize;
        } else {
            return (cycle - cycleSize / 2) / cycleSize;
        }
    }
}
