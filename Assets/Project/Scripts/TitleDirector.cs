using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleDirector : MonoBehaviour
{
    public AudioGenerator audioGenerator;
    public Button startButton;
    public Button HowToButton;

    void Start()
    {
        Debug.Log($"{startButton}");
        startButton.onClick.AddListener(()=>{
            startButton.enabled = false;
            HowToButton.enabled = false;

            audioGenerator.PlaySE(SE.SelectStart);
            Invoke("LoadGameScene", 4.0f);
        });

        audioGenerator.PlayBGM(BGM.TitleScene);
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    void LoadHowToScene()
    {
        //TODO:
    }
}
