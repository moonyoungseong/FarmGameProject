using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// SceneBGMController.cs
///
/// 씬마다 다른 BGM을 자동으로 재생하는 컨트롤러
/// BGM의 볼륨, 루프, 피치, 랜덤 피치 설정을 관리
/// 씬 이름을 BGM 인덱스와 매핑하여 씬 전환 시 자동으로 적절한 BGM을 재생
/// 싱글톤으로 동작
/// </summary>
public class SceneBGMController : MonoBehaviour
{
    public static SceneBGMController Instance;

    // Inspector에서 설정하는 BGM 리스트
    public List<AudioClipData> bgmList;

    // 현재 BGM을 재생하는 AudioSource
    private AudioSource audioSource;

    // 씬 이름을 BGM 인덱스와 매핑
    private Dictionary<string, int> sceneBGMMap = new Dictionary<string, int>()
    {
        { "StartScene", 0 },
        { "MainScene", 1 },
        { "Ending", 2 },
    };

    // 씬 로딩 이벤트 등록
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

        // AudioSource 자동 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 게임 시작 시 현재 씬의 BGM을 즉시 재생
    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    // 씬이 로드될 때마다 해당 씬에 맞는 BGM을 재생
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // Dictionary에서 씬 이름을 기반으로 BGM 인덱스 검색
        if (sceneBGMMap.TryGetValue(sceneName, out int bgmIndex))
        {
            // 유효한 인덱스인지 체크
            if (bgmList != null && bgmIndex >= 0 && bgmIndex < bgmList.Count)
            {
                AudioClipData clipData = bgmList[bgmIndex];

                if (clipData == null || clipData.clip == null)
                {
                    Debug.LogWarning("[SceneBGMController] " + sceneName + "에 AudioClipData가 비어 있음");
                    return;
                }

                // 같은 BGM이면 다시 재생할 필요 없음
                if (audioSource.clip == clipData.clip && audioSource.isPlaying)
                    return;

                // 오디오 설정
                audioSource.Stop();
                audioSource.clip = clipData.clip;
                audioSource.loop = clipData.isLoop;
                audioSource.volume = clipData.volume;

                // 랜덤 피치 옵션 적용
                float pitch = clipData.useRandomPitch
                    ? Random.Range(clipData.minPitch, clipData.maxPitch)
                    : clipData.pitch;

                audioSource.pitch = pitch;

                // BGM 재생
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("[SceneBGMController] 잘못된 BGM 인덱스: " + bgmIndex);
            }
        }
        else
        {
            Debug.LogWarning("[SceneBGMController] " + sceneName + "에 해당하는 BGM이 없음");
        }
    }

    // 씬 로드 이벤트 등록 해제
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
