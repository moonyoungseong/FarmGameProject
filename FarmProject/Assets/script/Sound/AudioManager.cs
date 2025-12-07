using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AudioManager.cs
///
/// - 오디오 재생을 관리하는 싱글톤 매니저
/// - AudioSource 풀링을 사용하여 동시에 재생 가능한 SFX 수를 제한
/// - 개별 AudioClipData로 볼륨/피치/루프/랜덤피치 옵션을 설정
/// - SFX 재생, 반복 재생, 전체 정지, 지연 후 정지 기능 포함
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Inspector에서 설정할 SFX 데이터 리스트
    public List<AudioClipData> sfxList;

    // 내부적으로 관리하는 AudioSource 풀
    private List<AudioSource> sfxSources = new List<AudioSource>();

    // 동시에 재생 가능한 SFX 수
    private int sfxPoolSize = 5;

    // 싱글톤 초기화 및 SFX 오디오 소스 풀 생성
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

        // 초기 풀 생성
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sfxSources.Add(src);
        }
    }

    // 인덱스로 AudioClipData를 선택하여 재생
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxList.Count) return;

        AudioClipData data = sfxList[index];
        AudioSource src = GetAvailableSFXSource();
        if (src == null) return; // 풀에 사용 가능한 소스 없음

        src.clip = data.clip;
        src.loop = data.isLoop;
        src.volume = data.volume;
        src.pitch = data.useRandomPitch
            ? Random.Range(data.minPitch, data.maxPitch)
            : data.pitch;
        src.Play();
    }

    // 현재 풀에 있는 모든 AudioSource의 재생을 중지
    public void StopAllSFX()
    {
        foreach (var src in sfxSources)
        {
            src.Stop();
        }
    }

    // 지정한 시간 이후에 모든 SFX를 정지
    public void StopAllSFXAfterTime(float seconds)
    {
        StartCoroutine(StopAllSFXCoroutine(seconds));
    }

    // StopAllSFXAfterTime의 코루틴 구현
    private IEnumerator StopAllSFXCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopAllSFX();
    }

    // 특정 SFX를 repeatCount만큼 연속 재생
    public void PlaySFXRepeated(int index, int repeatCount, float delayBetween = 0f)
    {
        StartCoroutine(PlaySFXRepeatedCoroutine(index, repeatCount, delayBetween));
    }

    // PlaySFXRepeated의 코루틴 구현
    private IEnumerator PlaySFXRepeatedCoroutine(int index, int repeatCount, float delayBetween)
    {
        if (index < 0 || index >= sfxList.Count) yield break;

        AudioClip clip = sfxList[index].clip;
        for (int i = 0; i < repeatCount; i++)
        {
            PlaySFX(index);
            // 효과음 길이 + 추가 딜레이만큼 대기
            yield return new WaitForSeconds(clip.length + delayBetween);
        }
    }

    // 외부에서 특정 SFX의 AudioClip을 직접 얻고 싶을 때 사용
    public AudioClip GetSFXClip(int index)
    {
        if (index >= 0 && index < sfxList.Count)
            return sfxList[index].clip;
        return null;
    }

    // 현재 풀에서 재생 중이지 않은 AudioSource를 반환
    private AudioSource GetAvailableSFXSource()
    {
        foreach (var src in sfxSources)
        {
            if (!src.isPlaying) return src;
        }
        return null;
    }
}
