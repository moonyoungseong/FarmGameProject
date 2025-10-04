using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UISwitch : MonoBehaviour
{
    [Header("패널")]
    public GameObject startPanel;
    public GameObject inProgressPanel;
    public GameObject completePanel;
    public GameObject comQuestPanel; // 수집형 또는 건설형 완료 전 확인 패널

    [Header("시작 패널 UI")]
    public TMP_Text startNpcNameText;
    public TMP_Text startDescriptionText;
    public Image[] startRewardIcons; // 최대 3개

    [Header("완료 패널 UI")]
    public Image[] completeRewardIcons; // 최대 3개

    [Header("퀘스트 버튼들")]
    public List<Button> questButtons;

    public Quest currentQuest;

    /// <summary>
    /// 퀘스트 선택 시 호출
    /// </summary>
    public void ShowQuest(int questID)
    {
        currentQuest = QuestManager.Instance.GetQuestByID(questID);

        if (currentQuest == null)
        {
            Debug.LogError($"퀘스트 {questID}를 찾을 수 없습니다.");
            return;
        }

        HideAllPanels();

        // --- 건설형 퀘스트 처리 ---
        if (!IsCollectionQuest(currentQuest) && !string.IsNullOrEmpty(currentQuest.buildingName))
        {
            QuestManager.Instance.SetUpConstructionQuests(currentQuest.buildingName);
            var command = QuestManager.Instance.GetConstructionCommand(currentQuest.buildingName);

            if (command == null)
            {
                Debug.LogError($"ConstructionCommand 없음: {currentQuest.buildingName}");
                return;
            }

            switch (currentQuest.state)
            {
                case QuestState.NotStarted:
                    command.Execute();
                    startPanel.SetActive(true);
                    UpdateStartPanel();
                    break;

                case QuestState.InProgress:
                    if (command.CheckConstruction())
                        comQuestPanel.SetActive(true);
                    else
                        inProgressPanel.SetActive(true);
                    break;

                case QuestState.Completed:
                    completePanel.SetActive(true);
                    UpdateCompletePanel();
                    break;
            }
            return;
        }

        // --- 수집형 퀘스트 처리 ---
        if (IsCollectionQuest(currentQuest))
        {
            QuestManager.Instance.SetUpCollectionQuests(currentQuest.questName);
            //var command = QuestManager.Instance.GetCollectCommand(currentQuest.questID);
            var command = new CollectQuestCommand(currentQuest, currentQuest.itemName, currentQuest.requiredAmount);


            if (command == null)
            {
                Debug.LogError($"CollectQuestCommand 없음: {currentQuest.questName}");
                return;
            }

            // 항상 최신 인벤토리 반영
            command.SyncWithInventory();

            // NotStarted 상태이면 실행
            if (currentQuest.state == QuestState.NotStarted)
            {
                command.Execute(); // 진행중으로 전환
                startPanel.SetActive(true);
                UpdateStartPanel();
            }
            // 진행 중이면 실제 인벤토리 수량 기준으로 UI 분기
            else if (currentQuest.state == QuestState.InProgress)
            {
                // CanComplete()가 true면 완료 가능 패널, 아니면 진행중 패널
                if (command.CanComplete())
                {
                    comQuestPanel.SetActive(true);
                    Debug.Log($"[UI] {currentQuest.questName} - 완료 가능 패널 표시");
                }
                else
                {
                    inProgressPanel.SetActive(true);
                    Debug.Log($"[UI] {currentQuest.questName} - 진행중 패널 표시");
                }
            }
            // 이미 완료된 퀘스트
            else if (currentQuest.state == QuestState.Completed)
            {
                completePanel.SetActive(true);
                UpdateCompletePanel();
            }

            return;
        }

        // --- 일반 퀘스트 처리 ---
        switch (currentQuest.state)
        {
            case QuestState.NotStarted:
                startPanel.SetActive(true);
                UpdateStartPanel();
                break;

            case QuestState.InProgress:
                inProgressPanel.SetActive(true);
                break;

            case QuestState.Completed:
                completePanel.SetActive(true);
                UpdateCompletePanel();
                break;
        }
    }
    /// <summary>
    /// 수집형 퀘스트 확인 버튼
    /// </summary>
    public void ConfirmComplete()
    {
        if (currentQuest == null) return;

        // currentQuest 자체로 판단
        // 수집형
        if (IsCollectionQuest(currentQuest))
        {
            var command = new CollectQuestCommand(currentQuest, currentQuest.itemName, currentQuest.requiredAmount);

            if (command.CanComplete())
            {
                command.CompleteQuest();
                HideAllPanels();
                completePanel.SetActive(true);
                UpdateCompletePanel();
            }
            else
            {
                Debug.Log("수집형 퀘스트: 아이템 부족. 진행중 패널 표시");
                HideAllPanels();
                inProgressPanel.SetActive(true);
            }
            return;
        }

        // 건설형
        if (!string.IsNullOrEmpty(currentQuest.buildingName))
        {
            var command = new ConstructionQuestCommand(currentQuest, currentQuest.buildingName);
            command.CompleteQuest();
            HideAllPanels();
            completePanel.SetActive(true);
            UpdateCompletePanel();
            return;
        }


        //// 대화형
        //if (IsDialogueQuest(currentQuest))
        //{
        //    var command = new DialogueQuestCommand(currentQuest);
        //    command.CompleteQuest();
        //    HideAllPanels();
        //    completePanel.SetActive(true);
        //    UpdateCompletePanel();
        //    return;
        //}

        //// 전달형
        //if (IsDeliveryQuest(currentQuest))
        //{
        //    var command = new DeliveryQuestCommand(currentQuest);
        //    command.CompleteQuest();
        //    HideAllPanels();
        //    completePanel.SetActive(true);
        //    UpdateCompletePanel();
        //    return;
        //}

        //// 이동형
        //if (IsMovementQuest(currentQuest))
        //{
        //    var command = new MovementQuestCommand(currentQuest);
        //    command.CompleteQuest();
        //    HideAllPanels();
        //    completePanel.SetActive(true);
        //    UpdateCompletePanel();
        //    return;
        //}

        // 커맨드 없는 경우
        currentQuest.state = QuestState.Completed;
        HideAllPanels();
        completePanel.SetActive(true);
        UpdateCompletePanel();
    }



    private bool IsCollectionQuest(Quest quest)
    {
        int[] collectionIDs = { 1, 2, 3 };
        return System.Array.Exists(collectionIDs, id => id == quest.questID);
    }

    private void HideAllPanels()
    {
        startPanel.SetActive(false);
        inProgressPanel.SetActive(false);
        completePanel.SetActive(false);
        comQuestPanel.SetActive(false);
    }

    private void UpdateStartPanel()
    {
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
                startRewardIcons[i].gameObject.SetActive(false);
        }
    }

    private void UpdateCompletePanel()
    {
        for (int i = 0; i < completeRewardIcons.Length; i++)
        {
            if (i < currentQuest.reward.Count)
            {
                Sprite sprite = Resources.Load<Sprite>($"Icons/{currentQuest.reward[i].icon}");
                completeRewardIcons[i].sprite = sprite;
                completeRewardIcons[i].gameObject.SetActive(sprite != null);
            }
            else
                completeRewardIcons[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 현재 퀘스트 버튼 활성화
    /// </summary>
    public void ShowCurrentQuestButton()
    {
        if (questButtons == null || questButtons.Count == 0) return;

        for (int i = 0; i < questButtons.Count; i++)
        {
            questButtons[i].gameObject.SetActive(currentQuest != null && currentQuest.questID == i + 1);
        }
    }
}
