using System.Collections.Generic;
using UnityEngine;

public class CollectQuestCommand : IQuestCommand
{
    private Quest quest;
    private string itemName;
    private int requiredAmount;
    private int collectedAmount;

    public CollectQuestCommand(Quest quest, string itemName, int requiredAmount)
    {
        this.quest = quest;
        this.itemName = itemName;
        this.requiredAmount = requiredAmount;
        this.collectedAmount = 0;
    }

    // 퀘스트 시작
    public void Execute()
    {
        if (quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;  // 진행 상태로 변경
            Debug.Log($"{quest.questName} 퀘스트 시작: {itemName} {requiredAmount}개 수집");
        }
    }

    public void CollectItem(string collectedItemName)
    {
        if (quest.state != QuestState.InProgress)
        {
            Debug.Log($"{quest.questName} 퀘스트가 진행 중이 아닙니다.");
            return;
        }

        if (collectedItemName == itemName)
        {
            collectedAmount++;
            Debug.Log($"{itemName} {collectedAmount}/{requiredAmount}개 수집됨");

            if (collectedAmount >= requiredAmount)
            {
                QuestCompleted();
            }
        }
        else
        {
            Debug.Log($"잘못된 아이템: {collectedItemName}");
        }
    }

    // 버튼 클릭 시 호출: 진행중 + 아이템 충분하면 완료,  QuestInteractionManager 스크립트에서 사용
    // QuestInteractionManager 스크립트 내부
    //public void TryCompleteQuest(Quest quest)
    //{
    //    // 아직 시작 안했으면 -> 시작 처리
    //    //if (quest.state == QuestState.NotStarted)
    //    //{
    //    //    quest.state = QuestState.InProgress;
    //    //    Debug.Log($"{quest.questName} 퀘스트를 시작했습니다!");
    //    //    return;
    //    //}

    //    // 진행 중이면 -> 완료 조건 확인
    //    if (quest.state == QuestState.InProgress)
    //    {
    //        // 인벤토리에서 아이템 가져오기
    //        Item item = InventoryManager.Instance.GetItemByID(itemName);
    //        int playerItemCount = 0;

    //        if (item != null)
    //        {
    //            int.TryParse(item.quantity, out playerItemCount);
    //        }

    //        if (playerItemCount >= requiredAmount)
    //        {
    //            QuestCompleted();
    //        }
    //        else
    //        {
    //            Debug.Log($"{itemName} 아이템이 부족합니다. 현재: {playerItemCount}/{requiredAmount}");
    //        }
    //        return;
    //    }

    //    // 이미 완료된 퀘스트면
    //    if (quest.state == QuestState.Completed)
    //    {
    //        Debug.Log($"{quest.questName} 퀘스트는 이미 완료됐습니다.");
    //    }
    //}



    private void QuestCompleted()
    {
        quest.state = QuestState.Completed;  // 완료 상태로 변경
        Debug.Log($"{quest.questName} 퀘스트 완료!");

        // 보상 처리
        foreach (var reward in quest.reward)
        {
            Debug.Log($"보상 지급: {reward.icon} 아이템 ID {reward.itemID}");
            // 실제 보상 로직 연결 가능
        }
    }
}
