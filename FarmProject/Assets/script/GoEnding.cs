using UnityEngine;

public class GoEnding : MonoBehaviour
{
    [Header("엔딩 패널")]
    public GameObject endingPanel; // 인스펙터에서 엔딩 패널 연결

    [Header("엔딩 조건")]
    public int goldThreshold = 100; // 엔딩 조건 골드

    private void Start()
    {
        if (endingPanel != null)
            endingPanel.SetActive(false); // 초기에는 패널 숨기기

        // GoldManager 구독
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += CheckGold;
            // 현재 골드로도 바로 체크
            CheckGold(GoldManager.Instance.GetGold());
        }
        else
        {
            Debug.LogWarning("[GoEnding] GoldManager 인스턴스가 존재하지 않습니다.");
        }
    }

    private void CheckGold(int currentGold)
    {
        if (currentGold >= goldThreshold && endingPanel != null)
        {
            endingPanel.SetActive(true); // 엔딩 패널 활성화
        }
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= CheckGold;
    }
}
