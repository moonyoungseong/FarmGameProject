using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// QuestDetailPanel.cs
///
/// - 퀘스트 상세 정보를 UI 패널에 표시하는 클래스.
/// - 퀘스트 이름, NPC, 설명, 진행 상태, 보상 아이콘 최대 3개까지 표시.
/// - QuestListController에서 슬롯 선택 시 호출되어 세부 정보를 갱신.
/// - 싱글톤으로 운용되며, UI 요소들을 직접 갱신하여 패널 구성.
/// </summary>
public class QuestDetailPanel : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static QuestDetailPanel Instance;

    // 퀘스트 이름 UI
    public TextMeshProUGUI questName;

    // 퀘스트를 주는 NPC 이름
    public TextMeshProUGUI giverText;

    // 퀘스트 설명
    public TextMeshProUGUI descriptionText;

    // 퀘스트 진행 상태 표시 텍스트
    public TextMeshProUGUI stateText;

    // 보상 아이콘을 배치할 부모 오브젝트
    public Transform rewardIconParent;

    // 보상 아이콘 UI 3개
    public Image[] rewardIcons;

    // 싱글톤 초기화
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 단일 퀘스트 상세 정보 표시
    public void ShowDetails(Quest quest)
    {
        if (quest == null) return;

        // 이름, NPC, 설명 텍스트 설정
        questName.text = quest.questName;
        giverText.text = "NPC : " + quest.giverNPC;
        descriptionText.text = quest.description;

        // 진행 상태 표시
        stateText.text = "진행 상태: " + GetStateText(quest.state);

        // 보상 아이콘 초기화
        foreach (var icon in rewardIcons)
        {
            icon.gameObject.SetActive(false);
        }

        // 보상 아이콘 로드 및 표시 (최대 3개)
        for (int i = 0; i < quest.reward.Count && i < rewardIcons.Length; i++)
        {
            Sprite sprite = Resources.Load<Sprite>($"Icons/{quest.reward[i].icon}");
            if (sprite == null) continue;

            rewardIcons[i].sprite = sprite;
            rewardIcons[i].gameObject.SetActive(true);
        }
    }

    // QuestState → 텍스트 변환
    private string GetStateText(QuestState state)
    {
        switch (state)
        {
            case QuestState.NotStarted: return "시작 전";
            case QuestState.InProgress: return "진행 중";
            case QuestState.Completed: return "완료";
            default: return "";
        }
    }

    // 퀘스트 리스트에서 선택한 인덱스를 기반으로 상세 정보 표시
    public void ShowDetailsFromList(List<Quest> quests, int index)
    {
        if (quests == null || quests.Count == 0) return;
        if (index < 0 || index >= quests.Count) return;

        ShowDetails(quests[index]);
    }
}
