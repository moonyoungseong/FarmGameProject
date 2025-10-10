using UnityEngine;

public class DialogueQuestCommand : IQuestCommand
{
    public Quest Quest { get; private set; }
    public string GiverNPC { get; private set; }
    private QuestListController questListController;

    public DialogueQuestCommand(Quest quest, string giverNPC, QuestListController controller)
    {
        Quest = quest;
        GiverNPC = giverNPC;
        questListController = controller;
        questListController?.UnlockQuestSlot(Quest.questName);
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            Debug.Log($"[대화형 퀘스트 시작] {Quest.questName}, NPC: {GiverNPC}");
        }
        else
        {
            Debug.Log($"[대화형 퀘스트 실행] {Quest.questName}, 현재 상태: {Quest.state}");
        }
    }

    public void CompleteQuest()
    {
        if (Quest.state == QuestState.InProgress)
        {
            Quest.state = QuestState.Completed;
            RewardManager.Instance.GiveRewards(Quest.reward);
            Debug.Log($"[퀘스트 완료] {Quest.questName}");
        }
    }

    public void Undo()
    {
        Debug.Log($"[퀘스트 되돌리기] {Quest.questName}");
        Quest.state = QuestState.NotStarted;
        // 이동형이면 목표 위치 초기화 필요하면 여기에 추가
    }
}
