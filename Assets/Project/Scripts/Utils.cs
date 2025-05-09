using System;

public class Utils
{
    public static int GetChunkX(float x)
    {
        return (int)Math.Round(x / (float)Const.chunkSizeX);
    }

    public static int GetChunkY(float y)
    {
        return (int)Math.Round(y / (float)Const.chunkSizeY);
    }
}
