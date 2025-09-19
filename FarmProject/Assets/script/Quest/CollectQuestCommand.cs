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
