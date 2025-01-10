using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryQuestCommand : IQuestCommand
{
    private Quest quest;
    private string itemName;  // 전달할 물건 이름
    private int requiredAmount;
    private string receiverNPC;

    public DeliveryQuestCommand(Quest quest, string itemName, int requiredAmount, string receiverNPC)
    {
        this.quest = quest;
        this.itemName = itemName;
        this.requiredAmount = requiredAmount;
        this.receiverNPC = receiverNPC;
    }

    // 퀘스트 실행
    public void Execute()
    {
        Debug.Log($"{quest.questName} 퀘스트 시작: {itemName} {requiredAmount}개 전달");
    }

    // 물건을 전달하는 메서드
    public void DeliverItem(string deliveredItemName)
    {
        if (deliveredItemName == itemName)
        {
            Debug.Log($"{itemName} 전달 완료!");

            // 퀘스트 완료 처리
            QuestCompleted();
        }
        else
        {
            Debug.Log("잘못된 물건 전달!");
        }
    }

    // 아이템 이름을 반환하는 메서드 (추가)
    public string GetItemName()
    {
        return itemName;
    }

    // 퀘스트 완료 처리
    private void QuestCompleted()
    {
        Debug.Log($"{quest.questName} 퀘스트 완료!");

        // 보상 지급 등 퀘스트 완료 로직
    }
}
