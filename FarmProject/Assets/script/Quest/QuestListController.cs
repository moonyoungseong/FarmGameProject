using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// QuestListController.cs
///
/// - 퀘스트 전체 목록을 UI 슬롯 형태로 생성하고 관리하는 클래스
/// - QuestManager에서 불러온 모든 퀘스트 데이터를 기반으로 슬롯을 자동 생성
/// - 각 슬롯에는 퀘스트 이름과 잠금(Lock) 이미지가 표시됨
/// - 퀘스트가 시작되면 UnlockQuestSlot()을 통해 해당 슬롯의 잠금을 해제
/// </summary>
public class QuestListController : MonoBehaviour
{
    // 퀘스트 슬롯 프리팹
    public GameObject questSlotPrefab;

    // 슬롯들이 배치될 부모 오브젝트
    public Transform questSlotParent;

    // 퀘스트 매니저 참조
    public QuestManager questManager;

    void Start()
    {
        CreateQuestSlots();  // 시작 시 전체 퀘스트 슬롯 생성
    }

    // 퀘스트 데이터를 기반으로 모든 퀘스트 슬롯 생성
    void CreateQuestSlots()
    {
        if (questManager.questData == null)
        {
            Debug.LogError("퀘스트 데이터가 로드되지 않았습니다.");
            return;
        }

        List<Quest> allQuests = new List<Quest>();

        // 모든 카테고리 퀘스트를 하나의 리스트로 합치기
        allQuests.AddRange(questManager.questData.quests.Collection);
        allQuests.AddRange(questManager.questData.quests.Dialogue);
        allQuests.AddRange(questManager.questData.quests.Construction);
        allQuests.AddRange(questManager.questData.quests.Delivery);
        allQuests.AddRange(questManager.questData.quests.Movement);

        // 슬롯 생성
        foreach (Quest quest in allQuests)
        {
            GameObject slot = Instantiate(questSlotPrefab, questSlotParent);
            QuestSlot questSlot = slot.GetComponent<QuestSlot>();

            // 슬롯에 데이터 연결
            if (questSlot != null)
                questSlot.questData = quest;

            // 텍스트 표시
            TextMeshProUGUI questText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (questText != null)
                questText.text = quest.questName;

            // 자물쇠 이미지 표시 여부 결정
            Transform lockImage = slot.transform.Find("LockImage");
            if (lockImage != null)
            {
                // NotStarted = 잠금 / 그 외 상태는 해제
                lockImage.gameObject.SetActive(quest.state == QuestState.NotStarted);
            }
        }
    }

    // 특정 퀘스트 이름을 가진 슬롯의 잠금 해제
    public void UnlockQuestSlot(string questName)
    {
        foreach (Transform slotTransform in questSlotParent)
        {
            QuestSlot slot = slotTransform.GetComponent<QuestSlot>();

            if (slot != null && slot.questData != null && slot.questData.questName == questName)
            {
                slot.Unlock();
                return;
            }
        }

        Debug.LogWarning($"[{questName}] 이름의 슬롯을 찾지 못했습니다.");
    }
}
