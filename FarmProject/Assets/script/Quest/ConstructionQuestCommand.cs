using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ConstructionQuestCommand.cs
///
/// - 건설형 퀘스트를 처리하는 명령 클래스.
/// - 특정 건물이 씬에 생성되었는지를 확인하여 퀘스트 완료 여부를 판단.
/// - 퀘스트 실행, 완료 여부 검사, 완료 처리, 되돌리기 기능을 포함.
/// </summary>
public class ConstructionQuestCommand : IQuestCommand
{
    // 현재 퀘스트 데이터
    public Quest Quest { get; private set; }

    // 건설해야 하는 건물 이름 (태그 기준)
    public string BuildingName { get; private set; }

    // 퀘스트 UI 슬롯을 제어하는 컨트롤러
    private QuestListController questListController;

    /// <summary>
    /// 생성자
    /// - 퀘스트 정보, 건물 이름, UI 컨트롤러를 초기화한다.
    /// </summary>
    public ConstructionQuestCommand(Quest quest, string buildingName, QuestListController controller)
    {
        Quest = quest;
        BuildingName = buildingName;
        questListController = controller;
    }

    /// <summary>
    /// 퀘스트 실행
    /// NotStarted 상태일 경우 InProgress로 변경하고 UI 슬롯을 열어준다.
    /// </summary>
    public void Execute()
    {
        if (Quest.state == QuestState.NotStarted)
        {
            Quest.state = QuestState.InProgress;

            // 퀘스트 슬롯 활성화
            questListController?.UnlockQuestSlot(Quest.questName);
        }
    }

    /// <summary>
    /// 퀘스트 완료 조건 검사
    /// BuildingName 과 동일한 태그를 가진 오브젝트가 씬에 존재하는지 체크
    /// </summary>
    public bool CanComplete()
    {
        var buildings = GameObject.FindGameObjectsWithTag(BuildingName);
        return buildings.Length > 0;
    }

    /// <summary>
    /// 퀘스트 완료 처리
    /// 건물이 생성되었는지 확인한 뒤 보상 지급 및 상태 완료 처리
    /// </summary>
    public void CompleteQuest()
    {
        if (!CanComplete())
        {
            Debug.LogWarning($"퀘스트 완료 불가: {Quest.questName}, 건물 {BuildingName} 아직 없음");
            return;
        }

        Quest.state = QuestState.Completed;

        // 보상 지급
        RewardManager.Instance.GiveRewards(Quest.reward);
    }

    /// <summary>
    /// 퀘스트 되돌리기
    /// - 퀘스트 상태를 다시 NotStarted 로 초기화
    /// </summary>
    public void Undo()
    {
        Quest.state = QuestState.NotStarted;
    }
}
