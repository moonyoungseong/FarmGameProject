using UnityEngine;
using System.Linq;

public class QuestInteractionManager : MonoBehaviour
{
    public QuestManager questManager; // QuestManager를 참조하여 퀘스트 시스템과 상호작용
    public QuestListController questListController; // Inspector에서 연결

    //private CollectQuestCommand currentCollectQuest;    // 수집형 퀘스트 

    // 게임 시작 시 호출되는 Start 함수
    void Start()
    {
        // 게임 시작과 함께 대화형 퀘스트 세팅
        //SetUpDialogueQuests("마을이장"); // "마을이장" NPC에 대한 대화형 퀘스트를 설정
        SetUpDialogueQuests(); // "마을이장" NPC에 대한 대화형 퀘스트를 설정
    }

    // 대화형 퀘스트 세팅 함수 (매개변수로 NPC 이름 받음)
    void SetUpDialogueQuests()
    {
        questManager.SetUpDialogueQuests(); // 대화형 퀘스트 설정
    }

    // NPC와 상호작용 시 호출되는 함수
    public void InteractWithNPC(string npcName)
    {
        if (npcName == "마을이장")
        {
            CompleteDialogueQuest(npcName); // 대화형은 상호작용하면 퀘스트 완료
        }
        else if (npcName == "토끼주민")
        {
            CompleteDialogueQuest(npcName); // 대화 완료
        }
        else if (npcName == "토마토농부")    // 수집형 string 버튼에 연결해서 누르면 퀘스트 시작
        {
            SetUpCollectionQuests("토마토");
            questManager.ExecuteCollectQuests();
        }
        else if (npcName == "쌀농부")
        {
            SetUpCollectionQuests("쌀");
            questManager.ExecuteCollectQuests();
        }
        else if (npcName == "옥수수농부")
        {
            SetUpCollectionQuests("옥수수");
            questManager.ExecuteCollectQuests();
        }
        else if (npcName == "마을이장2")
        {
            SetUpConstructionQuests("House_1"); // "집" 또는 "강아지"
            questManager.ExecuteConstructionQuests();
        }
        else if (npcName == "상점")
        {
            SetUpConstructionQuests("Dog"); // "집" 또는 "강아지"
            questManager.ExecuteConstructionQuests();
        }
        else if (npcName == "옥수수농부2")  // 전달형 퀘스트: 고기를 양봉업자에게 전달
        {
            SetUpDeliveryQuests("옥수수");
            questManager.ExecuteDeliveryQuests();
        }
        else if (npcName == "동물사육사")  // 전달형 퀘스트: 치킨을 상점에게 전달
        {
            SetUpDeliveryQuests("고기");
            questManager.ExecuteDeliveryQuests();
        }
        else if (npcName == "마을이장3")  // 이동형 퀘스트: 사다리 타고 올라가서 수리
        {
            SetUpMovementQuests("LadderFixSpot");  // movementTarget 값
            questManager.ExecuteMovementQuests();
        }
        // 다른 NPC의 퀘스트 추가 가능

        //ExecuteQuests();
    }

    //// 수집형 버튼 클릭 시 호출
    //public void CollectCompleteButtonClick(int questId)
    //{
    //    Quest quest = QuestManager.Instance.GetQuestByID(questId);
    //    if (quest != null)
    //    {
    //        // 퀘스트의 아이템 정보 가져오기
    //        string itemName = quest.targetItemName;   // Quest 클래스에 수집 아이템 이름 속성 필요
    //        int requiredAmount = quest.requiredAmount; // Quest 클래스에 수집 개수 속성 필요

    //        CollectQuestCommand command = new CollectQuestCommand(quest, itemName, requiredAmount);

    //        // 버튼 클릭 시 진행중이면 완료 시도
    //        command.TryCompleteQuest();
    //    }
    //    else
    //    {
    //        Debug.LogError($"퀘스트 {questId}를 찾을 수 없습니다.");
    //    }
    //}


    // 대화형 퀘스트 완료 함수
    void CompleteDialogueQuest(string npcName)
    {
        // 대화형 퀘스트 완료
        questManager.CompleteDialogueQuest(npcName);
    }

    // Collection 퀘스트 세팅 함수
    void SetUpCollectionQuests(string itemName)
    {
        // 수집형 퀘스트 세팅
        questManager.SetUpCollectionQuests(itemName);

        foreach (Quest quest in questManager.questData.quests.Collection)
        {
            if (quest.questName.Contains(itemName))
            {
                // QuestListController를 통해 슬롯 해제
                questListController.UnlockQuestSlot(quest.questName);
            }
        }
    }

    // 건설형 퀘스트 세팅
    void SetUpConstructionQuests(string buildingName)
    {
        questManager.SetUpConstructionQuests(buildingName);
    }

    // 전달형 퀘스트 세팅 함수
    void SetUpDeliveryQuests(string itemName)
    {
        questManager.SetUpDeliveryQuests(itemName);
    }

    // 이동형 퀘스트 세팅 함수
    void SetUpMovementQuests(string movementTarget)
    {
        questManager.SetUpMovementQuests(movementTarget);
    }

    // 퀘스트 실행 함수
    void ExecuteQuests()    // 이거 필요없어 보임, 나중에 삭제 하자.
    {
        // 퀘스트 실행
        //questManager.ExecuteQuestsExample();
    }
}
