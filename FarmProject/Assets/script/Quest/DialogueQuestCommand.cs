using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestCommand : IQuestCommand
{
    private Quest quest;
    private string npcName;
    private bool isQuestStarted;
    private bool isQuestCompleted;

    public string NpcName => npcName;
    public bool IsQuestStarted => isQuestStarted;  // �ܺο��� ��ȭ ���� ���θ� Ȯ���� �� �ְ� ����

    public DialogueQuestCommand(Quest quest, string npcName)    // ��ȭ�� ����Ʈ ����
    {
        this.quest = quest;
        this.npcName = npcName;
        this.isQuestStarted = false;
        this.isQuestCompleted = false;
    }

    public void Execute()   // ����Ʈ ����
    {
        if (!isQuestStarted)
        {
            Debug.Log($"{npcName}�� ��ȭ ����: {quest.questName} ����Ʈ");
            isQuestStarted = true;
        }
    }

    public void CompleteQuest()     // ��ȭ, ���� �Ϸ�
    {
        if (isQuestStarted && !isQuestCompleted)
        {
            Debug.Log($"{npcName}���� ��ȭ �Ϸ�: {quest.questName} ����Ʈ �Ϸ�!");
            isQuestCompleted = true;
            GiveRewards();
        }
        else if (isQuestCompleted)
        {
            Debug.Log($"{npcName}���� ��ȭ�� �̹� �Ϸ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log($"{npcName}�� ��ȭ�� �����ؾ� ����Ʈ�� ����˴ϴ�.");
        }
    }

    //private void GiveRewards()  // ���� ����Ʈ
    //{
    //    Debug.Log("�������� ��, �Ҽ���, ���̽�ũ���� ���޵Ǿ���."); // �׽�Ʈ�� �ּ� , �������� ġ��, ü��, ġŲ 34,35,36
    //    //foreach (var reward in quest.reward)
    //    //{
    //    //    // ���� ���� ���� �߰�
    //    //}
    //}

    private void GiveRewards()
    {
        if (quest.reward != null && quest.reward.Count > 0)
        {
            CountManager countManager = GameObject.FindObjectOfType<CountManager>();
            if (countManager == null)
            {
                Debug.LogWarning("CountManager�� ã�� �� �����ϴ�!");
                return;
            }

            foreach (var reward in quest.reward)
            {
                // UI �Է� ����
                countManager.ItemNameInput.text = reward.itemname;
                countManager.ItemNumberInput.text = "1";

                // GetItemClick ȣ��
                countManager.GetItemClick();

                Debug.Log($"���� ����: ������ �̸�={reward.itemname}, ����=1");
            }
        }
        else
        {
            Debug.LogWarning($"{quest.questName} ����Ʈ���� ������ �����ϴ�.");
        }
    }

}


