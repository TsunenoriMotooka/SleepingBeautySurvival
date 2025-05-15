using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearKeyUIController : MonoBehaviour
{
    public static HealthUIController instance{get; private set;}
    TextMeshProUGUI clearKeyCount;
    
    void Awake()
    {
        clearKeyCount = GetComponent<TextMeshProUGUI>();
        clearKeyCount.text = $"{ClearKeyManager.GetInstance().Count}";
        ClearKeyManager.GetInstance().AddListener((count)=>{
            clearKeyCount.text = $"{count}";
        });
    }
}
