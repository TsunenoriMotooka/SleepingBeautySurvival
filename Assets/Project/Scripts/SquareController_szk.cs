using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting.FullSerializer;

public class SquareController_szk : MonoBehaviour
{
    [SerializeField] float radias = 2f;
    [SerializeField] float duration = 2f;

    void Start()
    {
        transform.DORotate(
            new Vector3(0,0,360),duration,RotateMode.FastBeyond360)
            .SetLoops(-1,LoopType.Restart).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
