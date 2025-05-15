using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public Transform princess;
    public AudioGenerator audioGenerator;
    public DayNightSystem2D dayNightSystem2D;
    public FieldGenerator fieldGenerator;

    void Start()
    {
        // Frame Rate を 60fps に設定        
        Application.targetFrameRate = 60;

        // フィールド生成の初期化
        fieldGenerator.audioGenerator = audioGenerator;
        fieldGenerator.princess = princess;
        fieldGenerator.dayNightSystem2D = dayNightSystem2D;
        fieldGenerator.Start();

        // BGMの再生
        audioGenerator.PlayBGM(BGM.GameScene);
    }

    void Update()
    {
        fieldGenerator.Update();
    }
}
