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
    public bool IsQuestStarted => isQuestStarted;  // 외부에서 대화 진행 여부를 확인할 수 있게 수정

    public DialogueQuestCommand(Quest quest, string npcName)
    {
        this.quest = quest;
        this.npcName = npcName;
        this.isQuestStarted = false;
        this.isQuestCompleted = false;
    }

    public void Execute()
    {
        if (!isQuestStarted)
        {
            Debug.Log($"{npcName}와 대화 시작: {quest.questName} 퀘스트");
            isQuestStarted = true;
        }
    }

    public void CompleteQuest()
    {
        if (isQuestStarted && !isQuestCompleted)
        {
            Debug.Log($"{npcName}와의 대화 완료: {quest.questName} 퀘스트 완료!");
            isQuestCompleted = true;
            GiveRewards();
        }
        else if (isQuestCompleted)
        {
            Debug.Log($"{npcName}와의 대화가 이미 완료되었습니다.");
        }
        else
        {
            Debug.Log($"{npcName}와 대화를 시작해야 퀘스트가 진행됩니다.");
        }
    }

    private void GiveRewards()
    {
        foreach (var reward in quest.reward)
        {
            // 보상 지급 로직 추가
        }
    }
}


