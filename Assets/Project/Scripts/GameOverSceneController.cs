using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverSceneController : MonoBehaviour
{
    public Image fadeScreen; // 暗転用の黒い画像（UI）
    public GameObject princessImage; // 落ちてくるプリンセス画像
    public GameObject risingImage; // 下からせり上がる画像
    
    public float fadeDuration = 1f; // 暗転時間
    public float fallDuration = 2f; // プリンセス画像の落下時間
    public float riseDuration = 3f; // せり上がる画像の上昇時間
    public float fallHeight = 669.38f; // 落ちる距離
    public float riseHeight = 936.9f; // せり上がる距離

    void Start()
    {
        StartEffect();
    }

    public void StartEffect()
    {
        // 画面暗転
        fadeScreen.DOFade(1, fadeDuration).SetEase(Ease.OutQuad);

        // プリンセス画像を画面上から3秒かけて落とす
        princessImage.transform.DOMoveY(princessImage.transform.position.y - fallHeight, fallDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => StartPrincessAnimation());
    }

    private void StartPrincessAnimation()
    {
        Destroy(princessImage,1f);
        StartRisingImage();
    }

    private void StartRisingImage()
    {
        // 画像を3秒かけて下からせり上げる
        risingImage.transform.DOMoveY(risingImage.transform.position.y + riseHeight, riseDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                DOVirtual.DelayedCall(2f,() => {
                    RemoveTitle();
                });
            });
    }

    void RemoveTitle(){
        SceneManager.LoadScene("TitleScene");
    }
}


