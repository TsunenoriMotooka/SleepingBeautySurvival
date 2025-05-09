using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class FiledsManager : MonoBehaviour
{
    private static FiledsManager _instance = new FiledsManager();
    public static FiledsManager GetInstance(){
        return _instance;
    }

    Dictionary<(int,int), int> objects = Dictionary<(int,int), int>();

    public bool ContainsKey(int x, int y)
    {
        return objects.ContainsKey((x, y));
    }

    public bool CanStay()
    {
        
    }

}
