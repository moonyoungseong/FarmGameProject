using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UISwitch.cs
/// 퀘스트의 상태에 따라 적절한 UI 패널을 표시하고
/// 보상 아이콘, 퀘스트 상태 전환 등을 처리하는 퀘스트 UI 총괄 관리자
/// 
/// - ShowQuest() : 퀘스트 ID로 패널 출력
/// - ConfirmComplete() : 퀘스트 완료 버튼 처리
/// - InteractWithNPC() : NPC와 상호작용 시 대화형 퀘스트 처리
/// - UpdateStartPanel / UpdateCompletePanel : 보상 아이콘 갱신
/// - ShowCurrentQuestButton : 선택된 퀘스트 버튼만 활성화
/// QuestManager, QuestInvoker, CountManager 등 여러 시스템과 연결됨
/// </summary>
public class UISwitch : MonoBehaviour
{
    // 퀘스트 데이터를 관리하는 매니저
    public QuestManager questManager;

    // Command 패턴으로 퀘스트 실행을 담당
    public QuestInvoker questInvoker;

    // 이동형 퀘스트 완료 여부 확인용
    public WindmillInteraction windmillInteraction;

    [Header("패널")]
    public GameObject startPanel;       // 퀘스트 시작 UI 패널
    public GameObject inProgressPanel;  // 퀘스트 진행 중 패널
    public GameObject completePanel;    // 퀘스트 완료 패널
    public GameObject comQuestPanel;    // 완료 가능 패널 

    [Header("시작 패널 UI")]
    public TMP_Text startNpcNameText;        // NPC 이름
    public TMP_Text startDescriptionText;    // 퀘스트 설명
    public Image[] startRewardIcons;         // 보상 아이콘들

    [Header("완료 패널 UI")]
    public Image[] completeRewardIcons;      // 완료 후 보상 아이콘들

    [Header("퀘스트 버튼들")]
    public List<Button> questButtons;

    // 현재 플레이어 퀘스트
    public Quest currentQuest;

    /// <summary>
    /// 퀘스트 ID를 전달받아 해당 퀘스트의 상태에 맞는 패널을 출력한다.
    /// - NotStarted → 시작 패널
    /// - InProgress → 완료 가능 패널
    /// - Completed → 완료 패널
    /// </summary>
    public void ShowQuest(int questID)
    {
        currentQuest = QuestManager.Instance.GetQuestByID(questID);

        if (currentQuest == null)
        {
            Debug.LogError($"퀘스트 {questID}를 찾을 수 없습니다.");
            return;
        }

        HideAllPanels(); // 모든 패널 숨김

        // 해당 퀘스트의 Command 가져오기
        if (!questManager.questCommands.TryGetValue(currentQuest.questID, out var command))
        {
            Debug.LogError($"Command 없음: {currentQuest.questName}");
            return;
        }

        questInvoker.SetQuestCommand(command);

        // 상태별 UI 분기 처리
        switch (currentQuest.state)
        {
            case QuestState.NotStarted:
                // 수락 즉시 상태 전환 (Command.Execute 내부에서 처리)
                questInvoker.ExecuteQuest();
                startPanel.SetActive(true);
                UpdateStartPanel();
                break;

            case QuestState.InProgress:
                bool canComplete = false;

                // 수집형
                if (command is CollectQuestCommand collectCommand)
                    canComplete = collectCommand.CanComplete();

                // 전달형
                else if (command is DeliveryQuestCommand deliveryCommand)
                    canComplete = InventoryManager.Instance.GetItemByName(deliveryCommand.ItemName) != null;

                // 건설형
                else if (command is ConstructionQuestCommand constructionCommand)
                    canComplete = constructionCommand.CanComplete();

                // 이동형
                else if (command is MovementQuestCommand moveCommand && windmillInteraction.repairDone)
                    canComplete = true;

                // UI 표시 + 효과음
                if (canComplete)
                {
                    comQuestPanel.SetActive(true);
                    AudioManager.Instance.PlaySFX(0); // 완료 가능 효과음
                }
                else
                {
                    inProgressPanel.SetActive(true);
                    AudioManager.Instance.PlaySFX(8); // 미완료 효과음
                }
                break;

            case QuestState.Completed:
                completePanel.SetActive(true);
                UpdateCompletePanel();
                break;
        }
    }

    /// <summary>
    /// 완료 버튼을 눌렀을 때 퀘스트가 조건을 충족했는지 검사 후 처리
    /// - 조건 충족 → Execute + CompleteQuest
    /// - 미충족 → InProgressPanel 유지
    /// </summary>
    public void ConfirmComplete()
    {
        if (currentQuest == null) return;

        if (!questManager.questCommands.TryGetValue(currentQuest.questID, out var command))
        {
            Debug.LogError($"Command 없음: {currentQuest.questName}");
            return;
        }

        questInvoker.SetQuestCommand(command);

        bool canComplete = false;

        // 수집형
        if (command is CollectQuestCommand collectCommand)
            canComplete = collectCommand.CanComplete();

        // 전달형
        else if (command is DeliveryQuestCommand deliveryCommand)
            canComplete = InventoryManager.Instance.GetItemByName(deliveryCommand.ItemName) != null;

        // 이동형
        else if (command is MovementQuestCommand moveCommand && moveCommand.IsArrived())
            canComplete = moveCommand.IsArrived();

        // 나머지(대화/건설형)
        else
            canComplete = true;

        // =============== 완료 가능 ==================
        if (canComplete)
        {
            command.Execute();       // 상태 확인
            command.CompleteQuest(); // 완료 처리

            HideAllPanels();
            completePanel.SetActive(true);       // 완료 UI 출력
            UpdateCompletePanel();
        }
        else
        {
            inProgressPanel.SetActive(true);
        }
    }

    // NPC와 대화했을 때 해당 NPC 관련 Dialogue 퀘스트를 자동 진행
    public void InteractWithNPC(string npcName)
    {
        // NPC가 giverNPC인 대화형 퀘스트 찾기
        Quest dialogueQuest = questManager.questData.quests.Dialogue.Find(q => q.giverNPC == npcName);

        if (dialogueQuest == null)
        {
            return;
        }

        // command 가져오기
        if (!questManager.questCommands.TryGetValue(dialogueQuest.questID, out var command))
        {
            Debug.LogError($"Command 없음: {dialogueQuest.questName}");
            return;
        }

        questInvoker.SetQuestCommand(command);

        // 상태별 처리
        if (dialogueQuest.state == QuestState.NotStarted)
        {
            questInvoker.ExecuteQuest(); // 진행중으로 전환
            startPanel.SetActive(true);
            currentQuest = dialogueQuest;
            UpdateStartPanel();
        }
        else if (dialogueQuest.state == QuestState.InProgress)
        {
            command.CompleteQuest();
            HideAllPanels();
            completePanel.SetActive(true);
            currentQuest = dialogueQuest;
            UpdateCompletePanel();
        }
        else
        {
            completePanel.SetActive(true);
            currentQuest = dialogueQuest;
            UpdateCompletePanel();
        }
    }

    //  시작 패널 업데이트 (NPC명 + 설명 + 보상)
    private void UpdateStartPanel()
    {
        if (currentQuest == null) return;

        startNpcNameText.text = currentQuest.giverNPC;
        startDescriptionText.text = currentQuest.description;

        for (int i = 0; i < startRewardIcons.Length; i++)
        {
            if (i < currentQuest.reward.Count)
            {
                Sprite sprite = Resources.Load<Sprite>($"Icons/{currentQuest.reward[i].icon}");
                startRewardIcons[i].sprite = sprite;
                startRewardIcons[i].gameObject.SetActive(sprite != null);
            }
            else
            {
                startRewardIcons[i].gameObject.SetActive(false);
            }
        }
    }

    //  완료 패널 업데이트 (보상 아이콘)
    private void UpdateCompletePanel()
    {
        if (currentQuest == null) return;

        for (int i = 0; i < completeRewardIcons.Length; i++)
        {
            if (i < currentQuest.reward.Count)
            {
                Sprite sprite = Resources.Load<Sprite>($"Icons/{currentQuest.reward[i].icon}");
                completeRewardIcons[i].sprite = sprite;
                completeRewardIcons[i].gameObject.SetActive(sprite != null);
            }
            else
            {
                completeRewardIcons[i].gameObject.SetActive(false);
            }
        }
    }

    // UI 상태 전환 시 모든 패널을 비활성화
    private void HideAllPanels()
    {
        startPanel.SetActive(false);
        inProgressPanel.SetActive(false);
        completePanel.SetActive(false);
        comQuestPanel.SetActive(false);
    }

    // 퀘스트 버튼 리스트 중 "현재 선택된 퀘스트" 번호만 활성화한다.
    public void ShowCurrentQuestButton()
    {
        if (questButtons == null || questButtons.Count == 0 || currentQuest == null) return;

        for (int i = 0; i < questButtons.Count; i++)
        {
            questButtons[i].gameObject.SetActive(currentQuest.questID == i + 1);
        }
    }
}
