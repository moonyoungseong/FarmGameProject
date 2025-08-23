using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestStateManager : MonoBehaviour
{
    public static QuestStateManager Instance;

    [Header("퀘스트 진행상태 텍스트")]
    public TMP_Text questDetailText; // UI에 하나만 사용

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 마우스를 갖다댄 퀘스트 정보를 받아서 상세 텍스트에 진행 상태 갱신
    /// </summary>
    public void ShowQuestDetail(Quest quest)
    {
        if (quest == null)
        {
            questDetailText.text = "";
            return;
        }

        questDetailText.text = $"진행 상태: {GetStateText(quest.state)}"; // $"{quest.questName}\n진행 상태: {GetStateText(quest.state)}\n설명: {quest.description}";
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
}
