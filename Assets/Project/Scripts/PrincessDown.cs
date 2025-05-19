using UnityEngine;
using DG.Tweening;

public class PrincessDown : MonoBehaviour
{
    public Transform princess;
    float delayTime = 1.3f;

    void Start()
    {
        princess = GameObject.Find("Princess").transform;
        LeftDown();
    }

    public void LeftDown()
    {
        transform.localScale = new Vector3(1, 1, 1);
        var seq = DOTween.Sequence();
        seq
        .Append(transform.DOMove(new Vector2(princess.transform.position.x - 1, princess.transform.position.y), delayTime))
        .Join(transform.DORotate(new Vector3(0, 0, 90), delayTime, RotateMode.FastBeyond360));
    }
    public void RigthDown()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        var seq = DOTween.Sequence();
        seq
        .Append(transform.DOMove(new Vector2(princess.transform.position.x + 1, princess.transform.position.y),delayTime))
        .Join(transform.DORotate(new Vector3(0, 0, -90), delayTime));
    }
}
