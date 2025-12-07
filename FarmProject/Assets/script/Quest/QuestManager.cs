using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective
{
    public string objective;   // 목표 설명 텍스트
}

[System.Serializable]
/// <summary>
/// 퀘스트의 핵심 정보를 담는 데이터 구조
/// </summary>
public class Quest
{
    public int questID;                 
    public string questName;            
    public string buildingName;         // 건설형 퀘스트에서 사용할 건물 이름
    public string description;          // 퀘스트 상세 설명
    public string giverNPC;             // 퀘스트 주는 NPC 이름
    public string receiverNPC;          // 전달형 퀘스트의 수령 NPC
    public string movementTarget;       // 이동형 퀘스트 목표 지점
    public List<QuestObjective> objectives; // 퀘스트 목표 리스트
    public List<Reward> reward;         // 보상 리스트
    public QuestState state = QuestState.NotStarted;  // 현재 퀘스트 진행 상태
    public int questIndex;              
    public int requiredAmount;          // 수집/전달 퀘스트에서 필요한 개수
    public string itemName;             // 필요한 아이템 이름
    public int levelRequirement;        // 필요 레벨(사용하지 않아도 확장성)
    public bool canComplete = false;    // 완료 가능 여부 플래그
}

[System.Serializable]
/// <summary>
/// 퀘스트를 종류별로 묶어 관리하는 카테고리 클래스
/// (수집, 대화, 건설, 전달, 이동)
/// </summary>
public class QuestCategory
{
    public List<Quest> Collection;
    public List<Quest> Dialogue;
    public List<Quest> Construction;
    public List<Quest> Delivery;
    public List<Quest> Movement;
}

// 퀘스트 진행 상태 Enum
public enum QuestState
{
    NotStarted, 
    InProgress, 
    Completed   
}

[System.Serializable]
// 퀘스트 보상 정보를 담는 클래스
public class Reward
{
    public int itemID;     
    public string itemname;
    public string icon;    // 리소스 폴더에 있는 아이콘 파일명
    public int quantity;   
}

[System.Serializable]
// JSON에서 불러올 전체 퀘스트 데이터 구조
public class QuestData
{
    public QuestCategory quests; // 카테고리별 퀘스트 데이터
}

/// <summary>
/// 퀘스트 전체를 관리하는 매니저
/// - JSON 데이터 로드
/// - 각 퀘스트에 맞는 Command 생성
/// - 실행 및 완료 헬퍼 제공
/// </summary>
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public TextAsset questDataFile;             // JSON 파일
    public QuestData questData;                 // 파싱된 퀘스트 데이터
    public QuestInvoker questInvoker;           // 커맨드 패턴 실행기
    public QuestListController questListController; // UI 슬롯 제어용

    public Dictionary<int, IQuestCommand> questCommands = new Dictionary<int, IQuestCommand>();

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadQuestData();     // JSON 로드
        SetUpAllQuests();    // 모든 퀘스트 커맨드 초기화
    }

    /// JSON 파일에서 퀘스트 데이터를 읽어오는 함수
    void LoadQuestData()
    {
        if (questDataFile != null)
        {
            questData = JsonUtility.FromJson<QuestData>(questDataFile.text);
        }
        else
        {
            Debug.LogError("퀘스트 JSON 파일이 연결되지 않았습니다!");
        }
    }

    // 전체 퀘스트 카테고리의 Command들을 설정
    void SetUpAllQuests()
    {
        SetUpCollectionQuests();
        SetUpDialogueQuests();
        SetUpConstructionQuests();
        SetUpDeliveryQuests();
        SetUpMovementQuests();
    }

    #region Collection Quests
    // 수집형 퀘스트(Command) 설정
    void SetUpCollectionQuests()
    {
        foreach (var quest in questData.quests.Collection)
        {
            // 필요한 아이템과 수량을 전달
            var command = new CollectQuestCommand(quest, quest.itemName, quest.requiredAmount, questListController);
            questCommands[quest.questID] = command;

            // 퀘스트 UI의 자물쇠 해제
            questListController?.UnlockQuestSlot(quest.questName);
        }
    }
    #endregion

    #region Dialogue Quests
    // 대화형 퀘스트(Command) 설정
    void SetUpDialogueQuests()
    {
        foreach (var quest in questData.quests.Dialogue)
        {
            var command = new DialogueQuestCommand(quest, quest.giverNPC, questListController);
            questCommands[quest.questID] = command;

            questListController?.UnlockQuestSlot(quest.questName);

            // 대화형 퀘스트는 자동 실행되도록 처리
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
        }
    }
    #endregion

    #region Construction Quests
    /// 건설형 퀘스트(Command) 설정
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
    // 전달형 퀘스트(Command) 설정
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
    // 이동형 퀘스트(Command) 설정
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
    // 퀘스트 ID를 기반으로 Command를 실행하는 함수
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

    /// <summary>
    /// 퀘스트 ID를 기반으로 퀘스트 완료 처리
    /// (Command 패턴의 CompleteQuest 호출)
    /// </summary>
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

    // 퀘스트 ID로 퀘스트 데이터를 가져오는 함수
    public Quest GetQuestByID(int questID)
    {
        if (questData == null || questData.quests == null) return null;

        // 카테고리를 순차적으로 검사해 해당 ID의 퀘스트 찾기
        Quest quest = questData.quests.Collection.Find(q => q.questID == questID)
                    ?? questData.quests.Dialogue.Find(q => q.questID == questID)
                    ?? questData.quests.Construction.Find(q => q.questID == questID)
                    ?? questData.quests.Delivery.Find(q => q.questID == questID)
                    ?? questData.quests.Movement.Find(q => q.questID == questID);

        return quest;
    }
}
