using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestCommand : IQuestCommand
{
    private Quest quest;
    private string npcName;

    // UI�� �ܺο��� quest�� ������ �� �ְ� ����
    public Quest Quest => quest;

    public string NpcName => npcName;
    public bool IsQuestStarted => quest.state == QuestState.InProgress || quest.state == QuestState.Completed;

    public DialogueQuestCommand(Quest quest, string npcName)
    {
        this.quest = quest;
        this.npcName = npcName;
    }

    // ����Ʈ ����
    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            Debug.Log($"{npcName}�� ��ȭ ����: {quest.questName} ����Ʈ");
            quest.state = QuestState.InProgress;

            // UI ����
            //UpdateQuestUI();
        }
    }

    // ����Ʈ �Ϸ�
    public void CompleteQuest()
    {
        if (quest.state == QuestState.InProgress)
        {
            quest.state = QuestState.Completed;
            Debug.Log($"{npcName}���� ��ȭ �Ϸ�: {quest.questName} ����Ʈ �Ϸ�!");

            //UpdateQuestUI();
            GiveRewards();
        }
        else if (quest.state == QuestState.Completed)
        {
            Debug.Log($"{quest.questName} ����Ʈ�� �̹� �Ϸ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log($"{npcName}�� ��ȭ�� �����ؾ� ����Ʈ�� ����˴ϴ�.");
        }
    }

    //private void UpdateQuestUI()
    //{
    //    if (QuestStateManager.Instance != null)
    //    {
    //        // ���⼭ �ٷ� QuestStateManager�� UI ����
    //        QuestStateManager.Instance.ShowQuestDetail(quest);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("QuestStateManager.Instance�� ���� �������� �ʽ��ϴ�!");
    //    }
    //}

    private void GiveRewards()
    {
        if (RewardManager.Instance != null && quest.reward != null)
        {
            RewardManager.Instance.GiveRewards(quest.reward);
        }
    }
}
