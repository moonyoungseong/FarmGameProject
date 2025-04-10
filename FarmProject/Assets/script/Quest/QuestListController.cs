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

        // ��� ������ ����Ʈ ����Ʈ ����
        allQuests.AddRange(questManager.questData.quests.Collection);
        allQuests.AddRange(questManager.questData.quests.Dialogue);
        allQuests.AddRange(questManager.questData.quests.Construction);
        allQuests.AddRange(questManager.questData.quests.Delivery);
        allQuests.AddRange(questManager.questData.quests.Movement);

        foreach (Quest quest in allQuests)
        {
            // ���� ������ ���� �� �θ� ���̱�
            GameObject slot = Instantiate(questSlotPrefab, questSlotParent);

            // ����Ʈ �̸� �ؽ�Ʈ ����
            TextMeshProUGUI questText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (questText != null)
            {
                questText.text = quest.questName;
            }

            // �ڹ��� �̹��� Ȱ��ȭ
            Transform lockImage = slot.transform.Find("LockImage");
            if (lockImage != null)
                lockImage.gameObject.SetActive(true);
        }
    }
}
