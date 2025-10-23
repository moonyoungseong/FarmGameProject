using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoEnding : MonoBehaviour
{
    [Header("엔딩 패널")]
    public GameObject endingPanel;

    [Header("엔딩 조건")]
    public int goldThreshold = 100;

    private void Start()
    {
        if (endingPanel != null)
            endingPanel.SetActive(false);

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += CheckGold;
            CheckGold(GoldManager.Instance.GetGold());
        }
    }

    private void CheckGold(int currentGold)
    {
        if (currentGold >= goldThreshold && endingPanel != null)
        {
            endingPanel.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= CheckGold;
    }

    public void OnClickEndingAButton1()
    {
        EndingChoice.SelectedEnding = EndingType.EndingA;
        //LoadScene("EndScene");
    }

    public void OnClickEndingBButton2()
    {
        EndingChoice.SelectedEnding = EndingType.EndingB;
        //LoadScene("EndScene");
    }

    // 씬 이동용 공용 함수 (Coroutine)
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
        else
        {
            Debug.LogWarning("[GoEnding] LoadScene 호출 시 씬 이름이 비어있습니다.");
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 씬 로드
        SceneManager.LoadScene(sceneName);

        // 1프레임 대기 후 카메라/조명 초기화
        yield return null;

        // 엔딩씬 Brightness나 Post Processing이 바로 적용되도록 카메라 강제 활성화
        var cam = FindObjectOfType<Camera>();
        if (cam != null)
        {
            cam.enabled = false;
            cam.enabled = true;
        }

        // 필요하면 Cinemachine Virtual Camera도 강제로 활성화
        var vCam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (vCam != null)
            vCam.gameObject.SetActive(false);
        if (vCam != null)
            vCam.gameObject.SetActive(true);
    }
}
