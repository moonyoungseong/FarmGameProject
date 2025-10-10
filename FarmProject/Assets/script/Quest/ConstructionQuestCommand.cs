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
            Debug.Log($"[�Ǽ��� ����Ʈ ����] {Quest.questName}, �ǹ�: {BuildingName}");
        }
        else
        {
            Debug.Log($"[�Ǽ��� ����Ʈ ����] {Quest.questName}, ���� ����: {Quest.state}");
        }
    }

    public bool CanComplete()
    {
        // BuildingName �±׸� ���� ������Ʈ�� ���� �ִ��� üũ
        var buildings = GameObject.FindGameObjectsWithTag(BuildingName);
        return buildings.Length > 0;
    }

    public void CompleteQuest()
    {
        if (!CanComplete())
        {
            Debug.LogWarning($"����Ʈ �Ϸ� �Ұ�: {Quest.questName}, �ǹ� {BuildingName} ���� ����");
            return;
        }

        Quest.state = QuestState.Completed;
        RewardManager.Instance.GiveRewards(Quest.reward);
        Debug.Log($"[����Ʈ �Ϸ�] {Quest.questName}");
    }

    public void Undo()
    {
        Debug.Log($"[����Ʈ �ǵ�����] {Quest.questName}");
        Quest.state = QuestState.NotStarted;
        // �̵����̸� ��ǥ ��ġ �ʱ�ȭ �ʿ��ϸ� ���⿡ �߰�
    }
}
