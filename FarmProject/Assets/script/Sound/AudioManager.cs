using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<AudioClipData> sfxList;

    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int sfxPoolSize = 5; // 동시에 재생할 수 있는 SFX 수

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // SFX Pool 생성
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sfxSources.Add(src);
        }
    }

    // SFX 재생
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxList.Count) return;

        AudioClipData data = sfxList[index];
        AudioSource src = GetAvailableSFXSource();
        if (src == null) return; // 풀 부족

        src.clip = data.clip;
        src.loop = data.isLoop;
        src.volume = data.volume;
        src.pitch = data.useRandomPitch
            ? Random.Range(data.minPitch, data.maxPitch)
            : data.pitch;
        src.Play();
    }

    // SFX 정지 (모든 SFX)
    public void StopAllSFX()
    {
        foreach (var src in sfxSources)
        {
            src.Stop();
        }
    }

    public void StopAllSFXAfterTime(float seconds)
    {
        StartCoroutine(StopAllSFXCoroutine(seconds));
    }

    private IEnumerator StopAllSFXCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopAllSFX();
    }

    public void PlaySFXRepeated(int index, int repeatCount, float delayBetween = 0f)    // 효과음 횟수만큼 반복
    {
        StartCoroutine(PlaySFXRepeatedCoroutine(index, repeatCount, delayBetween));
    }

    private IEnumerator PlaySFXRepeatedCoroutine(int index, int repeatCount, float delayBetween)
    {
        AudioClip clip = sfxList[index].clip;
        for (int i = 0; i < repeatCount; i++)
        {
            PlaySFX(index);
            // 효과음 길이 + 추가 딜레이만큼 대기
            yield return new WaitForSeconds(clip.length + delayBetween);
        }
    }

    public AudioClip GetSFXClip(int index)
    {
        if (index >= 0 && index < sfxList.Count)
            return sfxList[index].clip;
        return null;
    }

    // 사용 가능한 SFX AudioSource 가져오기
    private AudioSource GetAvailableSFXSource()
    {
        foreach (var src in sfxSources)
        {
            if (!src.isPlaying) return src;
        }
        return null; // 모두 재생 중이면 null
    }
}
