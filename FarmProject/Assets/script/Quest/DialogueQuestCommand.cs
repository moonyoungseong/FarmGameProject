using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestCommand : IQuestCommand
{
    private Quest quest;
    private string npcName;

    // UI나 외부에서 quest를 참조할 수 있게 공개
    public Quest Quest => quest;

    public string NpcName => npcName;
    public bool IsQuestStarted => quest.state == QuestState.InProgress || quest.state == QuestState.Completed;

    public DialogueQuestCommand(Quest quest, string npcName)
    {
        this.quest = quest;
        this.npcName = npcName;
    }

    // 퀘스트 시작
    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            Debug.Log($"{npcName}와 대화 시작: {quest.questName} 퀘스트");
            quest.state = QuestState.InProgress;

            // UI 갱신
            //UpdateQuestUI();
        }
    }

    // 퀘스트 완료
    public void CompleteQuest()
    {
        if (quest.state == QuestState.InProgress)
        {
            quest.state = QuestState.Completed;
            Debug.Log($"{npcName}와의 대화 완료: {quest.questName} 퀘스트 완료!");

            //UpdateQuestUI();
            GiveRewards();
        }
        else if (quest.state == QuestState.Completed)
        {
            Debug.Log($"{quest.questName} 퀘스트는 이미 완료되었습니다.");
        }
        else
        {
            Debug.Log($"{npcName}와 대화를 시작해야 퀘스트가 진행됩니다.");
        }
    }

    //private void UpdateQuestUI()
    //{
    //    if (QuestStateManager.Instance != null)
    //    {
    //        // 여기서 바로 QuestStateManager로 UI 갱신
    //        QuestStateManager.Instance.ShowQuestDetail(quest);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("QuestStateManager.Instance가 씬에 존재하지 않습니다!");
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
