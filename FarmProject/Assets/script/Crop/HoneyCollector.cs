using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoneyCollector : MonoBehaviour
{
    public TextMeshProUGUI timerText; // 남은 시간을 표시할 UI Text
    public Button collectButton; // 꿀 수집 버튼
    public Button waitButton; // 대기 버튼

    private int honeyCount = 0; // 현재 충전된 꿀 개수
    private float timer = 10f; // 타이머 초기값 (10초)

    void Start()
    {
        // 버튼 클릭 이벤트 등록
        collectButton.onClick.AddListener(CollectHoney);
        waitButton.onClick.AddListener(WaitHoney);

        UpdateUI();
        UpdateButtonStates(); // 초기 버튼 상태 설정
    }

    void Update()
    {
        // 타이머 감소
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
                timer = 0;

            UpdateUI();
        }
        else if (honeyCount == 0) // 시간이 0이 되면 꿀 충전
        {
            honeyCount = 3;
            UpdateButtonStates(); // 버튼 상태 업데이트
        }
    }

    void CollectHoney()
    {
        if (honeyCount > 0)
        {
            // 꿀을 인벤토리에 추가 (여기서는 Debug로 확인)
            Debug.Log($"꿀 {honeyCount}개를 수집했습니다!");

            honeyCount = 0; // 충전된 꿀 초기화
            timer = 10f; // 타이머 초기화 (10초)
            UpdateUI();
            UpdateButtonStates(); // 버튼 상태 업데이트
        }
    }

    void WaitHoney()
    {
        Debug.Log("꿀이 충전되는 중입니다...");
        // 대기 버튼은 단순히 비활성화된 상태로 유지
    }

    void UpdateUI()
    {
        // 시간 형식을 분:초 또는 초로 표시
        if (timer >= 60)
        {
            int minutes = Mathf.FloorToInt(timer / 60); // 분 계산
            int seconds = Mathf.FloorToInt(timer % 60); // 초 계산
            timerText.text = $"남은 시간: {minutes}분 {seconds}초";
        }
        else
        {
            int seconds = Mathf.FloorToInt(timer); // 초만 표시
            timerText.text = $"남은 시간: {seconds}초";
        }
    }

    void UpdateButtonStates()
    {
        // 시간이 남아 있으면 대기 버튼만 활성화
        if (timer > 0 || honeyCount == 0)
        {
            waitButton.gameObject.SetActive(true);
            collectButton.gameObject.SetActive(false);
        }
        else
        {
            // 시간이 0이 되고 꿀이 충전되었으면 수집 버튼 활성화
            waitButton.gameObject.SetActive(false);
            collectButton.gameObject.SetActive(true);
        }
    }
}
