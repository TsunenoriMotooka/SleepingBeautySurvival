using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI_Controller : MonoBehaviour
{
    public static HealthUI_Controller instance{get; private set;}
    public Image mask;
    float originalSize;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value){
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,originalSize * value);
    }

}
