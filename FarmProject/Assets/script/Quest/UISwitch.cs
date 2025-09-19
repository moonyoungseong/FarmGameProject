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
    public GameObject comQuestPanel;    // �Ϸ� ���� Ȯ���ϴ� �г�

    [Header("���� �г� UI")]
    public TMP_Text startNpcNameText;
    public TMP_Text startDescriptionText;
    public Image[] startRewardIcons; // 3�� ����

    [Header("�Ϸ� �г� UI")]
    public Image[] completeRewardIcons; // 3�� ����

    [Header("����Ʈ ��ư��")]
    public List<Button> questButtons; // Inspector���� ��ư 10�� ����

    public Quest currentQuest;

    /// <summary>
    /// ��ư���� ȣ��: questID�� �´� ����Ʈ�� �ҷ��� �г� ����
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
    /// ������ ����Ʈ ó��
    /// ����Ʈ ������ ���� �Լ��� ����
    /// </summary>
    private void HandleInProgressQuest(Quest quest)
    {
        bool canComplete = false;

        // ������ ���� üũ
        if (IsCollectionQuest(quest))
            canComplete = CheckCollectionQuest(quest);

        // �ٸ� ����Ʈ ���� ���� �߰� ����
        // else if (IsDialogueQuest(quest))
        //     canComplete = CheckDialogueQuest(quest);

        if (canComplete)
            comQuestPanel.SetActive(true);  // �Ϸ� ���� �г�
        else
            inProgressPanel.SetActive(true); // ������ �г�
    }

    #region ����Ʈ ���� üũ �Լ�

    private bool IsCollectionQuest(Quest quest)
    {
        // ������ ����Ʈ ID ��� ����
        int[] collectionIDs = { 1, 2, 3 }; // �丶��, ������, �� ��
        return System.Array.Exists(collectionIDs, id => id == quest.questID);
    }

    private bool CheckCollectionQuest(Quest quest)
    {
        int requiredItemID = 0;
        int requiredQuantity = 0;

        switch (quest.questID)
        {
            case 1: // �丶�� 3��
                requiredItemID = 3;
                requiredQuantity = 3;
                break;
            case 2: // ������ 3��
                requiredItemID = 6;
                requiredQuantity = 3;
                break;
            case 3: // �� 3��
                requiredItemID = 9;
                requiredQuantity = 3;
                break;
        }

        if (requiredItemID == 0) return false;

        Item item = InventoryManager.Instance.GetItemByID(requiredItemID.ToString());
        return item != null && item.quantityInt >= requiredQuantity;
    }

    #endregion

    /// <summary>
    /// comQuestPanel �ȿ��� '�Ϸ�' ��ư�� ������ ��
    /// </summary>
    public void ConfirmComplete()
    {
        HideAllPanels();
        currentQuest.state = QuestState.Completed;
        completePanel.SetActive(true);
        UpdateCompletePanel();
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
    /// ���� ��ư���� ȣ��
    /// currentQuest.questID �������� ��ư Ȱ��ȭ
    /// </summary>
    public void ShowCurrentQuestButton()
    {
        if (questButtons == null || questButtons.Count == 0) return;

        for (int i = 0; i < questButtons.Count; i++)
        {
            if (currentQuest != null && currentQuest.questID == i + 1) // ID�� 1~10 ����
                questButtons[i].gameObject.SetActive(true);
            else
                questButtons[i].gameObject.SetActive(false);
        }
    }
}
