using UnityEngine;

public class MovementQuestCommand : IQuestCommand
{
    public Quest Quest { get; private set; }
    public string MovementTarget { get; private set; }
    private QuestListController questListController;

    // 중복 완료 방지
    private bool hasArrived = false;

    public MovementQuestCommand(Quest quest, string target, QuestListController controller)
    {
        Quest = quest;
        MovementTarget = target;
        questListController = controller;
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            questListController?.UnlockQuestSlot(Quest.questName);

            Debug.Log($"[이동형 퀘스트 시작] {Quest.questName}, 목표 위치: {MovementTarget}");
        }
        else
        {
            Debug.Log($"[이동형 퀘스트 실행] {Quest.questName}, 현재 상태: {Quest.state}");
        }
    }

    public bool SetArrived(bool arrived)
    {
        hasArrived = arrived;
        Debug.Log($"[{Quest.questName}] 완료 상태 변경: {hasArrived}");
        return arrived; // arrived 값 그대로 반환
    }

    public bool IsArrived() //=> hasArrived;
    {
        if (hasArrived)  // SetArrived(true)가 호출되면 hasArrived가 true
            return true;
        else
            return false;
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
        hasArrived = false;
        Quest.state = QuestState.NotStarted;
        // 필요하다면 목표 위치 초기화
    }
}
