using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective     // ��ǥ �׸� ���� Ŭ���� "�丶�� 3�� ����" ����
{
    public string objective;
}

[System.Serializable]
public class Quest              // ����Ʈ�� �����ϴ� Ŭ����
{
    public int questID;
    public string questName;
    public string description;  // ����
    public string giverNPC;
    public string receiverNPC;
    public List<QuestObjective> objectives;     // ����Ʈ �ؾ��Ұ�
    public List<Reward> reward;
    public int levelRequirement; // �䱸�Ǵ� ����
}

[System.Serializable]
public class QuestCategory
{
    public List<Quest> Collection; // ������
    public List<Quest> Dialogue; // ��ȭ��
    public List<Quest> Construction; // �Ǽ���
    public List<Quest> Delivery; // ������
    public List<Quest> Movement; // �̵���
}

[System.Serializable]
public class Reward     // ����
{
    public int itemID;
    public string icon;
}

[System.Serializable]
public class QuestData  // QuestCategory�� ���Ե� ����Ʈ ��ü �����͸� ����
{
    public QuestCategory quests;
}

public class QuestManager : MonoBehaviour
{
    public TextAsset questDataFile;
    public QuestData questData;
    public QuestInvoker questInvoker;

    private List<CollectQuestCommand> collectCommands = new List<CollectQuestCommand>();
    private List<DialogueQuestCommand> dialogueCommands = new List<DialogueQuestCommand>();
    private List<ConstructionQuestCommand> constructionCommands = new List<ConstructionQuestCommand>();
    private List<DeliveryQuestCommand> deliveryCommands = new List<DeliveryQuestCommand>();
    private List<MovementQuestCommand> movementCommands = new List<MovementQuestCommand>();

    void Awake()    // �ּ��ļ� ���� �������, ����Ʈ �ý��� ���� �� ����
    {
        LoadQuestData();
        //SetUpCollectionQuests();  // ������ ����Ʈ ���� - ������ �ؾ� ������ ���������� �۵��ȴ�.
        SetUpDialogueQuests();
        //SetUpConstructionQuests()
        //SetUpDeliveryQuests();
        //SetUpMovementQuests();
        ExecuteQuestsExample();     // ����Ʈ ���� �ڵ� 
    }

    void LoadQuestData()    // JSON ���Ͽ��� ����Ʈ �����͸� �ε�
    {
        if (questDataFile != null)
        {
            questData = JsonUtility.FromJson<QuestData>(questDataFile.text);
            Debug.Log("����Ʈ ������ �ε� �Ϸ�!");
        }
        else
        {
            Debug.LogError("����Ʈ JSON ������ ������� �ʾҽ��ϴ�!");
        }
    }

    public void SetUpCollectionQuests(string itemName)  // itemName�� �޾Ƽ� �ش� ����Ʈ�� ����
    {
        collectCommands.Clear();  // ������ ��� ����Ʈ ����� �ʱ�ȭ
        foreach (var quest in questData.quests.Collection)
        {
            // itemName�� �´� ����Ʈ�� ����
            if (quest.questName.Contains(itemName))  // ����Ʈ �̸��� itemName�� ���ԵǸ� �ش� ����Ʈ �߰�
            {
                CollectQuestCommand command = new CollectQuestCommand(quest, itemName, 3);
                collectCommands.Add(command);
            }
        }
    }

    //public void SetUpCollectionQuests()     // ����Ʈ �̸��� �丶�� ������ �丶��, �ٸ��� ������ �ٸ� ����Ʈ ������ ��
    //{
    //    foreach (var quest in questData.quests.Collection)
    //    {
    //        string itemName = quest.questName.Contains("�丶��") ? "�丶��" :
    //                          quest.questName.Contains("������") ? "������" : "��";

    //        CollectQuestCommand command = new CollectQuestCommand(quest, itemName, 3);
    //        collectCommands.Add(command);
    //    }
    //}

    // ��ȭ�� ����Ʈ �ڵ� ����
    public void SetUpDialogueQuests()   // ����Ʈ�� NPC�� �ƴ϶� �������� �����Ѵ�.
    {
        foreach (var quest in questData.quests.Dialogue)
        {
            if (quest.giverNPC != null)
            {
                DialogueQuestCommand command = new DialogueQuestCommand(quest, quest.giverNPC);
                dialogueCommands.Add(command);
            }
        }
    }

    public void CompleteDialogueQuest(string npcName)
    {
        foreach (var command in dialogueCommands)
        {
            if (command.NpcName == npcName)
            {
                if (!command.IsQuestStarted)  // ��ȭ�� ���۵��� �ʾҴٸ� ��ȭ ����
                {
                    command.Execute();  // ��ȭ ����
                }
                else
                {
                    // ��ȭ �� ����Ʈ �Ϸ� ó��
                    command.CompleteQuest();
                }
            }
        }
    }

    // ����Ʈ �Ϸ� ������ �ܺο��� ȣ���� �� �ֵ��� ����
    public void MarkQuestAsCompleted(string npcName)
    {
        foreach (var command in dialogueCommands)
        {
            if (command.NpcName == npcName)
            {
                command.CompleteQuest();  // ������ ����Ʈ �Ϸ�
                questInvoker.SetQuestCommand(command);
                questInvoker.ExecuteQuest();
            }
        }
    }

    void SetUpConstructionQuests()
    {
        foreach (var quest in questData.quests.Construction)
        {
            string buildingName = quest.questName.Contains("��") ? "��" :
                                  quest.questName.Contains("������") ? "������" : "��Ÿ �ǹ�";

            ConstructionQuestCommand command = new ConstructionQuestCommand(quest, buildingName);
            constructionCommands.Add(command);
        }
    }

    void SetUpDeliveryQuests()
    {
        foreach (var quest in questData.quests.Delivery)
        {
            // ����Ʈ �̸��� �´� ���� ������ ����
            string itemName = quest.questName.Contains("���") ? "���" :
                              quest.questName.Contains("ġŲ") ? "ġŲ" :
                              quest.questName.Contains("��") ? "��" : "��Ÿ";

            string receiverNPC = quest.receiverNPC;  // �������� ���� NPC �̸�
            DeliveryQuestCommand command = new DeliveryQuestCommand(quest, itemName, 3, receiverNPC);
            deliveryCommands.Add(command);
        }
    }

    void SetUpMovementQuests()
    {
        foreach (var quest in questData.quests.Movement)  // Movement ����Ʈ�� ����
        {
            string receiverNPC = quest.receiverNPC;  // �̵��� NPC �̸�
            MovementQuestCommand command = new MovementQuestCommand(quest, receiverNPC);
            movementCommands.Add(command);  // movementCommands ����Ʈ�� �߰�
        }
    }

    public void ExecuteQuestsExample()     // �� �Լ��� ������ ����Ʈ�� �����ϴ� �ڵ� .ExecuteQuest()
    {
        // ������ ����Ʈ ���� ����
        foreach (var command in collectCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
        }

        // ��ȭ�� ����Ʈ ���� ����
        foreach (var command in dialogueCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();

            // ��ȭ �� ����Ʈ �Ϸ� ó��
            //command.CompleteQuest();
        }

        // �Ǽ��� ����Ʈ ���� ����
        foreach (var command in constructionCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();

            // �Ǽ� �� ����Ʈ �Ϸ� ó��
            command.ConstructBuilding();
        }

        // ������ ����Ʈ ���� ����
        foreach (var command in deliveryCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();

            // ������ ���� �̸��� ����Ͽ� ����Ʈ �Ϸ� ó��
            command.DeliverItem(command.GetItemName());  // GetItemName()�� ������ �̸��� ��ȯ�ϴ� �޼����� ����
        }

        // Movement�� ����Ʈ ���� ����
        foreach (var command in movementCommands)
        {
            questInvoker.SetQuestCommand(command); // ����Ʈ ��� ����
            questInvoker.ExecuteQuest();  // ����Ʈ ����

            // ��ǥ ������ ���� �� ����Ʈ �Ϸ� ó��
            command.OnTriggerEnter(new Collider());  // Ʈ���Ÿ� ���� ����Ʈ �Ϸ� (�÷��̾ Ʈ���� ������ ����)
        }
    }
}