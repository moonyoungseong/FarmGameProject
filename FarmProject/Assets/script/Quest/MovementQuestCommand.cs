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

        // QuestListController �ڵ� ���� (�ִٸ�)
        questListController = GameObject.FindObjectOfType<QuestListController>();
        if (questListController == null)
            Debug.LogWarning("QuestListController�� ã�� ���߽��ϴ�. UI ������ �Ұ����� �� �ֽ��ϴ�.");
    }

    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;
            Debug.Log($"[����Ʈ ����] {quest.questName} - {targetLocationName}�� �̵��ϼ���.");
        }

        // ���� �ڹ��� ���� (�ִٸ�)
        questListController?.UnlockQuestSlot(quest.questName);
    }

    //  �ܺο��� ȣ�� ���� (��: FixWind() ������ ��)
    public bool ReachTarget()
    {
        if (quest.state == QuestState.InProgress && !hasArrived)
        {
            hasArrived = true;
            quest.canComplete = true;  // �Ϸ� ���� ǥ��
            CompleteQuest();
            return true;
        }
        return false;
    }

    private void CompleteQuest()
    {
        quest.state = QuestState.Completed;
        Debug.Log($"[����Ʈ �Ϸ�] {quest.questName} - {targetLocationName} ���� �Ϸ�!");

        RewardManager.Instance.GiveRewards(quest.reward);
    }
}
