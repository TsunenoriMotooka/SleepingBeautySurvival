using DG.Tweening;
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

            StartGameScene();
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

    void StartGameScene()
    {
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                audioGenerator.PlaySE(SE.SelectStart);
            })
            .AppendInterval(2.0f)
            .AppendCallback(() =>
            {
                audioGenerator.FadeOutBGM();
            })
            .AppendInterval(2.5f)
            .AppendCallback(() =>
            {
                SceneManager.LoadScene("GameScene");
            });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && startButton.enabled)
        {
            FlashController flashController = startButton.GetComponent<FlashController>();
            flashController.FlashStart();
             
            startButton.enabled = false;
            howToButton.enabled = false;
            StartGameScene();
        }
    }
}
