using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// HoneyCollector.cs
/// 꿀벌집에서 일정 시간이 지나면 꿀이 충전되고,
/// 플레이어가 꿀을 수집하거나 기다릴 수 있도록 처리하는 컴포넌트.
///
/// - 60초 타이머 기반 꿀 충전
/// - 수집 버튼 / 대기 버튼 UI 상태 관리
/// - 꿀 수집 시 인벤토리에 아이템 추가
/// - 대기 버튼 클릭 시 알림 UI 잠시 표시
/// </summary>
public class HoneyCollector : MonoBehaviour
{
    // 남은 시간을 표시하는 TMP UI 텍스트
    public TextMeshProUGUI timerText;

    // 꿀을 수집하는 버튼
    public Button collectButton;

    // 시간이 남았을 때 누르는 '대기' 버튼
    public Button waitButton;

    // 대기 버튼 클릭 시 표시되는 알림 UI
    public GameObject notificationUI;

    // 현재 충전된 꿀 개수
    private int honeyCount = 0;

    private float timer = 60f;

    // 인벤토리에 접근하기 위한 싱글톤
    private InventoryManager inventory;

    // 초기 설정 (버튼 리스너, UI 초기화 등)
    private void Start()
    {
        collectButton.onClick.AddListener(CollectHoney);
        waitButton.onClick.AddListener(OnWaitButtonClicked);

        UpdateUI();
        UpdateButtonStates();

        notificationUI.SetActive(false);

        inventory = InventoryManager.Instance;
    }

    // 매 프레임 타이머 감소 및 꿀 충전 처리
    private void Update()
    {
        // 타이머 감소
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                timer = 0;

            UpdateUI();
        }
        else if (honeyCount == 0)
        {
            // 시간이 끝나면 꿀 3개 충전
            honeyCount = 3;
            UpdateButtonStates();
        }
    }

    /// <summary>
    /// 꿀 수집 버튼 클릭 시 호출되는 메서드.
    /// 꿀을 인벤토리에 추가하고 타이머 및 상태 초기화.
    /// </summary>
    private void CollectHoney()
    {
        if (honeyCount > 0)
        {
            AddItemToInventory("꿀", 3);

            honeyCount = 0;
            timer = 60f;

            UpdateUI();
            UpdateButtonStates();
        }
    }

    // 대기 버튼 클릭 시 알림 UI를 잠시 표시
    private void OnWaitButtonClicked()
    {
        StartCoroutine(ShowNotification());
    }

    // 알림 UI를 2초간 표시한 뒤 숨기는 코루틴
    private IEnumerator ShowNotification()
    {
        notificationUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        notificationUI.SetActive(false);
    }

    // 남은 시간을 TMP UI에 업데이트
    private void UpdateUI()
    {
        if (timer >= 60)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = $"남은 시간: {minutes}분 {seconds}초";
        }
        else
        {
            int seconds = Mathf.FloorToInt(timer);
            timerText.text = $"남은 시간: {seconds}초";
        }
    }

    // 꿀 충전 여부에 따라 Collect / Wait 버튼 상태를 갱신
    private void UpdateButtonStates()
    {
        if (timer > 0 || honeyCount == 0)
        {
            waitButton.gameObject.SetActive(true);
            collectButton.gameObject.SetActive(false);
        }
        else
        {
            waitButton.gameObject.SetActive(false);
            collectButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 특정 아이템을 인벤토리에 추가하는 기능.
    /// AllItemList에서 찾고 MyItemList에 추가 또는 수량 증가.
    /// </summary>
    /// <param name="itemName">추가할 아이템 이름</param>
    /// <param name="quantityToAdd">추가 수량</param>
    public void AddItemToInventory(string itemName, int quantityToAdd)
    {
        Item targetItem = inventory.AllItemList.Find(item => item.itemName == itemName);

        if (targetItem != null)
        {
            Item existingItem = inventory.MyItemList.Find(item => item.itemID == targetItem.itemID);

            if (existingItem != null)
            {
                int currentQuantity = int.Parse(existingItem.quantity);
                existingItem.quantity = (currentQuantity + quantityToAdd).ToString();
            }
            else
            {
                targetItem.quantity = quantityToAdd.ToString();
                inventory.MyItemList.Add(targetItem);
            }

            inventory.Save();
        }
        else
        {
            Debug.LogWarning($"{itemName}이라는 아이템을 AllItemList에서 찾을 수 없습니다.");
        }
    }
}
