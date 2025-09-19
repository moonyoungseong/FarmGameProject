using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective     // 목표 항목 정의 클래스 "토마토 3개 수집" 같은
{
    public string objective;
}

[System.Serializable]
public class Quest              // 퀘스트를 정의하는 클래스
{
    public int questID;
    public string questName;
    public string buildingName; // 지을 물건 이름
    public string description;  // 설명
    public string giverNPC;
    public string receiverNPC;
    public string movementTarget;
    public List<QuestObjective> objectives;     // 퀘스트 해야할것
    public List<Reward> reward;
    public QuestState state = QuestState.NotStarted;    // 퀘스트 하기 전 상태 (고정값)
    public int questIndex;                      // 퀘스트 순서 ( 진행상태에서 쓰기
    public int levelRequirement; // 요구되는 레벨
    public bool canComplete = false;    // 퀘스트 완료 가능한지 확인
}

[System.Serializable]
public class QuestCategory  // 퀘스트 카테고리
{
    public List<Quest> Collection; // 수집형
    public List<Quest> Dialogue; // 대화형
    public List<Quest> Construction; // 건설형
    public List<Quest> Delivery; // 전달형
    public List<Quest> Movement; // 이동형
}

public enum QuestState
{
    NotStarted,   // 시작 전
    InProgress,   // 진행 중
    Completed     // 완료
}

[System.Serializable]
public class Reward     // 보상
{
    public int itemID;
    public string itemname;
    public string icon;
}

[System.Serializable]
public class QuestData  // QuestCategory가 포함된 퀘스트 전체 데이터를 저장
{
    public QuestCategory quests;
}

public class QuestManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static QuestManager Instance { get; private set; }

    public TextAsset questDataFile;
    public QuestData questData;
    public QuestInvoker questInvoker;

    private List<CollectQuestCommand> collectCommands = new List<CollectQuestCommand>();
    private List<DialogueQuestCommand> dialogueCommands = new List<DialogueQuestCommand>();
    private List<ConstructionQuestCommand> constructionCommands = new List<ConstructionQuestCommand>();
    private List<DeliveryQuestCommand> deliveryCommands = new List<DeliveryQuestCommand>();
    private List<MovementQuestCommand> movementCommands = new List<MovementQuestCommand>();

    void Awake()    // 주석쳐서 아직 실행안함, 퀘스트 시스템 아직 못 만듬
    {
        // 싱글톤 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 다른 씬으로 넘어가도 유지하고 싶다면
        // DontDestroyOnLoad(gameObject);

        LoadQuestData();
        //SetUpCollectionQuests();  // 수집형 퀘스트 세팅 - 세팅을 해야 실행이 정상적으로 작동된다.
        SetUpDialogueQuests();      // 대화형 퀘스트 세팅 - 시작부터 퀘스트가 실행되서 주석 처리 XX
        //SetUpConstructionQuests()
        //SetUpDeliveryQuests();
        //SetUpMovementQuests();
        //ExecuteQuestsExample();     // 퀘스트 실행 코드 - 이거 종류별로 나눴는데 주석 처리 테스트
        ExecuteDialogueQuests();    // 대화형 코드 실행 / 다른 종류도 있음. 나중에 확인
    }

    void LoadQuestData()    // JSON 파일에서 퀘스트 데이터를 로드
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

    public void SetUpCollectionQuests(string itemName)  // itemName을 받아서 해당 퀘스트만 설정
    {
        collectCommands.Clear();  // 기존의 모든 퀘스트 명령을 초기화
        foreach (var quest in questData.quests.Collection)
        {
            // itemName에 맞는 퀘스트만 설정
            if (quest.questName.Contains(itemName))  // 퀘스트 이름에 itemName이 포함되면 해당 퀘스트 추가
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
    //        string itemName = quest.questName.Contains("토마토") ? "토마토" :
    //                          quest.questName.Contains("옥수수") ? "옥수수" : "쌀";
    //        CollectQuestCommand command = new CollectQuestCommand(quest, itemName, 3);
    //        collectCommands.Add(command);
    //    }
    //}

    // 대화형 퀘스트 자동 시작
    public void SetUpDialogueQuests()   // 퀘스트를 NPC가 아니라 원래부터 존재한다.
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
                if (!command.IsQuestStarted)  // 대화가 시작되지 않았다면 대화 시작 -- 이부분도 수정 필요, 퀘스트 완료하고 싹 정리하자.
                {
                    //command.Execute();  // 대화 시작
                }
                else
                {
                    // 대화 후 퀘스트 완료 처리
                    command.CompleteQuest();
                }
            }
        }
    }

    // 퀘스트 완료 로직을 외부에서 호출할 수 있도록 수정, 대화형 퀘스트
    public void MarkQuestAsCompleted(string npcName)
    {
        foreach (var command in dialogueCommands)
        {
            if (command.NpcName == npcName)
            {
                command.CompleteQuest();  // 실제로 퀘스트 완료
                questInvoker.SetQuestCommand(command);
                questInvoker.ExecuteQuest();
            }
        }
    }

    public void SetUpConstructionQuests(string buildingName)
    {
        constructionCommands.Clear(); // 기존 명령 초기화 (선택)
        foreach (var quest in questData.quests.Construction)
        {
            if (quest.buildingName == buildingName) // 매개변수로 들어온 buildingName에 해당하는 퀘스트만
            {
                ConstructionQuestCommand command = new ConstructionQuestCommand(quest, buildingName);
                constructionCommands.Add(command);
            }
        }
    }

    public void SetUpDeliveryQuests(string itemName)
    {
        deliveryCommands.Clear();  // 기존 전달형 퀘스트 초기화

        foreach (var quest in questData.quests.Delivery)
        {
            // 퀘스트 이름에 itemName이 포함되어 있으면 세팅
            if (quest.questName.Contains(itemName))
            {
                string receiverNPC = quest.receiverNPC;  // 아이템을 받을 NPC 이름
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

    // 수집형 퀘스트만 실행
    public void ExecuteCollectQuests()
    {
        foreach (var command in collectCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
        }
    }

    // 대화형 퀘스트만 실행
    public void ExecuteDialogueQuests()
    {
        foreach (var command in dialogueCommands)
        {
            command.Execute();   // questInvoker를 거치지 않고 바로 실행 // 이거 대화형 퀘스트 수정하다가 변경, 나중에 수정해도 가능
        }
    }

    // 건설형 퀘스트만 실행
    public void ExecuteConstructionQuests()
    {
        foreach (var command in constructionCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
            //command.ConstructBuilding();  // 건설 진행하면서 완료까지 되는 함수
        }
    }

    // 전달형 퀘스트만 실행
    public void ExecuteDeliveryQuests()
    {
        foreach (var command in deliveryCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
            //command.DeliverItem(command.GetItemName());
        }
    }

    // 이동형 퀘스트만 실행
    public void ExecuteMovementQuests()
    {
        foreach (var command in movementCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
            //command.OnTriggerEnter(new Collider());
        }
    }

    //  UISwitch에서 호출할 수 있도록 Quest 검색 함수 추가
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