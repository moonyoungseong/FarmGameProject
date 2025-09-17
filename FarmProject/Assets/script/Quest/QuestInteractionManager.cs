using UnityEngine;
using System.Linq;

public class QuestInteractionManager : MonoBehaviour
{
    public QuestManager questManager; // QuestManager�� �����Ͽ� ����Ʈ �ý��۰� ��ȣ�ۿ�
    public QuestListController questListController; // Inspector���� ����

    //private CollectQuestCommand currentCollectQuest;    // ������ ����Ʈ 

    // ���� ���� �� ȣ��Ǵ� Start �Լ�
    void Start()
    {
        // ���� ���۰� �Բ� ��ȭ�� ����Ʈ ����
        //SetUpDialogueQuests("��������"); // "��������" NPC�� ���� ��ȭ�� ����Ʈ�� ����
        SetUpDialogueQuests(); // "��������" NPC�� ���� ��ȭ�� ����Ʈ�� ����
    }

    // ��ȭ�� ����Ʈ ���� �Լ� (�Ű������� NPC �̸� ����)
    void SetUpDialogueQuests()
    {
        questManager.SetUpDialogueQuests(); // ��ȭ�� ����Ʈ ����
    }

    // NPC�� ��ȣ�ۿ� �� ȣ��Ǵ� �Լ�
    public void InteractWithNPC(string npcName)
    {
        if (npcName == "��������")
        {
            CompleteDialogueQuest(npcName); // ��ȭ���� ��ȣ�ۿ��ϸ� ����Ʈ �Ϸ�
        }
        else if (npcName == "�䳢�ֹ�")
        {
            CompleteDialogueQuest(npcName); // ��ȭ �Ϸ�
        }
        else if (npcName == "�丶����")    // ������ string ��ư�� �����ؼ� ������ ����Ʈ ����
        {
            SetUpCollectionQuests("�丶��");
            questManager.ExecuteCollectQuests();
        }
        else if (npcName == "�ҳ��")
        {
            SetUpCollectionQuests("��");
            questManager.ExecuteCollectQuests();
        }
        else if (npcName == "���������")
        {
            SetUpCollectionQuests("������");
            questManager.ExecuteCollectQuests();
        }
        else if (npcName == "��������2")
        {
            SetUpConstructionQuests("House_1"); // "��" �Ǵ� "������"
            questManager.ExecuteConstructionQuests();
        }
        else if (npcName == "����")
        {
            SetUpConstructionQuests("Dog"); // "��" �Ǵ� "������"
            questManager.ExecuteConstructionQuests();
        }
        else if (npcName == "���������2")  // ������ ����Ʈ: ��⸦ ������ڿ��� ����
        {
            SetUpDeliveryQuests("������");
            questManager.ExecuteDeliveryQuests();
        }
        else if (npcName == "����������")  // ������ ����Ʈ: ġŲ�� �������� ����
        {
            SetUpDeliveryQuests("���");
            questManager.ExecuteDeliveryQuests();
        }
        else if (npcName == "��������3")  // �̵��� ����Ʈ: ��ٸ� Ÿ�� �ö󰡼� ����
        {
            SetUpMovementQuests("LadderFixSpot");  // movementTarget ��
            questManager.ExecuteMovementQuests();
        }
        // �ٸ� NPC�� ����Ʈ �߰� ����

        //ExecuteQuests();
    }

    //// ������ ��ư Ŭ�� �� ȣ��
    //public void CollectCompleteButtonClick(int questId)
    //{
    //    Quest quest = QuestManager.Instance.GetQuestByID(questId);
    //    if (quest != null)
    //    {
    //        // ����Ʈ�� ������ ���� ��������
    //        string itemName = quest.targetItemName;   // Quest Ŭ������ ���� ������ �̸� �Ӽ� �ʿ�
    //        int requiredAmount = quest.requiredAmount; // Quest Ŭ������ ���� ���� �Ӽ� �ʿ�

    //        CollectQuestCommand command = new CollectQuestCommand(quest, itemName, requiredAmount);

    //        // ��ư Ŭ�� �� �������̸� �Ϸ� �õ�
    //        command.TryCompleteQuest();
    //    }
    //    else
    //    {
    //        Debug.LogError($"����Ʈ {questId}�� ã�� �� �����ϴ�.");
    //    }
    //}


    // ��ȭ�� ����Ʈ �Ϸ� �Լ�
    void CompleteDialogueQuest(string npcName)
    {
        // ��ȭ�� ����Ʈ �Ϸ�
        questManager.CompleteDialogueQuest(npcName);
    }

    // Collection ����Ʈ ���� �Լ�
    void SetUpCollectionQuests(string itemName)
    {
        // ������ ����Ʈ ����
        questManager.SetUpCollectionQuests(itemName);

        foreach (Quest quest in questManager.questData.quests.Collection)
        {
            if (quest.questName.Contains(itemName))
            {
                // QuestListController�� ���� ���� ����
                questListController.UnlockQuestSlot(quest.questName);
            }
        }
    }

    // �Ǽ��� ����Ʈ ����
    void SetUpConstructionQuests(string buildingName)
    {
        questManager.SetUpConstructionQuests(buildingName);
    }

    // ������ ����Ʈ ���� �Լ�
    void SetUpDeliveryQuests(string itemName)
    {
        questManager.SetUpDeliveryQuests(itemName);
    }

    // �̵��� ����Ʈ ���� �Լ�
    void SetUpMovementQuests(string movementTarget)
    {
        questManager.SetUpMovementQuests(movementTarget);
    }

    // ����Ʈ ���� �Լ�
    void ExecuteQuests()    // �̰� �ʿ���� ����, ���߿� ���� ����.
    {
        // ����Ʈ ����
        //questManager.ExecuteQuestsExample();
    }
}
