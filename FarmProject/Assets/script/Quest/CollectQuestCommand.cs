using System.Collections.Generic; // List 사용을 위한 네임스페이스
using UnityEngine;

public class CollectQuestCommand : IQuestCommand
{
    private Quest quest;  // 퀘스트 정보
    private string itemName;  // 수집할 아이템 이름
    private int requiredAmount;  // 목표 수량
    private int collectedAmount;  // 현재 수집된 양

    // 생성자에서 퀘스트, 아이템 이름, 수집해야 할 양을 받음
    public CollectQuestCommand(Quest quest, string itemName, int requiredAmount)
    {
        this.quest = quest;
        this.itemName = itemName;
        this.requiredAmount = requiredAmount;
        this.collectedAmount = 0;  // 시작 시 수집량은 0
    }

    // 퀘스트 실행 메서드 (퀘스트 시작 시 호출)
    public void Execute()
    {
        Debug.Log($"{quest.questName} 퀘스트 시작: {itemName} {requiredAmount}개 수집");
    }

    // 아이템을 수집하는 메서드
    public void CollectItem(string collectedItemName)
    {
        if (collectedItemName == itemName)
        {
            collectedAmount++;  // 수집량 증가
            Debug.Log($"{itemName} {collectedAmount}개 수집됨");

            if (collectedAmount >= requiredAmount)  // 목표 수량 달성 시 퀘스트 완료
            {
                QuestCompleted();
            }
        }
        else
        {
            Debug.Log($"잘못된 아이템: {collectedItemName}");
        }
    }

    // 퀘스트 완료 처리 메서드
    private void QuestCompleted()
    {
        Debug.Log($"{quest.questName} 퀘스트 완료!");  // 퀘스트 완료 메시지 출력

        // 보상 처리
        foreach (var reward in quest.reward)
        {
            // 보상 아이템 지급 처리 (아이템을 지급하는 로직은 보상 처리 부분에서 구현)
            Debug.Log($"보상 지급: {reward.icon} 아이템 ID {reward.itemID}");
            // 보상 아이템 지급에 대한 추가 로직을 여기에 구현
        }

        // 추가로 퀘스트 완료 상태를 기록하거나 UI에 퀘스트 완료 표시 등의 작업을 할 수 있습니다.
    }
}
