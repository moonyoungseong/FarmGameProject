using UnityEngine;

public class DeliveryQuestCommand : IQuestCommand
{
    public Quest Quest { get; private set; }
    public string ItemName { get; private set; }
    public string ReceiverNPC { get; private set; }
    private QuestListController questListController;

    public DeliveryQuestCommand(Quest quest, string itemName, string receiverNPC, QuestListController controller)
    {
        Quest = quest;
        ItemName = itemName;
        ReceiverNPC = receiverNPC;
        questListController = controller;
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            questListController?.UnlockQuestSlot(Quest.questName);

            Debug.Log($"[������ ����Ʈ ����] {Quest.questName}, ������: {ItemName}, ������: {ReceiverNPC}");
        }
        else
        {
            Debug.Log($"[������ ����Ʈ ����] {Quest.questName}, ���� ����: {Quest.state}");
        }
    }

    public void DeliverItem(string itemName, string npcName)
    {
        if (Quest.state != QuestState.InProgress) return;
        if (itemName != ItemName || npcName != ReceiverNPC) return;

        var item = InventoryManager.Instance.GetItemByName(ItemName);
        if (item != null)
        {
            CountManager.Instance.RemoveItemByID(item.itemID, 1);
            Debug.Log($"[{Quest.questName}] {ReceiverNPC}���� {ItemName} ���� �Ϸ�");
        }

        CompleteQuest();
    }

    public void CompleteQuest()
    {
        if (Quest.state == QuestState.InProgress)
        {
            Quest.state = QuestState.Completed;
            RewardManager.Instance.GiveRewards(Quest.reward);
            Debug.Log($"[����Ʈ �Ϸ�] {Quest.questName}");
        }
    }
    public void Undo()
    {
        Debug.Log($"[����Ʈ �ǵ�����] {Quest.questName}");
        Quest.state = QuestState.NotStarted;
        // �̵����̸� ��ǥ ��ġ �ʱ�ȭ �ʿ��ϸ� ���⿡ �߰�
    }
}
