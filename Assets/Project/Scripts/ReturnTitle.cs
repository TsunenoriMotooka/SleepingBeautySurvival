using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class ReturnTitle : MonoBehaviour
{
    public Button button; //Asetするボタン
    public float delayTime; //遅延時間
    CanvasGroup canvasGroup; //透明度管理用

    void Start()
    {
        button.gameObject.SetActive(false); //ボタンの非表示
        // canvasGroupの取得または追加
        canvasGroup = button.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = button.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        button.interactable = false;

        StartCoroutine(ShowButton());
    }

    IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(delayTime); //delay時間待つ
        button.gameObject.SetActive(true);

        // DOTweenでフェードイン
        canvasGroup.DOFade(1f, 0.5f).OnComplete(() => button.interactable = true);
    }

    public void OnReturnButtonClicked()
    {
        ReturnTitleScene();
    }
    // Update is called once per frame
    void ReturnTitleScene()
    {
        SceneManager.LoadScene("TitleScene");   
    }
}
