using UnityEngine;
using System;
using System.Collections.Generic;

public class ExistPositionManager
{
    Dictionary<(float, float), int> map;

    private static ExistPositionManager _instance = new ExistPositionManager();
    public static ExistPositionManager GetInstance(){
        return _instance;
    }

    public ExistPositionManager()
    {
        map  = new Dictionary<(float, float), int>();

        for (int i = 0; i < Const.firstSpace.GetLength(0); i++) {
            for (int j = 0; j < Const.firstSpace.GetLength(1); j++) {
                int x = j - Const.firstSpace.GetLength(1) / 2;
                int y = i - Const.firstSpace.GetLength(0) / 2;
                if (Const.firstSpace[i, j] != 0) {
                    map[(x, y)] = 1;
                } 
            }
        }
    }

    public bool Contains(float px, float py)
    {
        int x = (int)Math.Round(px);
        int y = (int)Math.Round(py);
        return Contains(x, y);
    }
    public bool Contains(int px, int py)
    {
        int wx = (Const.fieldSizeX + (px + Const.fieldSizeX/2) % Const.fieldSizeX) % Const.fieldSizeX - Const.fieldSizeX / 2;
        int wy = (Const.fieldSizeY + (py + Const.fieldSizeY/2) % Const.fieldSizeY) % Const.fieldSizeY - Const.fieldSizeY / 2;
        return map.ContainsKey((wx, wy));
    }
    public bool Contains(Vector2 position)
    {
        return Contains(position.x, position.y);
    }

    public bool Contains(Vector3 position)
    {
        return Contains(position.x, position.y);
    }

    public void Put(float px, float py)
    {
        int x = (int)Math.Round(px);
        int y = (int)Math.Round(py);
        Put(x, y);
    }
    public void Put(int px, int py)
    {
        Vector2 fp = Utils.ToFieldPosition(px, py); 
        if (Contains(fp.x, fp.y)) return;

        map[(fp.x, fp.y)] = 1;
    }
    public void Put(Vector2 position)
    {
        Put(position.x, position.y);
    }
    public void Put(Vector3 position)
    {
        Put(position.x, position.y);
    }

    public void Remove(float px, float py)
    {
        int x = (int)Math.Round(px);
        int y = (int)Math.Round(py);
        Remove(x, y);
    }

    public void Remove(int px, int py)
    {
        Vector2 fp = Utils.ToFieldPosition(px, py);
        if (!Contains(fp.x, fp.y)) return;

        map.Remove((fp.x, fp.y));
    }
    public void Remove(Vector2 position)
    {
        Remove(position.x, position.y);
    }
    public void Remove(Vector3 position)
    {
        Remove(position.x, position.y);
    }
}
