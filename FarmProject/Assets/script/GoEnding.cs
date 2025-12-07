using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// GoEnding.cs
/// 
/// 플레이어의 골드가 특정 기준을 넘으면 엔딩 패널을 활성화하고,
/// 엔딩 버튼(A/B)을 통해 선택된 엔딩 타입을 저장한 뒤
/// 엔딩 씬으로 이동할 수 있게 관리하는 클래스.
/// </summary>
public class GoEnding : MonoBehaviour
{
    [Header("엔딩 패널")]
    // 골드 기준을 달성했을 때 표시할 엔딩 선택 패널.
    public GameObject endingPanel;

    [Header("엔딩 조건")]
    // 엔딩 패널을 표시하기 위한 골드 기준값, 임의로 적어놓음
    public int goldThreshold = 100;

    private void Start()
    {
        // 시작 시 엔딩 패널 숨기기
        if (endingPanel != null)
            endingPanel.SetActive(false);

        // GoldManager 이벤트 등록
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += CheckGold;

            // 게임 시작 시 초기 골드 기준 달성 여부 체크
            CheckGold(GoldManager.Instance.GetGold());
        }
    }

    /// <summary>
    /// 현재 골드가 기준값 이상이면 엔딩 선택 패널을 보여준다
    /// </summary>
    /// <param name="currentGold">현재 플레이어의 골드 값</param>
    private void CheckGold(int currentGold)
    {
        if (currentGold >= goldThreshold && endingPanel != null)
        {
            endingPanel.SetActive(true);
        }
    }

    // 오브젝트가 Destroy될 때 GoldManager 이벤트 구독 해제.
    private void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= CheckGold;
    }

    // 엔딩 A 버튼 클릭 시 EndingChoice에 EndingA 저장.
    public void OnClickEndingAButton1()
    {
        EndingChoice.SelectedEnding = EndingType.EndingA;
    }

    // 엔딩 B 버튼 클릭 시 EndingChoice에 EndingB 저장.
    public void OnClickEndingBButton2()
    {
        EndingChoice.SelectedEnding = EndingType.EndingB;
    }

    /// <summary>
    /// 씬을 로드하는 공용 함수. 이름이 유효하면 코루틴 실행.
    /// </summary>
    /// <param name="sceneName">이동할 씬 이름</param>
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

    /// <summary>
    /// 씬 로드 후 한 프레임 대기하여 카메라나 조명 초기화를 강제 적용하고,
    /// 필요 시 Cinemachine VirtualCamera도 새로고침 
    /// </summary>
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 씬 로드
        SceneManager.LoadScene(sceneName);

        // 한 프레임 대기 후 초기화 반영
        yield return null;

        // 카메라 강제 재활성화
        var cam = FindObjectOfType<Camera>();
        if (cam != null)
        {
            cam.enabled = false;
            cam.enabled = true;
        }

        // Cinemachine 카메라도 초기화를 위해 재활성화
        var vCam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (vCam != null)
            vCam.gameObject.SetActive(false);
        if (vCam != null)
            vCam.gameObject.SetActive(true);
    }
}
