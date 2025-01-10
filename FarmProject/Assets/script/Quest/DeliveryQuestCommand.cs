using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryQuestCommand : IQuestCommand
{
    private Quest quest;
    private string itemName;  // ������ ���� �̸�
    private int requiredAmount;
    private string receiverNPC;

    public DeliveryQuestCommand(Quest quest, string itemName, int requiredAmount, string receiverNPC)
    {
        this.quest = quest;
        this.itemName = itemName;
        this.requiredAmount = requiredAmount;
        this.receiverNPC = receiverNPC;
    }

    // ����Ʈ ����
    public void Execute()
    {
        Debug.Log($"{quest.questName} ����Ʈ ����: {itemName} {requiredAmount}�� ����");
    }

    // ������ �����ϴ� �޼���
    public void DeliverItem(string deliveredItemName)
    {
        if (deliveredItemName == itemName)
        {
            Debug.Log($"{itemName} ���� �Ϸ�!");

            // ����Ʈ �Ϸ� ó��
            QuestCompleted();
        }
        else
        {
            Debug.Log("�߸��� ���� ����!");
        }
    }

    // ������ �̸��� ��ȯ�ϴ� �޼��� (�߰�)
    public string GetItemName()
    {
        return itemName;
    }

    // ����Ʈ �Ϸ� ó��
    private void QuestCompleted()
    {
        Debug.Log($"{quest.questName} ����Ʈ �Ϸ�!");

        // ���� ���� �� ����Ʈ �Ϸ� ����
    }
}
