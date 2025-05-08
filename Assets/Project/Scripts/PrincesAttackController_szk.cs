using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PrincesAttackController_szk : MonoBehaviour
{
    public float speed = 10f;

    public void Attack(Vector3 direction){
        transform.DOMove(transform.position + direction * speed ,1f)
        .SetEase(Ease.OutQuad).OnComplete(()=>Destroy(gameObject));
    }

}
