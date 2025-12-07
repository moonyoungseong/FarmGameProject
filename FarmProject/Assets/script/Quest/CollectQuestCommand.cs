using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CollectQuestCommand.cs
///
/// - 수집형 퀘스트를 처리하는 명령 클래스.
/// - 특정 아이템을 지정된 개수만큼 모으면 퀘스트가 완료됨.
/// - 인벤토리와 동기화하고, UI 슬롯 활성화, 완료 조건 확인, 보상 지급 등을 담당.
/// </summary>
public class CollectQuestCommand : IQuestCommand
{
    // 현재 수행 중인 퀘스트 정보
    public Quest Quest { get; private set; }

    // 수집해야 하는 아이템 이름
    public string ItemName { get; private set; }

    // 필요한 총 아이템 수
    public int RequiredAmount { get; private set; }

    // 현재 수집된 아이템 개수
    public int CollectedAmount { get; private set; }

    // 퀘스트 UI 슬롯을 열고 갱신하는 컨트롤러
    private QuestListController questListController;

    /// <summary>
    /// 생성자
    /// - 퀘스트 데이터, 대상 아이템, 필요 수량, UI 컨트롤러를 초기화한다.
    /// - 인벤토리와 즉시 동기화하여 현재 보유량을 반영한다.
    /// </summary>
    public CollectQuestCommand(Quest quest, string itemName, int requiredAmount, QuestListController controller)
    {
        Quest = quest;
        ItemName = itemName;
        RequiredAmount = requiredAmount;
        questListController = controller;

        SyncWithInventory();
    }

    /// <summary>
    /// 퀘스트 실행
    /// NotStarted 상태라면 InProgress로 변경하고 UI 슬롯을 활성화
    /// </summary>
    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;

            // 퀘스트 리스트에서 슬롯 열기
            questListController?.UnlockQuestSlot(Quest.questName);
        }
    }

    // 인벤토리와 동기화하여 CollectedAmount를 업데이트한다
    public void SyncWithInventory()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager.Instance is null");
            CollectedAmount = 0;
            return;
        }

        var item = InventoryManager.Instance.GetItemByName(ItemName);
        CollectedAmount = item != null ? Mathf.Min(item.quantityInt, RequiredAmount) : 0;
    }

    // 아이템 획득 시 호출
    public void CollectItem(string collectedItemName)
    {
        if (Quest.state != QuestState.InProgress || collectedItemName != ItemName)
            return;

        SyncWithInventory();
    }

    // 퀘스트 완료 조건을 만족하는지 확인
    public bool CanComplete()
    {
        var item = InventoryManager.Instance.GetItemByName(ItemName);
        return item != null && item.quantityInt >= RequiredAmount;
    }

    /// <summary>
    /// 퀘스트 완료 처리
    /// - 아이템 소비
    /// - 보상 지급
    /// - 퀘스트 상태 Completed로 변경
    /// </summary>
    public void CompleteQuest()
    {
        if (!CanComplete())
        {
            Debug.LogWarning($"퀘스트 완료 불가: {Quest.questName}");
            return;
        }

        var item = InventoryManager.Instance.GetItemByName(ItemName);

        if (item != null)
        {
            CountManager.Instance.RemoveItemByID(item.itemID, RequiredAmount);
        }

        Quest.state = QuestState.Completed;

        // 보상 지급
        RewardManager.Instance.GiveRewards(Quest.reward);
    }

    /// <summary>
    /// 퀘스트 되돌리기
    /// - 상태를 NotStarted로 초기화
    /// </summary>
    public void Undo()
    {
        Quest.state = QuestState.NotStarted;
    }
}
