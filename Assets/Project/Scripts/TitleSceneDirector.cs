using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneDirector : MonoBehaviour
{
    public AudioGenerator audioGenerator;
    public Button startButton;
    public Button howToButton;
    public GameObject howToPanel;
    public Button closeButton;

    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            startButton.enabled = false;
            howToButton.enabled = false;
            closeButton.enabled = false;

            audioGenerator.PlaySE(SE.SelectStart);
            Invoke("FadeOutBGM", 2.0f);
            Invoke("LoadGameScene", 4.0f);
        });

        howToButton.onClick.AddListener(() =>
        {
            startButton.enabled = false;
            howToButton.enabled = false;
            howToPanel.SetActive(true);
            audioGenerator.PlaySE(SE.SelectHowto);
        });

        closeButton.onClick.AddListener(() =>
        {
            howToPanel.SetActive(false);
            startButton.enabled = true;
            howToButton.enabled = true;
            audioGenerator.PlaySE(SE.SelectHowto);
        });

        howToPanel.SetActive(false);

        audioGenerator.PlayBGM(BGM.TitleScene);
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    void FadeOutBGM()
    {
        audioGenerator.FadeOutBGM();
    }

}
