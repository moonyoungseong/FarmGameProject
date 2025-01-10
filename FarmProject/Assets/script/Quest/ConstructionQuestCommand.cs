using System.Collections.Generic; // List ����� ���� ���ӽ����̽�
using UnityEngine;

public class ConstructionQuestCommand : IQuestCommand
{
    private Quest quest;  // ����Ʈ ����
    private string constructionType;  // �Ǽ��ؾ� �� ��ü ���� (��: ��, ������)
    private bool isConstructed;  // �Ǽ� �Ϸ� ����

    // �����ڿ��� ����Ʈ, �Ǽ��ؾ� �� ��ü, �ʱ� ���¸� ����
    public ConstructionQuestCommand(Quest quest, string constructionType)
    {
        this.quest = quest;
        this.constructionType = constructionType;
        this.isConstructed = false;  // ���� �� �Ǽ� �Ϸ���� ����
    }

    // ����Ʈ ���� �޼��� (����Ʈ ���� �� ȣ��)
    public void Execute()
    {
        Debug.Log($"{quest.questName} ����Ʈ ����: {constructionType} �Ǽ�");
    }

    // �Ǽ��� �����ϴ� �޼���
    public void ConstructBuilding()
    {
        if (!isConstructed)
        {
            // �Ǽ� �۾��� �Ϸ�Ǿ����� ��Ÿ���� ������ ���⿡ �߰�
            isConstructed = true;
            Debug.Log($"{constructionType} �Ǽ��� �Ϸ�Ǿ����ϴ�.");

            // ����Ʈ �Ϸ� ó��
            QuestCompleted();
        }
        else
        {
            Debug.Log($"{constructionType}�� �̹� �Ǽ��Ǿ����ϴ�.");
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
