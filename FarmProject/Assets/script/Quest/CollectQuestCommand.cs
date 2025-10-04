using System;
using UnityEngine;

public class CollectQuestCommand : IQuestCommand
{
    public Quest Quest { get; private set; }
    public string ItemName { get; private set; }
    public int RequiredAmount { get; private set; }
    public int CollectedAmount { get; private set; }

    private QuestListController questListController;

    public CollectQuestCommand(Quest quest, string itemName, int requiredAmount)
    {
        Quest = quest;
        ItemName = itemName;
        RequiredAmount = requiredAmount;

        // 초기 동기화
        SyncWithInventory();

        // QuestListController 자동 주입 (있다면)
        questListController = GameObject.FindObjectOfType<QuestListController>();
        if (questListController == null)
            Debug.LogWarning("QuestListController를 찾지 못했습니다. UI 갱신이 불가능할 수 있습니다.");
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            Debug.Log($"[퀘스트 시작] {Quest.questName} - {ItemName} {RequiredAmount}개 수집");
        }

        // 슬롯 자물쇠 해제 (있다면)
        questListController?.UnlockQuestSlot(Quest.questName);
    }

    /// <summary>
    /// 인벤토리와 동기화해서 CollectedAmount 업데이트
    /// </summary>
    public void SyncWithInventory()
    {
        Item item = InventoryManager.Instance.GetItemByName(ItemName);
        int count = (item != null) ? item.quantityInt : 0;
        CollectedAmount = Mathf.Min(count, RequiredAmount);
    }

    /// <summary>
    /// 플레이어가 아이템 획득 시 호출
    /// </summary>
    public void CollectItem(string collectedItemName)
    {
        if (Quest == null || Quest.state != QuestState.InProgress) return;

        if (!string.Equals(collectedItemName?.Trim(), ItemName?.Trim(), StringComparison.OrdinalIgnoreCase))
            return; // 다른 아이템이면 무시

        SyncWithInventory();
        Debug.Log($"[{Quest.questName}] 아이템 수집 업데이트: {CollectedAmount}/{RequiredAmount}");
    }

    /// <summary>
    ///  완료 가능 여부 확인 (UISwitch에서 사용)
    /// </summary>
    public bool CanComplete()
    {
        string questItem = ItemName.Trim().ToLower(); // 퀘스트 아이템 이름
        Item item = InventoryManager.Instance.MyItemList.Find(x => x.itemName.Trim().ToLower() == questItem);
        bool result = (item != null && item.quantityInt >= RequiredAmount);

        Debug.Log($"[CanComplete] Quest:{Quest.questName}, Item:{ItemName}, 현재:{(item != null ? item.quantityInt : 0)}, 필요:{RequiredAmount}, 가능여부:{result}");

        return result;
    }

    /// <summary>
    /// 퀘스트 완료 처리 (확인 버튼 등에서 호출)
    /// </summary>
    public void CompleteQuest()
    {
        if (!CanComplete())
        {
            Debug.LogWarning($"퀘스트 완료 불가: {Quest.questName}, 아이템 부족");
            return;
        }

        // 아이템 차감
        Item item = InventoryManager.Instance.GetItemByName(ItemName);
        if (item != null)
        {
            CountManager.Instance.RemoveItemByID(item.itemID, RequiredAmount);
            Debug.Log($"퀘스트 완료: 아이템 {item.itemName} {RequiredAmount}개 차감됨");
        }

        // 상태 변경
        Quest.state = QuestState.Completed;
        Debug.Log($"[퀘스트 완료] {Quest.questName} - {ItemName} 수집 완료!");

        // 보상 지급
        RewardManager.Instance.GiveRewards(Quest.reward);
        foreach (var reward in Quest.reward)
        {
            Debug.Log($"보상 지급: {reward.itemname} x{reward.quantity}");
        }
    }
}
