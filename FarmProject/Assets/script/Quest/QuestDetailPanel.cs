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
    public Transform rewardIconParent;
    public Image[] rewardIcons;  // �ν����Ϳ��� 3�� �Ҵ�

    void Awake()
    {
        Instance = this;
    }

    public void ShowDetails(Quest quest)
    {
        questName.text = quest.questName;
        giverText.text = "NPC : " + quest.giverNPC;
        descriptionText.text = quest.description;

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
}
