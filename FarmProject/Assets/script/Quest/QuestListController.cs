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
        CreateQuestSlots();  // ���� �� ����Ʈ ���� ����
    }

    void CreateQuestSlots()
    {
        if (questManager.questData == null)
        {
            Debug.LogError("����Ʈ �����Ͱ� �ε���� �ʾҽ��ϴ�.");
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
                // ���� ���̰ų� �Ϸ�� ����Ʈ�� �ڹ��� ����
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
                Debug.Log($"[{questName}] �ڹ��� ���� �Ϸ�");
                return;
            }
        }

        Debug.LogWarning($"[{questName}] �̸��� ������ ã�� ���߽��ϴ�.");
    }
}
