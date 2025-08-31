using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuestDetailPanel : MonoBehaviour
{
    public static QuestDetailPanel Instance;

    public TextMeshProUGUI questName;
    public TextMeshProUGUI giverText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI stateText;  // 새로 추가: 진행 상태 표시용
    public Transform rewardIconParent;
    public Image[] rewardIcons;  // 인스펙터에서 3개 할당

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 단일 퀘스트 상세 정보 갱신
    /// </summary>
    public void ShowDetails(Quest quest)
    {
        if (quest == null) return;

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

        // 최대 3개의 보상만 표시
        for (int i = 0; i < quest.reward.Count && i < rewardIcons.Length; i++)
        {
            Sprite sprite = Resources.Load<Sprite>($"Icons/{quest.reward[i].icon}");
            if (sprite == null) continue;

            rewardIcons[i].sprite = sprite;
            rewardIcons[i].gameObject.SetActive(true);
        }
    }

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

    /// <summary>
    /// 여러 퀘스트 리스트에서 선택된 퀘스트를 갱신
    /// </summary>
    public void ShowDetailsFromList(List<Quest> quests, int index)
    {
        if (quests == null || quests.Count == 0) return;
        if (index < 0 || index >= quests.Count) return;

        ShowDetails(quests[index]);
    }
}
