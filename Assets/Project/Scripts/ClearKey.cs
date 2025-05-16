using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearKey : MonoBehaviour
{
    public Action destoryDelegate { get; set; }
    public AudioGenerator audioGenerator;

    public bool isDetected = false;

    void OnDestroy()
    {
        if (destoryDelegate != null)
        {
            destoryDelegate();
        }
        destoryDelegate = null;
    }

    public void Detect()
    {
        if (!isDetected)
        {
            isDetected = true;
            audioGenerator.PlaySE(SE.DetectClearKey);
        }
    }

    public void Lost()
    {
        isDetected = false;
    }
}
