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
        if (destoryDelegate != null) {
            destoryDelegate();
        }
        destoryDelegate = null;
    }
}
