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
    public string description;  // 설명
    public string giverNPC;
    public string receiverNPC;
    public List<QuestObjective> objectives;     // 퀘스트 해야할것
    public List<Reward> reward;
    public int levelRequirement; // 요구되는 레벨
}

[System.Serializable]
public class QuestCategory
{
    public List<Quest> Collection; // 수집형
    public List<Quest> Dialogue; // 대화형
    public List<Quest> Construction; // 건설형
    public List<Quest> Delivery; // 전달형
    public List<Quest> Movement; // 이동형
}

[System.Serializable]
public class Reward     // 보상
{
    public int itemID;
    public string icon;
}

[System.Serializable]
public class QuestData  // QuestCategory가 포함된 퀘스트 전체 데이터를 저장
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

    void Start()    // 주석쳐서 아직 실행안함, 퀘스트 시스템 아직 못 만듬
    {
        LoadQuestData();
        //SetUpCollectionQuests();  // 수집형 퀘스트 세팅 - 세팅을 해야 실행이 정상적으로 작동된다.
        //SetUpDialogueQuests();
        //SetUpConstructionQuests()
        //SetUpDeliveryQuests();
        //SetUpMovementQuests();
        ExecuteQuestsExample();     // 퀘스트 실행 코드 
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

    void SetUpCollectionQuests()     // 퀘스트 이름에 토마토 적히면 토마토, 다른거 적히면 다른 퀘스트 수집이 됨
    {
        foreach (var quest in questData.quests.Collection)
        {
            string itemName = quest.questName.Contains("토마토") ? "토마토" :
                              quest.questName.Contains("옥수수") ? "옥수수" : "쌀";

            CollectQuestCommand command = new CollectQuestCommand(quest, itemName, 3);
            collectCommands.Add(command);
        }
    }

    void SetUpDialogueQuests()
    {
        foreach (var quest in questData.quests.Dialogue)
        {
            string npcName = quest.giverNPC; // 대화할 NPC 이름
            DialogueQuestCommand command = new DialogueQuestCommand(quest, npcName);
            dialogueCommands.Add(command);
        }
    }

    void SetUpConstructionQuests()
    {
        foreach (var quest in questData.quests.Construction)
        {
            string buildingName = quest.questName.Contains("집") ? "집" :
                                  quest.questName.Contains("강아지") ? "강아지" : "기타 건물";

            ConstructionQuestCommand command = new ConstructionQuestCommand(quest, buildingName);
            constructionCommands.Add(command);
        }
    }

    void SetUpDeliveryQuests()
    {
        foreach (var quest in questData.quests.Delivery)
        {
            // 퀘스트 이름에 맞는 전달 물건을 설정
            string itemName = quest.questName.Contains("고기") ? "고기" :
                              quest.questName.Contains("치킨") ? "치킨" :
                              quest.questName.Contains("물") ? "물" : "기타";

            string receiverNPC = quest.receiverNPC;  // 아이템을 받을 NPC 이름
            DeliveryQuestCommand command = new DeliveryQuestCommand(quest, itemName, 3, receiverNPC);
            deliveryCommands.Add(command);
        }
    }

    void SetUpMovementQuests()
    {
        foreach (var quest in questData.quests.Movement)  // Movement 퀘스트로 변경
        {
            string receiverNPC = quest.receiverNPC;  // 이동할 NPC 이름
            MovementQuestCommand command = new MovementQuestCommand(quest, receiverNPC);
            movementCommands.Add(command);  // movementCommands 리스트에 추가
        }
    }


    void ExecuteQuestsExample()     // 이 함수가 실제로 퀘스트를 실행하는 코드 .ExecuteQuest()
    {
        // 수집형 퀘스트 실행 예제
        foreach (var command in collectCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();
        }

        // 대화형 퀘스트 실행 예제
        foreach (var command in dialogueCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();

            // 대화 후 퀘스트 완료 처리
            command.CompleteQuest();
        }

        // 건설형 퀘스트 실행 예제
        foreach (var command in constructionCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();

            // 건설 후 퀘스트 완료 처리
            command.ConstructBuilding();
        }

        // 전달형 퀘스트 실행 예제
        foreach (var command in deliveryCommands)
        {
            questInvoker.SetQuestCommand(command);
            questInvoker.ExecuteQuest();

            // 전달할 물건 이름을 명시하여 퀘스트 완료 처리
            command.DeliverItem(command.GetItemName());  // GetItemName()은 아이템 이름을 반환하는 메서드라고 가정
        }

        // Movement형 퀘스트 실행 예제
        foreach (var command in movementCommands)
        {
            questInvoker.SetQuestCommand(command); // 퀘스트 명령 설정
            questInvoker.ExecuteQuest();  // 퀘스트 실행

            // 목표 지점에 도달 후 퀘스트 완료 처리
            command.OnTriggerEnter(new Collider());  // 트리거를 통해 퀘스트 완료 (플레이어가 트리거 지점에 도달)
        }
    }
}