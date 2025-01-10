using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementQuestCommand : IQuestCommand
{
    private Quest quest;  // 퀘스트 정보
    private string targetNPCName;  // 목표 NPC 이름
    private bool isCompleted;  // 퀘스트 완료 여부

    // 생성자에서 퀘스트와 목표 NPC 이름을 받음
    public MovementQuestCommand(Quest quest, string targetNPCName)
    {
        this.quest = quest;
        this.targetNPCName = targetNPCName;
        this.isCompleted = false;  // 시작 시 퀘스트 완료되지 않음
    }

    // 퀘스트 실행 메서드
    public void Execute()
    {
        Debug.Log($"{quest.questName} 퀘스트 시작: {targetNPCName}에게 이동");
        // 퀘스트 시작 메시지 등 추가 로직을 작성할 수 있음
    }

    // 트리거 영역에 들어갔을 때 호출되는 메서드 (이 메서드는 트리거 영역에 붙은 오브젝트에서 호출)
    public void OnTriggerEnter(Collider other)
    {
        if (other != null && !isCompleted && other.CompareTag("Player"))
        {
            Debug.Log($"{quest.questName} 퀘스트 완료! {targetNPCName}에게 도달");
            QuestCompleted();
        }
        else
        {
            Debug.Log("트리거에 유효하지 않은 객체가 들어옴");
        }
    }

    // 퀘스트 완료 처리 메서드
    private void QuestCompleted()
    {
        isCompleted = true;
        Debug.Log($"{quest.questName} 퀘스트 완료!");

        // 보상 처리 등 퀘스트 완료 후 처리
        foreach (var reward in quest.reward)
        {
            Debug.Log($"보상 지급: {reward.icon} 아이템 ID {reward.itemID}");
            // 추가 보상 처리 로직
        }
    }
}
