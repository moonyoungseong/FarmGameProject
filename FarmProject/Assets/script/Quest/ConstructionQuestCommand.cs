using UnityEngine;

public class ConstructionQuestCommand : IQuestCommand
{
    private Quest quest;                  // 퀘스트 정보
    private string buildingName;          // 지어야 할 건물 이름
    private bool isConstructed = false;   // 건설 여부

    private QuestListController questListController;

    public string BuildingName => buildingName; // 외부에서 건물 이름 확인 가능

    public ConstructionQuestCommand(Quest quest, string buildingName)
    {
        this.quest = quest;
        this.buildingName = buildingName;
    }

    // 퀘스트 시작
    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;
            Debug.Log($"[퀘스트 시작] {quest.questName} - {buildingName} 건설 시작");
        }

        // 슬롯 자물쇠 해제
        if (questListController != null)
            questListController.UnlockQuestSlot(quest.questName);
    }

    public bool CheckConstruction()
    {
        GameObject building = GameObject.FindWithTag("Dog");
        if (building != null)
        {
            //CompleteQuest(); // 퀘스트 완료 처리
            return true;     //  건물 존재 → true
        }
        return false;        //  건물 없음 → false
    }

    // 퀘스트 완료 처리
    public void CompleteQuest()
    {
        quest.state = QuestState.Completed;
        Debug.Log($"[퀘스트 완료] {quest.questName} - {buildingName} 건설 완료!");

        RewardManager.Instance.GiveRewards(quest.reward);
    }
}
