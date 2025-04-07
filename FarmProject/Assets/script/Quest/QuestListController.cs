using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestListController : MonoBehaviour
{
    public GameObject questSlotPrefab;
    public Transform questSlotParent;
    public QuestManager questManager;

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

            // 텍스트 설정
            TextMeshProUGUI questText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (questText != null)
            {
                questText.text = quest.questName;
            }

            // 자물쇠 표시만 활성화
            Transform lockImage = slot.transform.Find("LockImage");
            if (lockImage != null)
                lockImage.gameObject.SetActive(true);
        }
    }
}
