using System.Collections.Generic;
using UnityEngine;

public class CollectQuestCommand : IQuestCommand
{
    private Quest quest;
    private string itemName;
    private int requiredAmount;
    private int collectedAmount;

    public CollectQuestCommand(Quest quest, string itemName, int requiredAmount)
    {
        this.quest = quest;
        this.itemName = itemName;
        this.requiredAmount = requiredAmount;
        this.collectedAmount = 0;
    }

    // ����Ʈ ����
    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;  // ���� ���·� ����
            Debug.Log($"{quest.questName} ����Ʈ ����: {itemName} {requiredAmount}�� ����");
        }
    }

    public void CollectItem(string collectedItemName)
    {
        if (quest.state != QuestState.InProgress)
        {
            Debug.Log($"{quest.questName} ����Ʈ�� ���� ���� �ƴմϴ�.");
            return;
        }

        if (collectedItemName == itemName)
        {
            collectedAmount++;
            Debug.Log($"{itemName} {collectedAmount}/{requiredAmount}�� ������");

            if (collectedAmount >= requiredAmount)
            {
                QuestCompleted();
            }
        }
        else
        {
            Debug.Log($"�߸��� ������: {collectedItemName}");
        }
    }

    // ��ư Ŭ�� �� ȣ��: ������ + ������ ����ϸ� �Ϸ�,  QuestInteractionManager ��ũ��Ʈ���� ���
    // QuestInteractionManager ��ũ��Ʈ ����
    //public void TryCompleteQuest(Quest quest)
    //{
    //    // ���� ���� �������� -> ���� ó��
    //    //if (quest.state == QuestState.NotStarted)
    //    //{
    //    //    quest.state = QuestState.InProgress;
    //    //    Debug.Log($"{quest.questName} ����Ʈ�� �����߽��ϴ�!");
    //    //    return;
    //    //}

    //    // ���� ���̸� -> �Ϸ� ���� Ȯ��
    //    if (quest.state == QuestState.InProgress)
    //    {
    //        // �κ��丮���� ������ ��������
    //        Item item = InventoryManager.Instance.GetItemByID(itemName);
    //        int playerItemCount = 0;

    //        if (item != null)
    //        {
    //            int.TryParse(item.quantity, out playerItemCount);
    //        }

    //        if (playerItemCount >= requiredAmount)
    //        {
    //            QuestCompleted();
    //        }
    //        else
    //        {
    //            Debug.Log($"{itemName} �������� �����մϴ�. ����: {playerItemCount}/{requiredAmount}");
    //        }
    //        return;
    //    }

    //    // �̹� �Ϸ�� ����Ʈ��
    //    if (quest.state == QuestState.Completed)
    //    {
    //        Debug.Log($"{quest.questName} ����Ʈ�� �̹� �Ϸ�ƽ��ϴ�.");
    //    }
    //}



    private void QuestCompleted()
    {
        quest.state = QuestState.Completed;  // �Ϸ� ���·� ����
        Debug.Log($"{quest.questName} ����Ʈ �Ϸ�!");

        // ���� ó��
        foreach (var reward in quest.reward)
        {
            Debug.Log($"���� ����: {reward.icon} ������ ID {reward.itemID}");
            // ���� ���� ���� ���� ����
        }
    }
}
