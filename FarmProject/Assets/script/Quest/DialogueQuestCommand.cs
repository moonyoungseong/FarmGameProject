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
        // 대화 시작
        Debug.Log($"{npcName}와 대화 시작: {quest.questName} 퀘스트");
        isQuestStarted = true;
    }

    // 대화 후 퀘스트 완료 체크
    public void CompleteQuest()
    {
        if (isQuestStarted)
        {
            Debug.Log($"{npcName}와의 대화 완료: {quest.questName} 퀘스트 완료!");
            // 보상 처리 등 추가
        }
        else
        {
            Debug.Log($"{npcName}와 대화를 시작해야 퀘스트가 진행됩니다.");
        }
    }

    //// 대화형 퀘스트 실행
    //public void StartDialogueQuest()
    //{
    //    Debug.Log($"{npcName}와 대화 시작!");
    //    QuestManager.Instance.StartDialogueQuest(4);  // 퀘스트 ID 2번 실행
    //}
}
