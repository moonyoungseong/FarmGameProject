using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestListController : MonoBehaviour  // Ŭ���� �̸� ����
{
    public GameObject questSlotPrefab;       // ���� ������
    public Transform questSlotParent;        // ������ ��ġ�� �θ� ������Ʈ
    public QuestManager questManager;        // ����Ʈ �Ŵ��� ����

    void Start()
    {
        CreateQuestSlots();
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
            TextMeshProUGUI questText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (questText != null)
            {
                questText.text = quest.questName;
            }
        }
    }
}
