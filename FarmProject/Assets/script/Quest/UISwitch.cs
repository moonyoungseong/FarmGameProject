using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UISwitch : MonoBehaviour
{
    public QuestManager questManager;
    public QuestInvoker questInvoker;
    public WindmillInteraction windmillInteraction; // 인스펙터에서 할당

    [Header("패널")]
    public GameObject startPanel;
    public GameObject inProgressPanel;
    public GameObject completePanel;
    public GameObject comQuestPanel;

    [Header("시작 패널 UI")]
    public TMP_Text startNpcNameText;
    public TMP_Text startDescriptionText;
    public Image[] startRewardIcons;

    [Header("완료 패널 UI")]
    public Image[] completeRewardIcons;

    [Header("퀘스트 버튼들")]
    public List<Button> questButtons;

    public Quest currentQuest;

    public void ShowQuest(int questID)
    {
        currentQuest = QuestManager.Instance.GetQuestByID(questID);
        if (currentQuest == null)
        {
            Debug.LogError($"퀘스트 {questID}를 찾을 수 없습니다.");
            return;
        }

        HideAllPanels();

        if (!questManager.questCommands.TryGetValue(currentQuest.questID, out var command))
        {
            Debug.LogError($"Command 없음: {currentQuest.questName}");
            return;
        }

        questInvoker.SetQuestCommand(command);

        switch (currentQuest.state)
        {
            case QuestState.NotStarted:
                questInvoker.ExecuteQuest(); // NotStarted → InProgress 퀘스트 수락
                startPanel.SetActive(true);
                UpdateStartPanel();
                break;

            case QuestState.InProgress:
                // 수집형/전달형 퀘스트 처리 가능 여부
                bool canComplete = false;
                if (command is CollectQuestCommand collectCommand)
                    canComplete = collectCommand.CanComplete();
                else if (command is DeliveryQuestCommand deliveryCommand)
                    canComplete = InventoryManager.Instance.GetItemByName(deliveryCommand.ItemName) != null;
                else if (command is ConstructionQuestCommand constructionCommand)
                    canComplete = constructionCommand.CanComplete();
                else if (command is MovementQuestCommand moveCommand && windmillInteraction.repairDone == true)
                    canComplete = true; 

                if (canComplete)
                    comQuestPanel.SetActive(true);
                else
                    inProgressPanel.SetActive(true);
                break;

            case QuestState.Completed:
                completePanel.SetActive(true);
                UpdateCompletePanel();
                break;
        }
    }
    public void ConfirmComplete()
    {
        if (currentQuest == null) return;
        if (!questManager.questCommands.TryGetValue(currentQuest.questID, out var command))
        {
            Debug.LogError($"Command 없음: {currentQuest.questName}");
            return;
        }

        questInvoker.SetQuestCommand(command);

        // 수집형/전달형 체크
        bool canComplete = false;
        if (command is CollectQuestCommand collectCommand)
            canComplete = collectCommand.CanComplete();
        else if (command is DeliveryQuestCommand deliveryCommand)
            canComplete = InventoryManager.Instance.GetItemByName(deliveryCommand.ItemName) != null;
        else if (command is MovementQuestCommand moveCommand && moveCommand.IsArrived())
            canComplete = moveCommand.IsArrived(); //  목표 도달 여부 확인
        else
            canComplete = true; // 대화형/건설형

        if (canComplete)
        {
            command.Execute();      // 진행 중 상태 확인
            command.CompleteQuest(); // 완료 처리
            HideAllPanels();
            completePanel.SetActive(true);
            UpdateCompletePanel();
        }
        else
        {
            inProgressPanel.SetActive(true);
        }
    }

    public void InteractWithNPC(string npcName)
    {
        Quest dialogueQuest = questManager.questData.quests.Dialogue.Find(q => q.giverNPC == npcName);
        if (dialogueQuest == null)
        {
            Debug.Log($"[{npcName}] 관련 대화형 퀘스트 없음");
            return;
        }

        if (!questManager.questCommands.TryGetValue(dialogueQuest.questID, out var command))
        {
            Debug.LogError($"Command 없음: {dialogueQuest.questName}");
            return;
        }

        questInvoker.SetQuestCommand(command);

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

    private void HideAllPanels()
    {
        startPanel.SetActive(false);
        inProgressPanel.SetActive(false);
        completePanel.SetActive(false);
        comQuestPanel.SetActive(false);
    }

    public void ShowCurrentQuestButton()
    {
        if (questButtons == null || questButtons.Count == 0 || currentQuest == null) return;

        for (int i = 0; i < questButtons.Count; i++)
        {
            questButtons[i].gameObject.SetActive(currentQuest.questID == i + 1);
        }
    }
}
