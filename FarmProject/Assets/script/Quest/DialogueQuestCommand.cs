using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestCommand : IQuestCommand
{
    private Quest quest;
    private string npcName;
    private bool isQuestStarted;

    public DialogueQuestCommand(Quest quest, string npcName)
    {
        this.quest = quest;
        this.npcName = npcName;
        this.isQuestStarted = false;
    }

    public void Execute()
    {
        // ��ȭ ����
        Debug.Log($"{npcName}�� ��ȭ ����: {quest.questName} ����Ʈ");
        isQuestStarted = true;
    }

    // ��ȭ �� ����Ʈ �Ϸ� üũ
    public void CompleteQuest()
    {
        if (isQuestStarted)
        {
            Debug.Log($"{npcName}���� ��ȭ �Ϸ�: {quest.questName} ����Ʈ �Ϸ�!");
            // ���� ó�� �� �߰�
        }
        else
        {
            Debug.Log($"{npcName}�� ��ȭ�� �����ؾ� ����Ʈ�� ����˴ϴ�.");
        }
    }

    //// ��ȭ�� ����Ʈ ����
    //public void StartDialogueQuest()
    //{
    //    Debug.Log($"{npcName}�� ��ȭ ����!");
    //    QuestManager.Instance.StartDialogueQuest(4);  // ����Ʈ ID 2�� ����
    //}
}
