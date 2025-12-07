using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MovementQuestCommand.cs
///
/// - 이동형 퀘스트 커맨드를 담당.
/// - 특정 위치에 도착했는지를 기준으로 퀘스트를 완료
/// - SetArrived(true)가 호출되면 도착으로 간주되고 완료 가능
/// - QuestListController를 통해 퀘스트 슬롯 UI 활성화
/// </summary>
public class MovementQuestCommand : IQuestCommand
{
    // 퀘스트 데이터
    public Quest Quest { get; private set; }

    // 이동해야 하는 목표 지점 이름 또는 태그
    public string MovementTarget { get; private set; }

    // 퀘스트 리스트 UI 컨트롤러
    private QuestListController questListController;

    // 도착 여부 
    private bool hasArrived = false;

    // 이동형 퀘스트 생성자
    public MovementQuestCommand(Quest quest, string target, QuestListController controller)
    {
        Quest = quest;
        MovementTarget = target;
        questListController = controller;
    }

    // 퀘스트 실행 (시작 시 InProgress 상태로 전환)
    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;
            questListController?.UnlockQuestSlot(Quest.questName);
        }
    }

    /// <summary>
    /// 목표 지점에 도착했는지 설정
    /// true면 도착 처리
    /// </summary>
    public bool SetArrived(bool arrived)
    {
        hasArrived = arrived;
        return arrived;
    }

    // 도착 여부 반환
    public bool IsArrived()
    {
        if (hasArrived)
            return true;
        else
            return false;
    }

    // 퀘스트 완료 처리
    public void CompleteQuest()
    {
        if (Quest.state == QuestState.InProgress)
        {
            Quest.state = QuestState.Completed;
            RewardManager.Instance.GiveRewards(Quest.reward);
        }
    }

    // 퀘스트를 초기 상태로 되돌림
    public void Undo()
    {
        hasArrived = false;
        Quest.state = QuestState.NotStarted;
    }
}
