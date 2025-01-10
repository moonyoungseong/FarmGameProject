using System.Collections.Generic; // List ����� ���� ���ӽ����̽�
using UnityEngine;

public class CollectQuestCommand : IQuestCommand
{
    private Quest quest;  // ����Ʈ ����
    private string itemName;  // ������ ������ �̸�
    private int requiredAmount;  // ��ǥ ����
    private int collectedAmount;  // ���� ������ ��

    // �����ڿ��� ����Ʈ, ������ �̸�, �����ؾ� �� ���� ����
    public CollectQuestCommand(Quest quest, string itemName, int requiredAmount)
    {
        this.quest = quest;
        this.itemName = itemName;
        this.requiredAmount = requiredAmount;
        this.collectedAmount = 0;  // ���� �� �������� 0
    }

    // ����Ʈ ���� �޼��� (����Ʈ ���� �� ȣ��)
    public void Execute()
    {
        Debug.Log($"{quest.questName} ����Ʈ ����: {itemName} {requiredAmount}�� ����");
    }

    // �������� �����ϴ� �޼���
    public void CollectItem(string collectedItemName)
    {
        if (collectedItemName == itemName)
        {
            collectedAmount++;  // ������ ����
            Debug.Log($"{itemName} {collectedAmount}�� ������");

            if (collectedAmount >= requiredAmount)  // ��ǥ ���� �޼� �� ����Ʈ �Ϸ�
            {
                QuestCompleted();
            }
        }
        else
        {
            Debug.Log($"�߸��� ������: {collectedItemName}");
        }
    }

    // ����Ʈ �Ϸ� ó�� �޼���
    private void QuestCompleted()
    {
        Debug.Log($"{quest.questName} ����Ʈ �Ϸ�!");  // ����Ʈ �Ϸ� �޽��� ���

        // ���� ó��
        foreach (var reward in quest.reward)
        {
            // ���� ������ ���� ó�� (�������� �����ϴ� ������ ���� ó�� �κп��� ����)
            Debug.Log($"���� ����: {reward.icon} ������ ID {reward.itemID}");
            // ���� ������ ���޿� ���� �߰� ������ ���⿡ ����
        }

        // �߰��� ����Ʈ �Ϸ� ���¸� ����ϰų� UI�� ����Ʈ �Ϸ� ǥ�� ���� �۾��� �� �� �ֽ��ϴ�.
    }
}
