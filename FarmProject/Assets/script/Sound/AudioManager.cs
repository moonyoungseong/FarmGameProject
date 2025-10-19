using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<AudioClipData> bgmList;
    public List<AudioClipData> sfxList;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    //  BGM 재생
    public void PlayBGM(int index)
    {
        AudioClipData data = bgmList[index];
        bgmSource.clip = data.clip;
        bgmSource.loop = data.isLoop;
        bgmSource.volume = data.volume;
        bgmSource.pitch = data.pitch;
        bgmSource.Play();
    }

    //  SFX 재생
    public void PlaySFX(int index)
    {
        AudioClipData data = sfxList[index];
        sfxSource.loop = data.isLoop;
        sfxSource.volume = data.volume;
        sfxSource.pitch = data.useRandomPitch
            ? Random.Range(data.minPitch, data.maxPitch)
            : data.pitch;
        sfxSource.PlayOneShot(data.clip);
    }
}
