using UnityEngine;
using System.Linq;

public class QuestInteractionManager : MonoBehaviour
{
    public QuestManager questManager; // QuestManager�� �����Ͽ� ����Ʈ �ý��۰� ��ȣ�ۿ�

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
        // �ٸ� NPC�� ����Ʈ �߰� ����

        //ExecuteQuests();
    }

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
    }

    // ����Ʈ ���� �Լ�
    void ExecuteQuests()    // �̰� �ʿ���� ����, ���߿� ���� ����.
    {
        // ����Ʈ ����
        //questManager.ExecuteQuestsExample();
    }
}
