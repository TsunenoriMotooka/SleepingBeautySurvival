using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearKey : MonoBehaviour
{
    public Action destoryDelegate{get;set;}

    void OnDestroy()
    {
        if (destoryDelegate != null) {
            destoryDelegate();
            ClearKeyManager.GetInstance().Found();
        }
        destoryDelegate = null;
    }
}
