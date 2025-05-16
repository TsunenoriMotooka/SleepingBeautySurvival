using System;
using System.Collections.Generic;
using UnityEngine;

public class ClearKeyManager
{
    static ClearKeyManager _instance = new ClearKeyManager();
    int clearKeyCount = Const.clearKeyCount;
    public int Count{get{return clearKeyCount;}}
    List<Action<int>> actions = new List<Action<int>>();

    public static ClearKeyManager GetInstance()
    {
        return _instance;
    }

    public void AddListener(Action<int> action)
    {
        if (!actions.Contains(action)) {
            actions.Add(action);
        }
    }

    public void RemoveListener(Action<int> action)
    {
        if (actions.Contains(action)) {
            actions.Remove(action);
        }
    }

    public void RemoveAllListeners()
    {
        actions.Clear();
    }

    public void Reset()
    {
        clearKeyCount = Const.clearKeyCount;
    }

    public void Found()
    {
        clearKeyCount = Mathf.Max(0, clearKeyCount - 1);
        foreach(Action<int> action in actions) {
            action(clearKeyCount);
        }
    }
}
