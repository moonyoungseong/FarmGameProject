using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective
{
    public string objective;
}

[System.Serializable]
public class Quest
{
    public int questID;
    public string questName;
    public string buildingName;
    public string description;
    public string giverNPC;
    public string receiverNPC;
    public string movementTarget;
    public List<QuestObjective> objectives;
    public List<Reward> reward;
    public QuestState state = QuestState.NotStarted;
    public int questIndex;
    public int requiredAmount;
    public string itemName;
    public int levelRequirement;
    public bool canComplete = false;
}

[System.Serializable]
public class QuestCategory
{
    public List<Quest> Collection;
    public List<Quest> Dialogue;
    public List<Quest> Construction;
    public List<Quest> Delivery;
    public List<Quest> Movement;
}

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed
}

[System.Serializable]
public class Reward
{
    public int itemID;
    public string itemname;
    public string icon;
    public int quantity;
}

[System.Serializable]
public class QuestData
{
    public QuestCategory quests;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public TextAsset questDataFile;
    public QuestData questData;
    public QuestInvoker questInvoker;
    public QuestListController questListController;

    public Dictionary<int, IQuestCommand> questCommands = new Dictionary<int, IQuestCommand>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadQuestData();
        SetUpAllQuests();
    }

    void LoadQuestData()
    {
        if (questDataFile != null)
        {
            questData = JsonUtility.FromJson<QuestData>(questDataFile.text);
            Debug.Log("퀘스트 데이터 로드 완료!");
        }
        else
        {
            Debug.LogError("퀘스트 JSON 파일이 연결되지 않았습니다!");
        }
    }

    void SetUpAllQuests()
    {
        SetUpCollectionQuests();
        SetUpDialogueQuests();
        SetUpConstructionQuests();
        SetUpDeliveryQuests();
        SetUpMovementQuests();
    }

    #region Collection Quests
    void SetUpCollectionQuests()
    {
        foreach (var quest in questData.quests.Collection)
        {
            var command = new CollectQuestCommand(quest, quest.itemName, quest.requiredAmount, questListController);
            questCommands[quest.questID] = command;
            questListController?.UnlockQuestSlot(quest.questName);
        }
    }
    #endregion

    #region Dialogue Quests
    void SetUpDialogueQuests()
    {
        foreach (var quest in questData.quests.Dialogue)
        {
            var command = new DialogueQuestCommand(quest, quest.giverNPC, questListController);
            questCommands[quest.questID] = command;
            questListController?.UnlockQuestSlot(quest.questName);

            // 대화형 퀘스트 자동 시작
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
        }
    }
    #endregion

    #region Construction Quests
    void SetUpConstructionQuests()
    {
        foreach (var quest in questData.quests.Construction)
        {
            var command = new ConstructionQuestCommand(quest, quest.buildingName, questListController);
            questCommands[quest.questID] = command;
            questListController?.UnlockQuestSlot(quest.questName);
        }
    }
    #endregion

    #region Delivery Quests
    void SetUpDeliveryQuests()
    {
        foreach (var quest in questData.quests.Delivery)
        {
            var command = new DeliveryQuestCommand(quest, quest.itemName, quest.receiverNPC, questListController);
            questCommands[quest.questID] = command;
            questListController?.UnlockQuestSlot(quest.questName);
        }
    }
    #endregion

    #region Movement Quests
    void SetUpMovementQuests()
    {
        foreach (var quest in questData.quests.Movement)
        {
            var command = new MovementQuestCommand(quest, quest.movementTarget, questListController);
            questCommands[quest.questID] = command;
            questListController?.UnlockQuestSlot(quest.questName);
        }
    }
    #endregion

    #region 퀘스트 실행 / 완료 헬퍼
    public void ExecuteQuestByID(int questID)
    {
        if (questCommands.TryGetValue(questID, out var command))
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
        }
        else
        {
            Debug.LogWarning($"퀘스트 ID {questID}에 해당하는 Command가 없습니다.");
        }
    }

    public void CompleteQuestByID(int questID)
    {
        if (questCommands.TryGetValue(questID, out var command))
        {
            command.CompleteQuest();
        }
        else
        {
            Debug.LogWarning($"퀘스트 ID {questID}에 해당하는 Command가 없습니다.");
        }
    }
    #endregion

    public Quest GetQuestByID(int questID)
    {
        if (questData == null || questData.quests == null) return null;

        Quest quest = questData.quests.Collection.Find(q => q.questID == questID)
                    ?? questData.quests.Dialogue.Find(q => q.questID == questID)
                    ?? questData.quests.Construction.Find(q => q.questID == questID)
                    ?? questData.quests.Delivery.Find(q => q.questID == questID)
                    ?? questData.quests.Movement.Find(q => q.questID == questID);

        return quest;
    }
}
