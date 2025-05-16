using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashController : MonoBehaviour
{
    public Image Flash;

    void Start()
    {
        Flash.color = new Color(255, 0, 0, 0);
    }

    public void FlashStart()
    {
        StartCoroutine(FlashScreen());
    }

    IEnumerator FlashScreen()
    {
        float flashSpeed = 0.2f;
        int flashCount = 2;

        for (int i = 0; i < flashCount; i++)
        {
            Flash.color = new Color(255, 0, 0, 1);
            yield return new WaitForSeconds(flashSpeed);

            //少し暗くする
            Flash.color = new Color(255, 0, 0, 0.5f);
            yield return new WaitForSeconds(flashSpeed);
        }

        float fadeDuration = 2f;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            Flash.color = new Color(255, 0, 0, 1 - (t / fadeDuration));
            yield return null;
        }

        Flash.color = new Color(255, 0, 0, 0);

    }

}
