using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameDirector : MonoBehaviour
{
    public Transform princess;
    public AudioGenerator audioGenerator;
    public DayNightSystem2D dayNightSystem2D;
    public FieldsGenerator fieldGenerator;

    PrincessController princessController;

    enum GameStatus
    {
        Init,
        Playing,
        GameOver,
        GameClear
    }

    GameStatus status = GameStatus.Init;

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

        princessController = princess.GetComponent<PrincessController>();

        // ステータスをゲーム中に変更
        status = GameStatus.Playing;
    }

    void Update()
    {
        // フィールド・モンスターの処理
        fieldGenerator.Update();

        //ゲームオーバーの判定
        if (status == GameStatus.Playing && princessController.health <= 0)
        {
            status = GameStatus.GameOver;

            Invoke("GameStop", 2.0f);
            Invoke("LoadGameOverScene", 4.0f);
        }

        //ゲームクリアーの判定
        if (status == GameStatus.Playing && ClearKeyManager.GetInstance().Count <= 0)
        {
            status = GameStatus.GameClear;
            
            Invoke("GameStop", 2.0f);
            Invoke("LoadGameClearScene", 4.0f);
        }
    }

    void GameStop()
    {
        audioGenerator.FadeOutBGM();
    }

    void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    void LoadGameClearScene()
    {
        SceneManager.LoadScene("GameClearScene");
    }
}
