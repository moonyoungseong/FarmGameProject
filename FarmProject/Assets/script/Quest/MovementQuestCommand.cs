using UnityEngine;

public class MovementQuestCommand : IQuestCommand
{
    private Quest quest;
    private string targetLocationName;
    private bool hasArrived = false;

    private QuestListController questListController;

    public MovementQuestCommand(Quest quest, string targetLocationName)
    {
        this.quest = quest;
        this.targetLocationName = targetLocationName;

        // QuestListController 자동 주입 (있다면)
        questListController = GameObject.FindObjectOfType<QuestListController>();
        if (questListController == null)
            Debug.LogWarning("QuestListController를 찾지 못했습니다. UI 갱신이 불가능할 수 있습니다.");
    }

    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;
            Debug.Log($"[퀘스트 시작] {quest.questName} - {targetLocationName}로 이동하세요.");
        }

        // 슬롯 자물쇠 해제 (있다면)
        questListController?.UnlockQuestSlot(quest.questName);
    }

    //  외부에서 호출 가능 (예: FixWind() 끝났을 때)
    public bool ReachTarget()
    {
        if (quest.state == QuestState.InProgress && !hasArrived)
        {
            hasArrived = true;
            quest.canComplete = true;  // 완료 가능 표시
            CompleteQuest();
            return true;
        }
        return false;
    }

    private void CompleteQuest()
    {
        quest.state = QuestState.Completed;
        Debug.Log($"[퀘스트 완료] {quest.questName} - {targetLocationName} 도착 완료!");

        RewardManager.Instance.GiveRewards(quest.reward);
    }
}
