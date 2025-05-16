using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameOverSceneController : MonoBehaviour
{
    public AudioGenerator audioGenerator; // 音楽再生
    public Image fadeScreen; // 暗転用の黒い画像（UI）
    public GameObject princessImage; // 落ちてくるプリンセス画像
    public GameObject risingImage; // 下からせり上がる画像
    public GameObject rose;

    public Color night;
    public Color midnight;

    public float fadeDuration = 1f; // 暗転時間
    public float fallDuration = 2f; // プリンセス画像の落下時間
    public float riseDuration = 5f; // せり上がる画像の上昇時間
    public float fallHeight = 669.38f; // 落ちる距離
    public float riseHeight = 936.9f; // せり上がる距離

    public Button button;
    public float delayTime;
    CanvasGroup canvasGroup;

    void Start()
    {
        rose.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = button.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        button.interactable = false;
        StartEffect();
        StartCoroutine(ShowButton());

        audioGenerator.PlayBGM(BGM.GameOver);
    }
    
    IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(delayTime);
        button.gameObject.SetActive(true);

        canvasGroup.DOFade(1f, 0.5f).OnComplete(() => button.interactable = true);
    }

    public void OnReturnButtonClicked()
    {
        ReturnTitleScene();   
    }

    void ReturnTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void StartEffect()
    {
        // 画面暗転
        fadeScreen.DOFade(1, fadeDuration).SetEase(Ease.OutQuad);

        // プリンセス画像を画面上から3秒かけて落とす
        princessImage.GetComponent<Image>().color = night;
        var sequence = DOTween.Sequence();
        sequence
            .Append(princessImage.transform.DOMoveY(princessImage.transform.position.y - fallHeight, fallDuration))
            .Join(princessImage.GetComponent<Image>().DOColor(midnight, fallDuration))
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                StartPrincessAnimation();
            });
    }

    private void StartPrincessAnimation()
    {
        Destroy(princessImage);
        rose.gameObject.SetActive(true);
        DOVirtual.DelayedCall(1f, () =>
        {
            Destroy(rose);
            StartRisingImage();
        });
    }

    private void StartRisingImage()
    {
        // 画像を3秒かけて下からせり上げる
        var sequence = DOTween.Sequence();
        sequence
            .Append(risingImage.transform.DOMoveY(risingImage.transform.position.y + riseHeight, riseDuration))
            .Join(princessImage.GetComponent<Image>().DOColor(Color.white, 0.2f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                DOVirtual.DelayedCall(20f,() => {
                    RemoveTitle();
                });
            });
    }

    void RemoveTitle(){
        SceneManager.LoadScene("TitleScene");
    }
}
