using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<AudioClipData> bgmList;
    public List<AudioClipData> sfxList;

    private AudioSource bgmSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int sfxPoolSize = 5; // ���ÿ� ����� �� �ִ� SFX ��

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

        // BGM�� AudioSource
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;

        // SFX Pool ����
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sfxSources.Add(src);
        }
    }

    // BGM ���
    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmList.Count) return;

        AudioClipData data = bgmList[index];
        bgmSource.clip = data.clip;
        bgmSource.loop = data.isLoop;
        bgmSource.volume = data.volume;
        bgmSource.pitch = data.pitch;
        bgmSource.Play();
    }

    // BGM ����
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // SFX ���
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxList.Count) return;

        AudioClipData data = sfxList[index];
        AudioSource src = GetAvailableSFXSource();
        if (src == null) return; // Ǯ ����

        src.clip = data.clip;
        src.loop = data.isLoop;
        src.volume = data.volume;
        src.pitch = data.useRandomPitch
            ? Random.Range(data.minPitch, data.maxPitch)
            : data.pitch;
        src.Play();
    }

    // SFX ���� (��� SFX)
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

    public void PlaySFXRepeated(int index, int repeatCount, float delayBetween = 0f)    // ȿ���� Ƚ����ŭ �ݺ�
    {
        StartCoroutine(PlaySFXRepeatedCoroutine(index, repeatCount, delayBetween));
    }

    private IEnumerator PlaySFXRepeatedCoroutine(int index, int repeatCount, float delayBetween)
    {
        AudioClip clip = sfxList[index].clip;
        for (int i = 0; i < repeatCount; i++)
        {
            PlaySFX(index);
            // ȿ���� ���� + �߰� �����̸�ŭ ���
            yield return new WaitForSeconds(clip.length + delayBetween);
        }
    }

    // ��� ������ SFX AudioSource ��������
    private AudioSource GetAvailableSFXSource()
    {
        foreach (var src in sfxSources)
        {
            if (!src.isPlaying) return src;
        }
        return null; // ��� ��� ���̸� null
    }
}
