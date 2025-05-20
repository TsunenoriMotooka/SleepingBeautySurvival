using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameOverSceneDirector : MonoBehaviour
{
    public AudioGenerator audioGenerator; // 音楽再生
    public Image fadeScreen; // 暗転用の黒い画像（UI）
    public GameObject princessImage; // 落ちてくるプリンセス画像
    public GameObject risingImage; // 下からせり上がる画像
    public GameObject roseImage;

    public Color night;
    public Color midnight;

    public float fadeDuration = 1f; // 暗転時間
    public float fallDuration = 3f; // プリンセス画像の落下時間
    public float riseDuration = 5f; // せり上がる画像の上昇時間
    public float fallHeight = 669.38f; // 落ちる距離
    public float riseHeight = 936.9f; // せり上がる距離

    public Button button;
    public float delayTime;

    void Start()
    {
        roseImage.gameObject.SetActive(false);
        audioGenerator.PlayBGM(BGM.GameOver);
        StartEffect();
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
        // 画面暗転
        fadeScreen.DOFade(1, fadeDuration).SetEase(Ease.OutQuad);

        // プリンセス画像を画面上から3秒かけて落とす
        princessImage.GetComponent<Image>().color = night;
        DOTween.Sequence()
            .Append(princessImage.transform.DOMoveY(princessImage.transform.position.y - fallHeight, fallDuration))
            .Join(princessImage.GetComponent<Image>().DOColor(midnight, fallDuration))
            .SetEase(Ease.InQuad)
            .AppendCallback(() =>
            {
                Destroy(princessImage);
                roseImage.gameObject.SetActive(true);
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                Destroy(roseImage);
            })
            .AppendInterval(1.5f)
            .OnComplete(() =>
            {
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

    void RemoveTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && button.enabled)
        {
            button.enabled = false;
            ReturnTitleScene();
        }
    }
}
