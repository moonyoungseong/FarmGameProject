using UnityEngine;

public class DeliveryQuestCommand : IQuestCommand
{
    public Quest Quest { get; private set; }
    public string ItemName { get; private set; }
    public string ReceiverNPC { get; private set; }
    private QuestListController questListController;

    public DeliveryQuestCommand(Quest quest, string itemName, string receiverNPC, QuestListController controller)
    {
        Quest = quest;
        ItemName = itemName;
        ReceiverNPC = receiverNPC;
        questListController = controller;
    }

    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            questListController?.UnlockQuestSlot(Quest.questName);

            Debug.Log($"[전달형 퀘스트 시작] {Quest.questName}, 아이템: {ItemName}, 수령인: {ReceiverNPC}");
        }
        else
        {
            Debug.Log($"[전달형 퀘스트 실행] {Quest.questName}, 현재 상태: {Quest.state}");
        }
    }

    public void DeliverItem(string itemName, string npcName)
    {
        if (Quest.state != QuestState.InProgress) return;
        if (itemName != ItemName || npcName != ReceiverNPC) return;

        var item = InventoryManager.Instance.GetItemByName(ItemName);
        if (item != null)
        {
            CountManager.Instance.RemoveItemByID(item.itemID, 1);
            Debug.Log($"[{Quest.questName}] {ReceiverNPC}에게 {ItemName} 전달 완료");
        }

        CompleteQuest();
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
