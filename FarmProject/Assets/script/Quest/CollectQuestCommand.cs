using UnityEngine;

public class CollectQuestCommand : IQuestCommand
{
    public Quest Quest { get; private set; }
    public string ItemName { get; private set; }
    public int RequiredAmount { get; private set; }
    public int CollectedAmount { get; private set; }

    private QuestListController questListController;

    public CollectQuestCommand(Quest quest, string itemName, int requiredAmount, QuestListController controller)
    {
        Quest = quest;
        ItemName = itemName;
        RequiredAmount = requiredAmount;
        questListController = controller;

        SyncWithInventory(); // 현재 인벤토리 상태 동기화
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            questListController?.UnlockQuestSlot(Quest.questName);

            Debug.Log($"[수집형 퀘스트 시작] {Quest.questName}, 필요 아이템: {ItemName}, 개수: {RequiredAmount}");
        }
        else
        {
            Debug.Log($"[수집형 퀘스트 실행] {Quest.questName}, 현재 상태: {Quest.state}");
        }
    }

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

    public void CollectItem(string collectedItemName)
    {
        if (Quest.state != QuestState.InProgress || collectedItemName != ItemName) return;

        SyncWithInventory();
        Debug.Log($"[{Quest.questName}] 아이템 수집: {CollectedAmount}/{RequiredAmount}");
    }

    public bool CanComplete()
    {
        var item = InventoryManager.Instance.GetItemByName(ItemName);
        return item != null && item.quantityInt >= RequiredAmount;
    }

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
        RewardManager.Instance.GiveRewards(Quest.reward);
        Debug.Log($"[퀘스트 완료] {Quest.questName}");
    }

    public void Undo()
    {
        Debug.Log($"[퀘스트 되돌리기] {Quest.questName}");
        Quest.state = QuestState.NotStarted;
        // 이동형이면 목표 위치 초기화 필요하면 여기에 추가
    }
}
