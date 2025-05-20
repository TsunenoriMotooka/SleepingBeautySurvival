using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameSceneDirector : MonoBehaviour
{
    public Transform princess;
    public AudioGenerator audioGenerator;
    public DayNightSystem2D dayNightSystem2D;
    public FieldsGenerator fieldsGenerator;

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
        fieldsGenerator.audioGenerator = audioGenerator;
        fieldsGenerator.princess = princess;
        fieldsGenerator.dayNightSystem2D = dayNightSystem2D;
        fieldsGenerator.Start();

        // BGMの再生
        audioGenerator.PlayBGM(BGM.GameScene);

        princessController = princess.GetComponent<PrincessController>();

        // ステータスをゲーム中に変更
        status = GameStatus.Playing;
    }

    void Update()
    {
        // フィールド・モンスターの処理
        fieldsGenerator.Update();

        //ゲームオーバーの判定
        if (status == GameStatus.Playing && princessController.health <= 0)
        {
            status = GameStatus.GameOver;

            DOTween.Sequence()
                .AppendCallback(() =>
                {
                })
                .AppendInterval(2.0f)
                .AppendCallback(() =>
                {
                    fieldsGenerator.StopEnemyBullets();
                    fieldsGenerator.StopEnemies();
                    audioGenerator.FadeOutBGM();
                })
                .AppendInterval(2.0f)
                .AppendCallback(() =>
                {
                    SceneManager.LoadScene("GameOverScene");
                });
        }

        //ゲームクリアーの判定
        if (status == GameStatus.Playing && ClearKeyManager.GetInstance().Count <= 0)
        {
            status = GameStatus.GameClear;

            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    fieldsGenerator.StopEnemies();
                    fieldsGenerator.StopEnemyBullets();
                })
                .AppendInterval(2.0f)
                .AppendCallback(() =>
                {
                    audioGenerator.FadeOutBGM();
                })
                .AppendInterval(2.0f)
                .AppendCallback(() =>
                {
                    SceneManager.LoadScene("GameClearScene");
                });
        }
    }
}
