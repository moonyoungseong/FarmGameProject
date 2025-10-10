using UnityEngine;

public class MovementQuestCommand : IQuestCommand
{
    public Quest Quest { get; private set; }
    public string MovementTarget { get; private set; }
    private QuestListController questListController;

    // �ߺ� �Ϸ� ����
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

            Debug.Log($"[�̵��� ����Ʈ ����] {Quest.questName}, ��ǥ ��ġ: {MovementTarget}");
        }
        else
        {
            Debug.Log($"[�̵��� ����Ʈ ����] {Quest.questName}, ���� ����: {Quest.state}");
        }
    }

    public bool SetArrived(bool arrived)
    {
        hasArrived = arrived;
        Debug.Log($"[{Quest.questName}] �Ϸ� ���� ����: {hasArrived}");
        return arrived; // arrived �� �״�� ��ȯ
    }

    public bool IsArrived() //=> hasArrived;
    {
        if (hasArrived)  // SetArrived(true)�� ȣ��Ǹ� hasArrived�� true
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
            Debug.Log($"[����Ʈ �Ϸ�] {Quest.questName}");
        }
    }

    public void Undo()
    {
        Debug.Log($"[����Ʈ �ǵ�����] {Quest.questName}");
        hasArrived = false;
        Quest.state = QuestState.NotStarted;
        // �ʿ��ϴٸ� ��ǥ ��ġ �ʱ�ȭ
    }
}
