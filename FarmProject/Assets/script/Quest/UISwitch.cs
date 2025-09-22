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
                    command.Execute(); // ���������� ��ȯ
                    startPanel.SetActive(true);

                    //  ���⼭ UI ����
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

        // --- ������ �� �Ϲ� ����Ʈ ó�� ---
        switch (currentQuest.state)
        {
            case QuestState.NotStarted:
                startPanel.SetActive(true);
                UpdateStartPanel();
                break;

            case QuestState.InProgress:
                HandleInProgressQuest(currentQuest);
                break;

            case QuestState.Completed:
                completePanel.SetActive(true);
                UpdateCompletePanel();
                break;
        }
    }
    /// <summary>
    /// ���� �� ����Ʈ ó��
    /// </summary>
    private void HandleInProgressQuest(Quest quest)
    {
        if (IsCollectionQuest(quest))
        {
            int itemID = 0;
            int requiredQuantity = 3;

            switch (quest.questID)
            {
                case 1: itemID = 3; break; // �丶��
                case 2: itemID = 6; break; // ������
                case 3: itemID = 9; break; // ��
            }

            Item item = InventoryManager.Instance.GetItemByID(itemID.ToString());

            if (item != null && item.quantityInt >= requiredQuantity)
            {
                comQuestPanel.SetActive(true); // �Ϸ� ����
            }
            else
            {
                inProgressPanel.SetActive(true); // ���� ������
            }
        }
        else
        {
            inProgressPanel.SetActive(true); // ������ �ƴ� �� �׳� ������
        }
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
    /// �Ǽ��� ����Ʈ �Ϸ� ��ư Ŭ��
    /// </summary>
    //public void OnConstructionCompleteButton()
    //{
    //    var command = QuestManager.Instance.GetConstructionCommand(currentQuest.buildingName);

    //    if (command == null)
    //    {
    //        Debug.LogError("ConstructionCommand ����!");
    //        return;
    //    }

    //    command.CompleteQuest(); // ������ �� �Ϸ� + ���� ó��
    //    HideAllPanels();
    //    completePanel.SetActive(true);
    //    UpdateCompletePanel();
    //}

    /// <summary>
    /// comQuestPanel �ȿ��� ������ �Ϸ� ��ư Ŭ��
    /// </summary>
    public void ConfirmComplete()
    {
        if (IsCollectionQuest(currentQuest))
        {
            int itemID = 0;
            int requiredQuantity = 3;

            switch (currentQuest.questID)
            {
                case 1: itemID = 3; break; // �丶��
                case 2: itemID = 6; break; // ������
                case 3: itemID = 9; break; // ��
            }

            Item item = InventoryManager.Instance.GetItemByID(itemID.ToString());

            if (item != null && item.quantityInt >= requiredQuantity)
            {
                // ������ �Ҹ�
                CountManager.Instance.RemoveItemByID(itemID.ToString(), requiredQuantity);
                Debug.Log($"����Ʈ �Ϸ�: ������ {item.itemName} {requiredQuantity}�� ����");

                currentQuest.state = QuestState.Completed;
                HideAllPanels();
                completePanel.SetActive(true);
                UpdateCompletePanel();

                // ���� ����
                RewardManager.Instance.GiveRewards(currentQuest.reward);
            }
            else
            {
                Debug.Log("������ ����. ������ �г� ǥ��");
                HideAllPanels();
                inProgressPanel.SetActive(true);
            }

            return;
        }

        // �������� �ƴ� ��� �Ϲ� �Ϸ� ó��
        currentQuest.state = QuestState.Completed;
        HideAllPanels();
        completePanel.SetActive(true);
        UpdateCompletePanel();
        //RewardManager.Instance.GiveRewards(currentQuest.reward);
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
