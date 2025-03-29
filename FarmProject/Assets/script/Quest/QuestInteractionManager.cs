using UnityEngine;

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
            // ��������� ��ȭ �� ��ȭ�� ����Ʈ �Ϸ�
            CompleteDialogueQuest("��������");
        }
        else if (npcName == "�丶����")    // ������ string ��ư�� �����ؼ� ������ ����Ʈ ����
        {
            SetUpCollectionQuests("�丶��");
        }
        else if (npcName == "�ҳ��")
        {
            SetUpCollectionQuests("��");
        }
        else if (npcName == "���������")
        {
            SetUpCollectionQuests("������");
        }
        // �ٸ� NPC�� ����Ʈ �߰� ����

        ExecuteQuests();
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
    void ExecuteQuests()
    {
        // ����Ʈ ����
        questManager.ExecuteQuestsExample();
    }
}
