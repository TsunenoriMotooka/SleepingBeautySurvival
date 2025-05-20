using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class GameClearScene : MonoBehaviour
{
    public AudioGenerator audioGenerator;

    public GameObject mainImage; //近づく画像
    public Image overlayImage; //浮かび上がる画像
    public float zoomDuration; //拡大する時間
    public float fadeDuration; //浮かび上がる時間
    public float targetScale; //どれくらい拡大するか

    public Button button;
    public float delayTime;
    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        Color c = overlayImage.color;
        c.a = 0f;
        overlayImage.color = c;
        

        button.gameObject.SetActive(false);
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = button.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        button.interactable = false;
        DOVirtual.DelayedCall(2f, () =>
        {
            StartEffect();
            StartCoroutine(ShowButton());
        });

        audioGenerator.PlayBGM(BGM.GameClear);
    }

    IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(delayTime);
        button.gameObject.SetActive(true);

        canvasGroup.DOFade(1f, 0.5f).OnComplete(() => button.interactable = true);
    }

    public void OnReturnButtonClicked()
    {
        button.enabled = false;
        ReturnTitleScene();   
    }

    void ReturnTitleScene()
    {
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                audioGenerator.PlaySE(SE.SelectContinue);
            })
            .AppendInterval(2.0f)
            .AppendCallback(() =>
            {
                audioGenerator.FadeOutBGM();
            })
            .AppendInterval(2.0f)
            .AppendCallback(() =>
            {
                SceneManager.LoadScene("TitleScene");
            });
    }

    public void StartEffect()
    {
        var seq = DOTween.Sequence();
        seq
            .Append(mainImage.GetComponent<Image>().DOColor(Color.white,2f).SetEase(Ease.Linear))
            .Append(mainImage.transform.DOScale(targetScale, zoomDuration).SetEase(Ease.OutQuad))
            .Append(mainImage.GetComponent<Image>().DOFade(0, fadeDuration).SetEase(Ease.InQuad))
            .Join(overlayImage.DOFade(1, fadeDuration).SetEase(Ease.OutQuad))
            .OnComplete(() =>
                mainImage.SetActive(false));
    }
    // Update is called once per frame
    void Update()
    {
    
    }
}
