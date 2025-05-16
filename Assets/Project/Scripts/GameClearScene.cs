using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearScene : MonoBehaviour
{
    public GameObject mainImage; //近づく画像
    public Image overlayImage; //浮かび上がる画像
    public float zoomDuration; //拡大する時間
    public float fadeDuration; //浮かび上がる時間
    public float targetScale; //どれくらい拡大するか
    public float fadeDelay; //フェード開始までの遅延時間

    // Start is called before the first frame update
    void Start()
    {
        Color c = overlayImage.color;
        c.a = 0f;
        overlayImage.color = c;
        StartEffect();
    }

    public void StartEffect(){
        // 近づく画像を徐々に拡大
        mainImage.transform.DOScale(targetScale, zoomDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => FadeOut());

        // 同時に別の画像をフェードイン
        overlayImage.DOFade(1,fadeDuration).SetEase(Ease.OutQuad).SetDelay(fadeDelay);
    }

    void FadeOut(){
        // Aをフェードアウトしながら消す
        mainImage.GetComponent<Image>().DOFade(0,fadeDuration).SetEase(Ease.InQuad)
        .OnComplete(() => mainImage.SetActive(false));
    }
    // Update is called once per frame
    void Update()
    {
    
    }
}
