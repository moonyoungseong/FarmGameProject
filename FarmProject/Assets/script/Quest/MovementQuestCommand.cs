using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementQuestCommand : IQuestCommand
{
    private Quest quest;  // ����Ʈ ����
    private string targetNPCName;  // ��ǥ NPC �̸�
    private bool isCompleted;  // ����Ʈ �Ϸ� ����

    // �����ڿ��� ����Ʈ�� ��ǥ NPC �̸��� ����
    public MovementQuestCommand(Quest quest, string targetNPCName)
    {
        this.quest = quest;
        this.targetNPCName = targetNPCName;
        this.isCompleted = false;  // ���� �� ����Ʈ �Ϸ���� ����
    }

    // ����Ʈ ���� �޼���
    public void Execute()
    {
        Debug.Log($"{quest.questName} ����Ʈ ����: {targetNPCName}���� �̵�");
        // ����Ʈ ���� �޽��� �� �߰� ������ �ۼ��� �� ����
    }

    // Ʈ���� ������ ���� �� ȣ��Ǵ� �޼��� (�� �޼���� Ʈ���� ������ ���� ������Ʈ���� ȣ��)
    public void OnTriggerEnter(Collider other)
    {
        if (other != null && !isCompleted && other.CompareTag("Player"))
        {
            Debug.Log($"{quest.questName} ����Ʈ �Ϸ�! {targetNPCName}���� ����");
            QuestCompleted();
        }
        else
        {
            Debug.Log("Ʈ���ſ� ��ȿ���� ���� ��ü�� ����");
        }
    }

    // ����Ʈ �Ϸ� ó�� �޼���
    private void QuestCompleted()
    {
        isCompleted = true;
        Debug.Log($"{quest.questName} ����Ʈ �Ϸ�!");

        // ���� ó�� �� ����Ʈ �Ϸ� �� ó��
        foreach (var reward in quest.reward)
        {
            Debug.Log($"���� ����: {reward.icon} ������ ID {reward.itemID}");
            // �߰� ���� ó�� ����
        }
    }
}
