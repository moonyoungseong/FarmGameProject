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

        allQuests.AddRange(questManager.questData.quests.Collection);
        allQuests.AddRange(questManager.questData.quests.Dialogue);
        allQuests.AddRange(questManager.questData.quests.Construction);
        allQuests.AddRange(questManager.questData.quests.Delivery);
        allQuests.AddRange(questManager.questData.quests.Movement);

        foreach (Quest quest in allQuests)
        {
            GameObject slot = Instantiate(questSlotPrefab, questSlotParent);
            QuestSlot questSlot = slot.GetComponent<QuestSlot>();
            if (questSlot != null)
                questSlot.questData = quest;

            TextMeshProUGUI questText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (questText != null)
                questText.text = quest.questName;

            Transform lockImage = slot.transform.Find("LockImage");
            if (lockImage != null)
            {
                // 진행 중이거나 완료된 퀘스트면 자물쇠 끄기
                lockImage.gameObject.SetActive(quest.state == QuestState.NotStarted);
            }
        }
    }


    public void UnlockQuestSlot(string questName)
    {
        foreach (Transform slotTransform in questSlotParent)
        {
            QuestSlot slot = slotTransform.GetComponent<QuestSlot>();
            if (slot != null && slot.questData != null && slot.questData.questName == questName)
            {
                slot.Unlock();
                Debug.Log($"[{questName}] 자물쇠 해제 완료");
                return;
            }
        }

        Debug.LogWarning($"[{questName}] 이름의 슬롯을 찾지 못했습니다.");
    }
}
