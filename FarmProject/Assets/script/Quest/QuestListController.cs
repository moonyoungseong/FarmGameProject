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

            QuestSlot questSlot = slot.GetComponent<QuestSlot>();   // 이게 퀘스트 슬롯 디테일 아래 3줄도
            if (questSlot != null)
            {
                questSlot.questData = quest;
            }

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

        // 대화형 퀘스트는 게임 시작 시 자물쇠 해제
        foreach (Quest dialogueQuest in questManager.questData.quests.Dialogue)
        {
            UnlockQuestSlot(dialogueQuest.questName);
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
