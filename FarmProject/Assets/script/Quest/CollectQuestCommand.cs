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
