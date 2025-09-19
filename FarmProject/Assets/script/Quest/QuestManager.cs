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
    public string buildingName; // ���� ���� �̸�
    public string description;  // ����
    public string giverNPC;
    public string receiverNPC;
    public string movementTarget;
    public List<QuestObjective> objectives;     // ����Ʈ �ؾ��Ұ�
    public List<Reward> reward;
    public QuestState state = QuestState.NotStarted;    // ����Ʈ �ϱ� �� ���� (������)
    public int questIndex;                      // ����Ʈ ���� ( ������¿��� ����
    public int levelRequirement; // �䱸�Ǵ� ����
    public bool canComplete = false;    // ����Ʈ �Ϸ� �������� Ȯ��
}

[System.Serializable]
public class QuestCategory  // ����Ʈ ī�װ�
{
    public List<Quest> Collection; // ������
    public List<Quest> Dialogue; // ��ȭ��
    public List<Quest> Construction; // �Ǽ���
    public List<Quest> Delivery; // ������
    public List<Quest> Movement; // �̵���
}

public enum QuestState
{
    NotStarted,   // ���� ��
    InProgress,   // ���� ��
    Completed     // �Ϸ�
}

[System.Serializable]
public class Reward     // ����
{
    public int itemID;
    public string itemname;
    public string icon;
}

[System.Serializable]
public class QuestData  // QuestCategory�� ���Ե� ����Ʈ ��ü �����͸� ����
{
    public QuestCategory quests;
}

public class QuestManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static QuestManager Instance { get; private set; }

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
        // �̱��� �ʱ�ȭ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // �ٸ� ������ �Ѿ�� �����ϰ� �ʹٸ�
        // DontDestroyOnLoad(gameObject);

        LoadQuestData();
        //SetUpCollectionQuests();  // ������ ����Ʈ ���� - ������ �ؾ� ������ ���������� �۵��ȴ�.
        SetUpDialogueQuests();      // ��ȭ�� ����Ʈ ���� - ���ۺ��� ����Ʈ�� ����Ǽ� �ּ� ó�� XX
        //SetUpConstructionQuests()
        //SetUpDeliveryQuests();
        //SetUpMovementQuests();
        //ExecuteQuestsExample();     // ����Ʈ ���� �ڵ� - �̰� �������� �����µ� �ּ� ó�� �׽�Ʈ
        ExecuteDialogueQuests();    // ��ȭ�� �ڵ� ���� / �ٸ� ������ ����. ���߿� Ȯ��
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

    //public void SetUpCollectionQuests()
    //{
    //    collectCommands.Clear();
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
                if (!command.IsQuestStarted)  // ��ȭ�� ���۵��� �ʾҴٸ� ��ȭ ���� -- �̺κе� ���� �ʿ�, ����Ʈ �Ϸ��ϰ� �� ��������.
                {
                    //command.Execute();  // ��ȭ ����
                }
                else
                {
                    // ��ȭ �� ����Ʈ �Ϸ� ó��
                    command.CompleteQuest();
                }
            }
        }
    }

    // ����Ʈ �Ϸ� ������ �ܺο��� ȣ���� �� �ֵ��� ����, ��ȭ�� ����Ʈ
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

    public void SetUpConstructionQuests(string buildingName)
    {
        constructionCommands.Clear(); // ���� ��� �ʱ�ȭ (����)
        foreach (var quest in questData.quests.Construction)
        {
            if (quest.buildingName == buildingName) // �Ű������� ���� buildingName�� �ش��ϴ� ����Ʈ��
            {
                ConstructionQuestCommand command = new ConstructionQuestCommand(quest, buildingName);
                constructionCommands.Add(command);
            }
        }
    }

    public void SetUpDeliveryQuests(string itemName)
    {
        deliveryCommands.Clear();  // ���� ������ ����Ʈ �ʱ�ȭ

        foreach (var quest in questData.quests.Delivery)
        {
            // ����Ʈ �̸��� itemName�� ���ԵǾ� ������ ����
            if (quest.questName.Contains(itemName))
            {
                string receiverNPC = quest.receiverNPC;  // �������� ���� NPC �̸�
                DeliveryQuestCommand command = new DeliveryQuestCommand(quest, itemName, 3, receiverNPC);
                deliveryCommands.Add(command);
            }
        }
    }

    public void SetUpMovementQuests(string targetID)
    {
        movementCommands.Clear();

        foreach (var quest in questData.quests.Movement)
        {
            if (quest.movementTarget == targetID)
            {
                MovementQuestCommand command = new MovementQuestCommand(quest, quest.receiverNPC);
                movementCommands.Add(command);
            }
        }
    }

    // ������ ����Ʈ�� ����
    public void ExecuteCollectQuests()
    {
        foreach (var command in collectCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
        }
    }

    // ��ȭ�� ����Ʈ�� ����
    public void ExecuteDialogueQuests()
    {
        foreach (var command in dialogueCommands)
        {
            command.Execute();   // questInvoker�� ��ġ�� �ʰ� �ٷ� ���� // �̰� ��ȭ�� ����Ʈ �����ϴٰ� ����, ���߿� �����ص� ����
        }
    }

    // �Ǽ��� ����Ʈ�� ����
    public void ExecuteConstructionQuests()
    {
        foreach (var command in constructionCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
            //command.ConstructBuilding();  // �Ǽ� �����ϸ鼭 �Ϸ���� �Ǵ� �Լ�
        }
    }

    // ������ ����Ʈ�� ����
    public void ExecuteDeliveryQuests()
    {
        foreach (var command in deliveryCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
            //command.DeliverItem(command.GetItemName());
        }
    }

    // �̵��� ����Ʈ�� ����
    public void ExecuteMovementQuests()
    {
        foreach (var command in movementCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
            //command.OnTriggerEnter(new Collider());
        }
    }

    //  UISwitch���� ȣ���� �� �ֵ��� Quest �˻� �Լ� �߰�
    public Quest GetQuestByID(int questID)
    {
        if (questData == null || questData.quests == null) return null;

        Quest quest = questData.quests.Collection.Find(q => q.questID == questID);
        if (quest != null) return quest;

        quest = questData.quests.Dialogue.Find(q => q.questID == questID);
        if (quest != null) return quest;

        quest = questData.quests.Construction.Find(q => q.questID == questID);
        if (quest != null) return quest;

        quest = questData.quests.Delivery.Find(q => q.questID == questID);
        if (quest != null) return quest;

        quest = questData.quests.Movement.Find(q => q.questID == questID);
        return quest;
    }
}