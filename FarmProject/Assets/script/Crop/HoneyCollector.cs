using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HoneyCollector : MonoBehaviour
{
    public TextMeshProUGUI timerText; // 남은 시간을 표시할 UI Text
    public Button collectButton; // 꿀 수집 버튼
    public Button waitButton; // 대기 버튼
    public GameObject notificationUI; // 특정 UI (알림 메시지 등)

    private int honeyCount = 0; // 현재 충전된 꿀 개수
    private float timer = 60f; // 타이머 초기값 (60초)

    private InventoryManager inventory;

    void Start()
    {
        // 버튼 클릭 이벤트 등록
        collectButton.onClick.AddListener(CollectHoney);
        waitButton.onClick.AddListener(OnWaitButtonClicked);

        UpdateUI();
        UpdateButtonStates(); // 초기 버튼 상태 설정
        notificationUI.SetActive(false); // UI 초기 상태는 비활성화

        // 싱글톤을 통해 InventoryManager에 접근
        inventory = InventoryManager.Instance;
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

            AddItemToInventory("꿀", 3);

            honeyCount = 0; // 충전된 꿀 초기화
            timer = 60f; // 타이머 초기화 (10초)
            UpdateUI();
            UpdateButtonStates(); // 버튼 상태 업데이트
        }
    }

    void OnWaitButtonClicked()
    {
        Debug.Log("대기 버튼 클릭됨");
        StartCoroutine(ShowNotification()); // 알림 UI 표시 코루틴 호출
    }

    IEnumerator ShowNotification()
    {
        notificationUI.SetActive(true); // UI 활성화
        yield return new WaitForSeconds(2f); // 2초 대기
        notificationUI.SetActive(false); // UI 비활성화
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

    public void AddItemToInventory(string itemName, int quantityToAdd)
    {
        Debug.Log($"AddItemToInventory 호출됨: {itemName}, 추가할 수량: {quantityToAdd}");              // 딱 여기까지만 호출 됨

        // AllItemList에서 아이템 검색
        Item targetItem = inventory.AllItemList.Find(item => item.itemName == itemName);

        if (targetItem != null)
        {
            Debug.Log($"아이템 {itemName}이 AllItemList에서 발견됨");

            // 이미 MyItemList에 있는지 확인
            Item existingItem = inventory.MyItemList.Find(item => item.itemID == targetItem.itemID);

            if (existingItem != null)
            {
                Debug.Log($"{itemName}이 MyItemList에 이미 존재함, 기존 수량: {existingItem.quantity}");

                // 존재하면 수량 증가
                int currentQuantity = int.Parse(existingItem.quantity);
                existingItem.quantity = (currentQuantity + quantityToAdd).ToString();
                Debug.Log($"{itemName}의 수량이 {quantityToAdd}개 증가했습니다. 새로운 수량: {existingItem.quantity}");
            }
            else
            {
                Debug.Log($"{itemName}이 MyItemList에 없음, 새로 추가");

                // 존재하지 않으면 새로 추가
                targetItem.quantity = quantityToAdd.ToString();
                inventory.MyItemList.Add(targetItem);
                Debug.Log($"{itemName} {quantityToAdd}개가 MyItemList에 추가되었습니다.");
            }

            // 변경 사항 저장
            inventory.Save();
            Debug.Log($"인벤토리 저장됨");
        }
        else
        {
            Debug.LogWarning($"{itemName}이라는 아이템을 AllItemList에서 찾을 수 없습니다.");
        }
    }
}
