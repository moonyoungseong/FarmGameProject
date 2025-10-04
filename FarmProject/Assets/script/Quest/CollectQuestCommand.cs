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

        // �ʱ� ����ȭ
        SyncWithInventory();

        // QuestListController �ڵ� ���� (�ִٸ�)
        questListController = GameObject.FindObjectOfType<QuestListController>();
        if (questListController == null)
            Debug.LogWarning("QuestListController�� ã�� ���߽��ϴ�. UI ������ �Ұ����� �� �ֽ��ϴ�.");
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            Debug.Log($"[����Ʈ ����] {Quest.questName} - {ItemName} {RequiredAmount}�� ����");
        }

        // ���� �ڹ��� ���� (�ִٸ�)
        questListController?.UnlockQuestSlot(Quest.questName);
    }

    /// <summary>
    /// �κ��丮�� ����ȭ�ؼ� CollectedAmount ������Ʈ
    /// </summary>
    public void SyncWithInventory()
    {
        Item item = InventoryManager.Instance.GetItemByName(ItemName);
        int count = (item != null) ? item.quantityInt : 0;
        CollectedAmount = Mathf.Min(count, RequiredAmount);
    }

    /// <summary>
    /// �÷��̾ ������ ȹ�� �� ȣ��
    /// </summary>
    public void CollectItem(string collectedItemName)
    {
        if (Quest == null || Quest.state != QuestState.InProgress) return;

        if (!string.Equals(collectedItemName?.Trim(), ItemName?.Trim(), StringComparison.OrdinalIgnoreCase))
            return; // �ٸ� �������̸� ����

        SyncWithInventory();
        Debug.Log($"[{Quest.questName}] ������ ���� ������Ʈ: {CollectedAmount}/{RequiredAmount}");
    }

    /// <summary>
    ///  �Ϸ� ���� ���� Ȯ�� (UISwitch���� ���)
    /// </summary>
    public bool CanComplete()
    {
        string questItem = ItemName.Trim().ToLower(); // ����Ʈ ������ �̸�
        Item item = InventoryManager.Instance.MyItemList.Find(x => x.itemName.Trim().ToLower() == questItem);
        bool result = (item != null && item.quantityInt >= RequiredAmount);

        Debug.Log($"[CanComplete] Quest:{Quest.questName}, Item:{ItemName}, ����:{(item != null ? item.quantityInt : 0)}, �ʿ�:{RequiredAmount}, ���ɿ���:{result}");

        return result;
    }

    /// <summary>
    /// ����Ʈ �Ϸ� ó�� (Ȯ�� ��ư ��� ȣ��)
    /// </summary>
    public void CompleteQuest()
    {
        if (!CanComplete())
        {
            Debug.LogWarning($"����Ʈ �Ϸ� �Ұ�: {Quest.questName}, ������ ����");
            return;
        }

        // ������ ����
        Item item = InventoryManager.Instance.GetItemByName(ItemName);
        if (item != null)
        {
            CountManager.Instance.RemoveItemByID(item.itemID, RequiredAmount);
            Debug.Log($"����Ʈ �Ϸ�: ������ {item.itemName} {RequiredAmount}�� ������");
        }

        // ���� ����
        Quest.state = QuestState.Completed;
        Debug.Log($"[����Ʈ �Ϸ�] {Quest.questName} - {ItemName} ���� �Ϸ�!");

        // ���� ����
        RewardManager.Instance.GiveRewards(Quest.reward);
        foreach (var reward in Quest.reward)
        {
            Debug.Log($"���� ����: {reward.itemname} x{reward.quantity}");
        }
    }
}
