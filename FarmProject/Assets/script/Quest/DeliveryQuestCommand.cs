using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DeliveryQuestCommand.cs
/// 전달형 퀘스트 처리 담당 클래스
/// 
/// - 특정 아이템을 특정 NPC에게 전달하면 퀘스트 완료
/// - QuestListController와 연동해 퀘스트 슬롯을 해제
/// - InventoryManager, CountManager, RewardManager와 연동
/// </summary>
public class DeliveryQuestCommand : IQuestCommand
{
    // 현재 처리 중인 퀘스트 정보
    public Quest Quest { get; private set; }

    // 플레이어가 전달해야 하는 아이템 이름
    public string ItemName { get; private set; }

    // 아이템을 받을 NPC 이름
    public string ReceiverNPC { get; private set; }

    // 퀘스트 목록 UI 제어용 컨트롤러
    private QuestListController questListController;

    // 생성자 - 전달형 퀘스트 초기 설정
    public DeliveryQuestCommand(Quest quest, string itemName, string receiverNPC, QuestListController controller)
    {
        Quest = quest;
        ItemName = itemName;
        ReceiverNPC = receiverNPC;
        questListController = controller;
    }

    // 퀘스트 시작 또는 실행 처리
    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            // 퀘스트 진행 상태로 변경
            Quest.state = QuestState.InProgress;

            // 퀘스트 슬롯 활성화
            questListController?.UnlockQuestSlot(Quest.questName);
        }
    }

    /// <summary>
    /// 실제로 아이템을 NPC에게 전달할 때 호출되는 메서드  
    /// 전달 아이템/수령 NPC가 조건과 동일할 때만 진행
    /// </summary>
    public void DeliverItem(string itemName, string npcName)
    {
        // 퀘스트 진행 중이 아니거나 조건 불일치하면 무시
        if (Quest.state != QuestState.InProgress) return;
        if (itemName != ItemName || npcName != ReceiverNPC) return;

        // 인벤토리에서 아이템을 찾음
        var item = InventoryManager.Instance.GetItemByName(ItemName);
        if (item != null)
        {
            // 아이템 1개 소모
            CountManager.Instance.RemoveItemByID(item.itemID, 1);
        }

        // 퀘스트 완료 처리
        CompleteQuest();
    }

    // 퀘스트 완료 처리 (보상 지급)
    public void CompleteQuest()
    {
        if (Quest.state == QuestState.InProgress)
        {
            Quest.state = QuestState.Completed;
            RewardManager.Instance.GiveRewards(Quest.reward);
        }
    }

    /// <summary>
    /// 퀘스트 상태 되돌리기
    /// </summary>
    public void Undo()
    {
        Quest.state = QuestState.NotStarted;
    }
}
