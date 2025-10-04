using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UISwitch : MonoBehaviour
{
    [Header("�г�")]
    public GameObject startPanel;
    public GameObject inProgressPanel;
    public GameObject completePanel;
    public GameObject comQuestPanel; // ������ �Ǵ� �Ǽ��� �Ϸ� �� Ȯ�� �г�

    [Header("���� �г� UI")]
    public TMP_Text startNpcNameText;
    public TMP_Text startDescriptionText;
    public Image[] startRewardIcons; // �ִ� 3��

    [Header("�Ϸ� �г� UI")]
    public Image[] completeRewardIcons; // �ִ� 3��

    [Header("����Ʈ ��ư��")]
    public List<Button> questButtons;

    public Quest currentQuest;

    /// <summary>
    /// ����Ʈ ���� �� ȣ��
    /// </summary>
    public void ShowQuest(int questID)
    {
        currentQuest = QuestManager.Instance.GetQuestByID(questID);

        if (currentQuest == null)
        {
            Debug.LogError($"����Ʈ {questID}�� ã�� �� �����ϴ�.");
            return;
        }

        HideAllPanels();

        // --- �Ǽ��� ����Ʈ ó�� ---
        if (!IsCollectionQuest(currentQuest) && !string.IsNullOrEmpty(currentQuest.buildingName))
        {
            QuestManager.Instance.SetUpConstructionQuests(currentQuest.buildingName);
            var command = QuestManager.Instance.GetConstructionCommand(currentQuest.buildingName);

            if (command == null)
            {
                Debug.LogError($"ConstructionCommand ����: {currentQuest.buildingName}");
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

        // --- ������ ����Ʈ ó�� ---
        if (IsCollectionQuest(currentQuest))
        {
            QuestManager.Instance.SetUpCollectionQuests(currentQuest.questName);
            //var command = QuestManager.Instance.GetCollectCommand(currentQuest.questID);
            var command = new CollectQuestCommand(currentQuest, currentQuest.itemName, currentQuest.requiredAmount);


            if (command == null)
            {
                Debug.LogError($"CollectQuestCommand ����: {currentQuest.questName}");
                return;
            }

            // �׻� �ֽ� �κ��丮 �ݿ�
            command.SyncWithInventory();

            // NotStarted �����̸� ����
            if (currentQuest.state == QuestState.NotStarted)
            {
                command.Execute(); // ���������� ��ȯ
                startPanel.SetActive(true);
                UpdateStartPanel();
            }
            // ���� ���̸� ���� �κ��丮 ���� �������� UI �б�
            else if (currentQuest.state == QuestState.InProgress)
            {
                // CanComplete()�� true�� �Ϸ� ���� �г�, �ƴϸ� ������ �г�
                if (command.CanComplete())
                {
                    comQuestPanel.SetActive(true);
                    Debug.Log($"[UI] {currentQuest.questName} - �Ϸ� ���� �г� ǥ��");
                }
                else
                {
                    inProgressPanel.SetActive(true);
                    Debug.Log($"[UI] {currentQuest.questName} - ������ �г� ǥ��");
                }
            }
            // �̹� �Ϸ�� ����Ʈ
            else if (currentQuest.state == QuestState.Completed)
            {
                completePanel.SetActive(true);
                UpdateCompletePanel();
            }

            return;
        }

        // --- �Ϲ� ����Ʈ ó�� ---
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
    /// ������ ����Ʈ Ȯ�� ��ư
    /// </summary>
    public void ConfirmComplete()
    {
        if (currentQuest == null) return;

        // currentQuest ��ü�� �Ǵ�
        // ������
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
                Debug.Log("������ ����Ʈ: ������ ����. ������ �г� ǥ��");
                HideAllPanels();
                inProgressPanel.SetActive(true);
            }
            return;
        }

        // �Ǽ���
        if (!string.IsNullOrEmpty(currentQuest.buildingName))
        {
            var command = new ConstructionQuestCommand(currentQuest, currentQuest.buildingName);
            command.CompleteQuest();
            HideAllPanels();
            completePanel.SetActive(true);
            UpdateCompletePanel();
            return;
        }


        //// ��ȭ��
        //if (IsDialogueQuest(currentQuest))
        //{
        //    var command = new DialogueQuestCommand(currentQuest);
        //    command.CompleteQuest();
        //    HideAllPanels();
        //    completePanel.SetActive(true);
        //    UpdateCompletePanel();
        //    return;
        //}

        //// ������
        //if (IsDeliveryQuest(currentQuest))
        //{
        //    var command = new DeliveryQuestCommand(currentQuest);
        //    command.CompleteQuest();
        //    HideAllPanels();
        //    completePanel.SetActive(true);
        //    UpdateCompletePanel();
        //    return;
        //}

        //// �̵���
        //if (IsMovementQuest(currentQuest))
        //{
        //    var command = new MovementQuestCommand(currentQuest);
        //    command.CompleteQuest();
        //    HideAllPanels();
        //    completePanel.SetActive(true);
        //    UpdateCompletePanel();
        //    return;
        //}

        // Ŀ�ǵ� ���� ���
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
    /// ���� ����Ʈ ��ư Ȱ��ȭ
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
