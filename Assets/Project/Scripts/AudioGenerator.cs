using DG.Tweening;
using UnityEngine;

public class AudioGenerator : MonoBehaviour
{
    public GameObject[] bgmPrefabs;
    public GameObject[] sePrefabs;

    static AudioSource bgmAudioSource;

    [SerializeField]
    float fadeOutDuration = 2.0f;

    public void PlaySE(int index, Transform target)
    {
        if (index < 0 || index >= sePrefabs.Length) return;

        GameObject es = Instantiate(sePrefabs[index]);
        if (es != null && target != null)
        {
            Vector2 fromPoint = Camera.main.ViewportToWorldPoint(Vector2.zero);
            Vector2 toPoint = Camera.main.ViewportToWorldPoint(Vector2.one);

            float width = toPoint.x - fromPoint.x;
            float panStereo = Mathf.Clamp((target.position.x - fromPoint.x - (width / 2)) / (width * 0.8f / 2)  , -1f, 1f);

            es.GetComponent<AudioSource>().panStereo = panStereo;
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
        PlayBGM(index, 0f);
    }

    public void PlayBGM(int index, float duration)
    {
        if (index < 0 || index >= bgmPrefabs.Length) return;

        var sequence = DOTween.Sequence();
        if (bgmAudioSource != null && bgmAudioSource.isPlaying) {
            sequence.Append(bgmAudioSource.DOFade(0, fadeOutDuration));
        }
        sequence.AppendInterval(duration);
        sequence.OnComplete(()=>{
            GameObject bgm = Instantiate(bgmPrefabs[index]);
            if (bgm != null) {
                bgmAudioSource = bgm.GetComponent<AudioSource>();
            }            
        });
    }

    public void PlayBGM(BGM bgm)
    {
        PlayBGM(bgm, 0f);
    }

    public void PlayBGM(BGM bgm, float duration)
    {
        int index = FoundBGMIndex(bgm);
        PlayBGM(index, duration);
    }

    public void StopBGM(bool fadeOut)
    {
        if (bgmAudioSource != null) {
            bgmAudioSource.Stop();
            bgmAudioSource = null;
        }
    }

    public void FadeOutBGM()
    {
        if (bgmAudioSource != null) {
            bgmAudioSource.DOFade(0, fadeOutDuration);
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
        SE[] enums = new SE[]{SE.DamagePrincess1, SE.DamagePrincess2};
        int index = Random.Range(0, enums.Length);
        PlaySE(enums[index]);
    }
}
