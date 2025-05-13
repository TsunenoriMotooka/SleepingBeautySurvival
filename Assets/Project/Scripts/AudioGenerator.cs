using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class AudioGenerator : MonoBehaviour
{
    public GameObject[] bgmPrefabs;
    public GameObject[] sePrefabs;

    Camera mainCamera;
    AudioSource bgmAudioSource;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void PlaySE(int index, Transform target)
    {
        if (index < 0 || index >= sePrefabs.Length) return;
        GameObject es = Instantiate(sePrefabs[index]);
        if (es != null && target != null && mainCamera != null) {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
            es.transform.position = screenPos;
        }
    }

    public void PlaySE(SE se)
    {
        PlaySE(se, null);
    }

    public void PlaySE(SE se, Transform target)
    {
        int index = FoundSEIndex(se);
        PlaySE(index, target);
    }

    int FoundSEIndex(SE se)
    {
        for (int i = 0; i < sePrefabs.Length; i++) {
            if (sePrefabs[i].name.Equals(Const.sePrefix + se.ToString())) {
                return i;
            }
        }
        return -1;
    }

    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmPrefabs.Length) return;

        GameObject bgm = Instantiate(bgmPrefabs[index]);
        if (bgm != null) {
            bgmAudioSource = bgm.GetComponent<AudioSource>();
        }
    }

    public void PlayBGM(BGM bgm)
    {
        int index = FoundBGMIndex(bgm);
        PlayBGM(index);
    }

    public void StopBGM()
    {
        if (bgmAudioSource != null) {
            bgmAudioSource.Stop();
            bgmAudioSource = null;
        }
    }

    int FoundBGMIndex(BGM bgm)
    {
        for (int i = 0; i < bgmPrefabs.Length; i++) {
            if (bgmPrefabs[i].name.Equals(Const.bgmPrefix + bgm.ToString())) {
                return i;
            }
        }
        return -1;
    }

    public void PlaySEDamagePrincess()
    {
        int rand = Random.Range(0,2);
        SE se = rand == 0 ? SE.DamagePrincess1 : SE.DamagePrincess2;
        PlaySE(se);
    }

    public void PlaySEDamageEnemy()
    {
        PlaySE(SE.DamageEnemy);
    }
}
