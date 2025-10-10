using UnityEngine;

public class ConstructionQuestCommand : IQuestCommand
{
    public Quest Quest { get; private set; }
    public string BuildingName { get; private set; }
    private QuestListController questListController;

    public ConstructionQuestCommand(Quest quest, string buildingName, QuestListController controller)
    {
        Quest = quest;
        BuildingName = buildingName;
        questListController = controller;
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            questListController?.UnlockQuestSlot(Quest.questName);
            Debug.Log($"[건설형 퀘스트 시작] {Quest.questName}, 건물: {BuildingName}");
        }
        else
        {
            Debug.Log($"[건설형 퀘스트 실행] {Quest.questName}, 현재 상태: {Quest.state}");
        }
    }

    public bool CanComplete()
    {
        // BuildingName 태그를 가진 오브젝트가 씬에 있는지 체크
        var buildings = GameObject.FindGameObjectsWithTag(BuildingName);
        return buildings.Length > 0;
    }

    public void CompleteQuest()
    {
        if (!CanComplete())
        {
            Debug.LogWarning($"퀘스트 완료 불가: {Quest.questName}, 건물 {BuildingName} 아직 없음");
            return;
        }

        Quest.state = QuestState.Completed;
        RewardManager.Instance.GiveRewards(Quest.reward);
        Debug.Log($"[퀘스트 완료] {Quest.questName}");
    }

    public void Undo()
    {
        Debug.Log($"[퀘스트 되돌리기] {Quest.questName}");
        Quest.state = QuestState.NotStarted;
        // 이동형이면 목표 위치 초기화 필요하면 여기에 추가
    }
}
