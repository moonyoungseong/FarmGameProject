using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneBGMController : MonoBehaviour
{
    public static SceneBGMController Instance; // 싱글톤

    [Header("BGM 데이터 리스트 (AudioClipData로 등록)")]
    public List<AudioClipData> bgmList; // ScriptableObject 리스트

    private AudioSource audioSource;

    // 씬 이름과 인덱스 매핑
    private Dictionary<string, int> sceneBGMMap = new Dictionary<string, int>()
    {
        { "StartScene", 0 },
        { "MainScene", 1 },
        { "Ending", 2 },
    };

    private void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 삭제 방지
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 오디오 소스 자동 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // 현재 씬의 BGM 즉시 재생
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneBGMMap.TryGetValue(sceneName, out int bgmIndex))
        {
            if (bgmList != null && bgmIndex >= 0 && bgmIndex < bgmList.Count)
            {
                AudioClipData clipData = bgmList[bgmIndex];

                if (clipData == null || clipData.clip == null)
                {
                    Debug.LogWarning($"[SceneBGMController] '{sceneName}'에 AudioClipData가 비어 있음");
                    return;
                }

                // 같은 곡이면 재생 스킵
                if (audioSource.clip == clipData.clip && audioSource.isPlaying)
                    return;

                // 오디오 세팅
                audioSource.Stop();
                audioSource.clip = clipData.clip;
                audioSource.loop = clipData.isLoop;
                audioSource.volume = clipData.volume;

                // 랜덤 피치 적용 여부
                float pitch = clipData.useRandomPitch
                    ? Random.Range(clipData.minPitch, clipData.maxPitch)
                    : clipData.pitch;

                audioSource.pitch = pitch;

                audioSource.Play();
            }
            else
            {
                Debug.LogWarning($"[SceneBGMController] 잘못된 BGM 인덱스: {bgmIndex}");
            }
        }
        else
        {
            Debug.LogWarning($"[SceneBGMController] '{sceneName}'에 해당하는 BGM이 없습니다.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
