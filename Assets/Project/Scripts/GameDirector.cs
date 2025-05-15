using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public AudioGenerator audioGenerator;

    void Start()
    {        
        Application.targetFrameRate = 60;
        
        audioGenerator.PlayBGM(BGM.GameScene);
    }

    void Update()
    {
        
    }
}
