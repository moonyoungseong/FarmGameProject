using UnityEngine;

public class DeliveryQuestCommand : IQuestCommand
{
    private Quest quest;
    private string itemName;
    private string receiverNPC;
    private bool isAccepted = false;

    private QuestListController questListController;

    public DeliveryQuestCommand(Quest quest, string itemName, string receiverNPC)
    {
        this.quest = quest;
        this.itemName = itemName;
        this.receiverNPC = receiverNPC;

        // QuestListController 자동 주입 (있다면)
        questListController = GameObject.FindObjectOfType<QuestListController>();
        if (questListController == null)
            Debug.LogWarning("QuestListController를 찾지 못했습니다. UI 갱신이 불가능할 수 있습니다.");
    }

    // 옥수수농부와 대화 시 실행 (퀘스트 수락)
    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;
            isAccepted = true;

            Debug.Log($"[퀘스트 시작] {quest.questName} - {receiverNPC}에게 {itemName} 전달하세요.");

            // 슬롯 자물쇠 해제 (있다면)
            questListController?.UnlockQuestSlot(quest.questName);

            // 퀘스트 시작 시 옥수수 1개 지급
            Reward startReward = new Reward { itemname = itemName, quantity = 1 };
            RewardManager.Instance.GiveReward(startReward);
        }
    }

    // 양봉업자와 대화 시 실행 (퀘스트 완료 체크)
    public void DeliverItem(string deliveredItemName, string npcName)
    {

        Debug.Log(isAccepted);
        if (quest.state != QuestState.InProgress)    // !isAccepted || 
        {
            Debug.Log("퀘스트가 진행 중이 아닙니다.");
            return;
        }

        if (npcName != receiverNPC)
        {
            Debug.Log($"잘못된 NPC에게 전달했습니다. ({npcName} → {receiverNPC}에게만 가능)");
            return;
        }

        // 인벤토리에서 아이템 가져오기
        Item item = InventoryManager.Instance.GetItemByName(itemName);

        if (deliveredItemName == itemName && item != null && item.quantityInt >= 1)
        {
            // 인벤토리에서 1개 차감 (나중에 구현)
            //CountManager.Instance.RemoveItemByID(itemID, 1);

            QuestCompleted();
        }
        else
        {
            Debug.Log("전달할 아이템이 없습니다!");
        }
    }


    private void QuestCompleted()
    {
        quest.state = QuestState.Completed;
        quest.canComplete = true;

        Debug.Log($"[퀘스트 완료] {quest.questName}");

        RewardManager.Instance.GiveRewards(quest.reward);
    }
}
