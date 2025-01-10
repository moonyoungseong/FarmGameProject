using System.Collections.Generic; // List 사용을 위한 네임스페이스
using UnityEngine;

public class ConstructionQuestCommand : IQuestCommand
{
    private Quest quest;  // 퀘스트 정보
    private string constructionType;  // 건설해야 할 객체 유형 (예: 집, 강아지)
    private bool isConstructed;  // 건설 완료 여부

    // 생성자에서 퀘스트, 건설해야 할 객체, 초기 상태를 받음
    public ConstructionQuestCommand(Quest quest, string constructionType)
    {
        this.quest = quest;
        this.constructionType = constructionType;
        this.isConstructed = false;  // 시작 시 건설 완료되지 않음
    }

    // 퀘스트 실행 메서드 (퀘스트 시작 시 호출)
    public void Execute()
    {
        Debug.Log($"{quest.questName} 퀘스트 시작: {constructionType} 건설");
    }

    // 건설을 진행하는 메서드
    public void ConstructBuilding()
    {
        if (!isConstructed)
        {
            // 건설 작업이 완료되었음을 나타내는 로직을 여기에 추가
            isConstructed = true;
            Debug.Log($"{constructionType} 건설이 완료되었습니다.");

            // 퀘스트 완료 처리
            QuestCompleted();
        }
        else
        {
            Debug.Log($"{constructionType}은 이미 건설되었습니다.");
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
