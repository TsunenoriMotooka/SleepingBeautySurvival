using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearKey : MonoBehaviour
{
    public Action destoryDelegate{get;set;}
    public AudioGenerator audioGenerator;

    void OnDestroy()
    {
        Debug.Log($"destory key {destoryDelegate}");
        if (destoryDelegate != null) {
            destoryDelegate();
            ClearKeyManager.GetInstance().Found();
            audioGenerator.PlaySE(SE.GetClearKey);
        }
        destoryDelegate = null;
    }
}
