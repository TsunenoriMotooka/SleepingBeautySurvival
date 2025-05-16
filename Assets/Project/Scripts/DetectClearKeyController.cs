using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClearKeyController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ClearKey"))
        {
            other.gameObject.GetComponent<ClearKey>().Detect();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ClearKey"))
        {
            other.gameObject.GetComponent<ClearKey>().Lost();
        }            
    }
}
