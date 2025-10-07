using UnityEngine;

public class DeliveryQuestCommand : IQuestCommand
{
    private Quest quest;
    private string itemName;
    private string receiverNPC;
    private bool isAccepted = false;

    private QuestListController questListController;

    public DeliveryQuestCommand(Quest quest, string itemName, string receiverNPC)
    {
        this.quest = quest;
        this.itemName = itemName;
        this.receiverNPC = receiverNPC;

        // QuestListController �ڵ� ���� (�ִٸ�)
        questListController = GameObject.FindObjectOfType<QuestListController>();
        if (questListController == null)
            Debug.LogWarning("QuestListController�� ã�� ���߽��ϴ�. UI ������ �Ұ����� �� �ֽ��ϴ�.");
    }

    // ��������ο� ��ȭ �� ���� (����Ʈ ����)
    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;
            isAccepted = true;

            Debug.Log($"[����Ʈ ����] {quest.questName} - {receiverNPC}���� {itemName} �����ϼ���.");

            // ���� �ڹ��� ���� (�ִٸ�)
            questListController?.UnlockQuestSlot(quest.questName);

            // ����Ʈ ���� �� ������ 1�� ����
            Reward startReward = new Reward { itemname = itemName, quantity = 1 };
            RewardManager.Instance.GiveReward(startReward);
        }
    }

    // ������ڿ� ��ȭ �� ���� (����Ʈ �Ϸ� üũ)
    public void DeliverItem(string deliveredItemName, string npcName)
    {

        Debug.Log(isAccepted);
        if (quest.state != QuestState.InProgress)    // !isAccepted || 
        {
            Debug.Log("����Ʈ�� ���� ���� �ƴմϴ�.");
            return;
        }

        if (npcName != receiverNPC)
        {
            Debug.Log($"�߸��� NPC���� �����߽��ϴ�. ({npcName} �� {receiverNPC}���Ը� ����)");
            return;
        }

        // �κ��丮���� ������ ��������
        Item item = InventoryManager.Instance.GetItemByName(itemName);

        if (deliveredItemName == itemName && item != null && item.quantityInt >= 1)
        {
            // �κ��丮���� 1�� ���� (���߿� ����)
            //CountManager.Instance.RemoveItemByID(itemID, 1);

            QuestCompleted();
        }
        else
        {
            Debug.Log("������ �������� �����ϴ�!");
        }
    }


    private void QuestCompleted()
    {
        quest.state = QuestState.Completed;
        quest.canComplete = true;

        Debug.Log($"[����Ʈ �Ϸ�] {quest.questName}");

        RewardManager.Instance.GiveRewards(quest.reward);
    }
}
