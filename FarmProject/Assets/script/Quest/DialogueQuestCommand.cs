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
            Debug.Log($"[��ȭ�� ����Ʈ ����] {Quest.questName}, NPC: {GiverNPC}");
        }
        else
        {
            Debug.Log($"[��ȭ�� ����Ʈ ����] {Quest.questName}, ���� ����: {Quest.state}");
        }
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
        Quest.state = QuestState.NotStarted;
        // �̵����̸� ��ǥ ��ġ �ʱ�ȭ �ʿ��ϸ� ���⿡ �߰�
    }
}
