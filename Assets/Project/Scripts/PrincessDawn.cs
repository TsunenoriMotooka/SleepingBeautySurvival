using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PrincessDawn : MonoBehaviour
{
    float lastX;
    public Transform princess;
    float delayTime = 1.3f;
    Animator anim;

    void Start()
    {
        princess = GameObject.Find("Princess").transform;
    }

    void LeftDown()
    {
        transform.localScale = new Vector3(1, 1, 1);
        var seq = DOTween.Sequence();
        seq
        .Append(transform.DOMove(new Vector2(princess.transform.position.x - 1, princess.transform.position.y), delayTime))
        .Join(transform.DORotate(new Vector3(0, 0, 90), delayTime, RotateMode.FastBeyond360));
    }
    void RigthDown()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        var seq = DOTween.Sequence();
        seq
        .Append(transform.DOMove(new Vector2(princess.transform.position.x + 1, princess.transform.position.y),delayTime))
        .Join(transform.DORotate(new Vector3(0, 0, -90), delayTime));
    }
    // Update is called once per frame
    void Update()
    {
    
    }
}
