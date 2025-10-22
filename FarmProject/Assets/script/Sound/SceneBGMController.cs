using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneBGMController : MonoBehaviour
{
    public static SceneBGMController Instance; // �̱���

    [Header("BGM ������ ����Ʈ (AudioClipData�� ���)")]
    public List<AudioClipData> bgmList; // ScriptableObject ����Ʈ

    private AudioSource audioSource;

    // �� �̸��� �ε��� ����
    private Dictionary<string, int> sceneBGMMap = new Dictionary<string, int>()
    {
        { "StartScene", 0 },
        { "MainScene", 1 },
        { "Ending", 2 },
    };

    private void Awake()
    {
        // �̱��� �ߺ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ���� ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ����� �ҽ� �ڵ� �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // ���� ���� BGM ��� ���
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
                    Debug.LogWarning($"[SceneBGMController] '{sceneName}'�� AudioClipData�� ��� ����");
                    return;
                }

                // ���� ���̸� ��� ��ŵ
                if (audioSource.clip == clipData.clip && audioSource.isPlaying)
                    return;

                // ����� ����
                audioSource.Stop();
                audioSource.clip = clipData.clip;
                audioSource.loop = clipData.isLoop;
                audioSource.volume = clipData.volume;

                // ���� ��ġ ���� ����
                float pitch = clipData.useRandomPitch
                    ? Random.Range(clipData.minPitch, clipData.maxPitch)
                    : clipData.pitch;

                audioSource.pitch = pitch;

                audioSource.Play();
            }
            else
            {
                Debug.LogWarning($"[SceneBGMController] �߸��� BGM �ε���: {bgmIndex}");
            }
        }
        else
        {
            Debug.LogWarning($"[SceneBGMController] '{sceneName}'�� �ش��ϴ� BGM�� �����ϴ�.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
