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

        SyncWithInventory(); // ���� �κ��丮 ���� ����ȭ
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            questListController?.UnlockQuestSlot(Quest.questName);

            Debug.Log($"[������ ����Ʈ ����] {Quest.questName}, �ʿ� ������: {ItemName}, ����: {RequiredAmount}");
        }
        else
        {
            Debug.Log($"[������ ����Ʈ ����] {Quest.questName}, ���� ����: {Quest.state}");
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
        Debug.Log($"[{Quest.questName}] ������ ����: {CollectedAmount}/{RequiredAmount}");
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
            Debug.LogWarning($"����Ʈ �Ϸ� �Ұ�: {Quest.questName}");
            return;
        }

        var item = InventoryManager.Instance.GetItemByName(ItemName);
        if (item != null)
        {
            CountManager.Instance.RemoveItemByID(item.itemID, RequiredAmount);
        }

        Quest.state = QuestState.Completed;
        RewardManager.Instance.GiveRewards(Quest.reward);
        Debug.Log($"[����Ʈ �Ϸ�] {Quest.questName}");
    }

    public void Undo()
    {
        Debug.Log($"[����Ʈ �ǵ�����] {Quest.questName}");
        Quest.state = QuestState.NotStarted;
        // �̵����̸� ��ǥ ��ġ �ʱ�ȭ �ʿ��ϸ� ���⿡ �߰�
    }
}
