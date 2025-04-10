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
        CreateQuestSlots();  // 시작 시 퀘스트 슬롯 생성
    }

    void CreateQuestSlots()
    {
        if (questManager.questData == null)
        {
            Debug.LogError("퀘스트 데이터가 로드되지 않았습니다.");
            return;
        }

        List<Quest> allQuests = new List<Quest>();

        // 모든 종류의 퀘스트 리스트 통합
        allQuests.AddRange(questManager.questData.quests.Collection);
        allQuests.AddRange(questManager.questData.quests.Dialogue);
        allQuests.AddRange(questManager.questData.quests.Construction);
        allQuests.AddRange(questManager.questData.quests.Delivery);
        allQuests.AddRange(questManager.questData.quests.Movement);

        foreach (Quest quest in allQuests)
        {
            // 슬롯 프리팹 생성 및 부모에 붙이기
            GameObject slot = Instantiate(questSlotPrefab, questSlotParent);

            // 퀘스트 이름 텍스트 설정
            TextMeshProUGUI questText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (questText != null)
            {
                questText.text = quest.questName;
            }

            // 자물쇠 이미지 활성화
            Transform lockImage = slot.transform.Find("LockImage");
            if (lockImage != null)
                lockImage.gameObject.SetActive(true);
        }
    }
}
