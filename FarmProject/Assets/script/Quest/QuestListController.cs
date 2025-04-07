using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestListController : MonoBehaviour  // 클래스 이름 변경
{
    public GameObject questSlotPrefab;       // 슬롯 프리팹
    public Transform questSlotParent;        // 슬롯이 배치될 부모 오브젝트
    public QuestManager questManager;        // 퀘스트 매니저 참조

    void Start()
    {
        CreateQuestSlots();
    }

    void CreateQuestSlots()
    {
        if (questManager.questData == null)
        {
            Debug.LogError("퀘스트 데이터가 로드되지 않았습니다.");
            return;
        }

        List<Quest> allQuests = new List<Quest>();

        allQuests.AddRange(questManager.questData.quests.Collection);
        allQuests.AddRange(questManager.questData.quests.Dialogue);
        allQuests.AddRange(questManager.questData.quests.Construction);
        allQuests.AddRange(questManager.questData.quests.Delivery);
        allQuests.AddRange(questManager.questData.quests.Movement);

        foreach (Quest quest in allQuests)
        {
            GameObject slot = Instantiate(questSlotPrefab, questSlotParent);
            TextMeshProUGUI questText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (questText != null)
            {
                questText.text = quest.questName;
            }
        }
    }
}
