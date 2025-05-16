using UnityEngine;
using UnityEngine.UI;

public class HowToDialog : MonoBehaviour
{
    public GameObject howToPanel;

    void Start()
    {
        howToPanel.SetActive(false);
    }

    public void ShowHowTo()
    {
        howToPanel.SetActive(true);
    }

    public void HideHowTo()
    {
        howToPanel.SetActive(false);
    }
}
