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
    public TextMeshProUGUI stateText;  // ���� �߰�: ���� ���� ǥ�ÿ�
    public Transform rewardIconParent;
    public Image[] rewardIcons;  // �ν����Ϳ��� 3�� �Ҵ�

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// ���� ����Ʈ �� ���� ����
    /// </summary>
    public void ShowDetails(Quest quest)
    {
        if (quest == null) return;

        questName.text = quest.questName;
        giverText.text = "NPC : " + quest.giverNPC;
        descriptionText.text = quest.description;

        // ���� ���� ǥ��
        stateText.text = "���� ����: " + GetStateText(quest.state);

        // ���� ������ �ʱ�ȭ
        foreach (var icon in rewardIcons)
        {
            icon.gameObject.SetActive(false);
        }

        // �ִ� 3���� ���� ǥ��
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
            case QuestState.NotStarted: return "���� ��";
            case QuestState.InProgress: return "���� ��";
            case QuestState.Completed: return "�Ϸ�";
            default: return "";
        }
    }

    /// <summary>
    /// ���� ����Ʈ ����Ʈ���� ���õ� ����Ʈ�� ����
    /// </summary>
    public void ShowDetailsFromList(List<Quest> quests, int index)
    {
        if (quests == null || quests.Count == 0) return;
        if (index < 0 || index >= quests.Count) return;

        ShowDetails(quests[index]);
    }
}
