using UnityEngine;

public class ConstructionQuestCommand : IQuestCommand
{
    private Quest quest;                  // ����Ʈ ����
    private string buildingName;          // ����� �� �ǹ� �̸�
    private bool isConstructed = false;   // �Ǽ� ����

    private QuestListController questListController;

    public string BuildingName => buildingName; // �ܺο��� �ǹ� �̸� Ȯ�� ����

    public ConstructionQuestCommand(Quest quest, string buildingName)
    {
        this.quest = quest;
        this.buildingName = buildingName;
    }

    // ����Ʈ ����
    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;
            Debug.Log($"[����Ʈ ����] {quest.questName} - {buildingName} �Ǽ� ����");
        }

        // ���� �ڹ��� ����
        if (questListController != null)
            questListController.UnlockQuestSlot(quest.questName);
    }

    public bool CheckConstruction()
    {
        GameObject building = GameObject.FindWithTag("Dog");
        if (building != null)
        {
            //CompleteQuest(); // ����Ʈ �Ϸ� ó��
            return true;     //  �ǹ� ���� �� true
        }
        return false;        //  �ǹ� ���� �� false
    }

    // ����Ʈ �Ϸ� ó��
    public void CompleteQuest()
    {
        quest.state = QuestState.Completed;
        Debug.Log($"[����Ʈ �Ϸ�] {quest.questName} - {buildingName} �Ǽ� �Ϸ�!");

        RewardManager.Instance.GiveRewards(quest.reward);
    }
}
