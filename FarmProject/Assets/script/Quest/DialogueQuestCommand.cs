using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DialogueQuestCommand.cs
/// 대화형 퀘스트를 처리하는 커맨드 클래스.
///
/// - 특정 NPC와 대화를 하면 퀘스트 완료
/// - 퀘스트 슬롯 UI 해제, 연동
/// - 보상 지급 시스템과 연동 (RewardManager)
/// </summary>
public class DialogueQuestCommand : IQuestCommand
{
    // 퀘스트 데이터(이름 / 보상 / 상태 등)
    public Quest Quest { get; private set; }

    // 대화해야 하는 NPC 이름
    public string GiverNPC { get; private set; }

    // 퀘스트 목록 UI 제어 컨트롤러
    private QuestListController questListController;

    // 생성자 - 대화형 퀘스트 초기 설정
    public DialogueQuestCommand(Quest quest, string giverNPC, QuestListController controller)
    {
        Quest = quest;
        GiverNPC = giverNPC;
        questListController = controller;

        // 대화형 퀘스트는 생성 즉시 퀘스트 슬롯을 열어줌
        questListController?.UnlockQuestSlot(Quest.questName);
    }

    /// <summary>
    /// 퀘스트 시작 또는 실행 처리  
    /// (NotStarted → InProgress)
    /// </summary>
    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
        }
    }

    // 대화 완료 시 퀘스트 완료 처리  
    public void CompleteQuest()
    {
        if (Quest.state == QuestState.InProgress)
        {
            Quest.state = QuestState.Completed;

            // 보상 지급
            RewardManager.Instance.GiveRewards(Quest.reward);
        }
    }

    /// <summary>
    /// 퀘스트 상태를 초기 상태로 되돌림
    /// </summary>
    public void Undo()
    {
        Quest.state = QuestState.NotStarted;
    }
}
