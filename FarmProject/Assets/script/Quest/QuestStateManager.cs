using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestStateManager : MonoBehaviour
{
    public static QuestStateManager Instance;

    [Header("����Ʈ ������� �ؽ�Ʈ")]
    public TMP_Text questDetailText; // UI�� �ϳ��� ���

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// ���콺�� ���ٴ� ����Ʈ ������ �޾Ƽ� �� �ؽ�Ʈ�� ���� ���� ����
    /// </summary>
    public void ShowQuestDetail(Quest quest)
    {
        if (quest == null)
        {
            questDetailText.text = "";
            return;
        }

        questDetailText.text = $"���� ����: {GetStateText(quest.state)}"; // $"{quest.questName}\n���� ����: {GetStateText(quest.state)}\n����: {quest.description}";
    }

    private string GetStateText(QuestState state)
    {
        switch (state)
        {
            case QuestState.NotStarted: return "���� ��";
            case QuestState.InProgress: return "���� ��";
            case QuestState.Completed: return "�Ϸ�";
            default: return "";
        }
    }
}
