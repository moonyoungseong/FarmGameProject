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
    public Image[] rewardIcons;  // 인스펙터에서 3개 할당

    void Awake()
    {
        Instance = this;
    }

    public void ShowDetails(Quest quest)
    {
        questName.text = quest.questName;
        giverText.text = "NPC : " + quest.giverNPC;
        descriptionText.text = quest.description;

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
}
