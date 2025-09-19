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
    public GameObject comQuestPanel; // ������ �Ϸ� �� Ȯ�� �г�

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
            int requiredQuantity = 3; // ������ ����Ʈ ���� ����

            switch (quest.questID)
            {
                case 1: itemID = 3; break; // �丶��
                case 2: itemID = 6; break; // ������
                case 3: itemID = 9; break; // ��
            }

            Item item = InventoryManager.Instance.GetItemByID(itemID.ToString());

            if (item != null && item.quantityInt >= requiredQuantity)
            {
                // ����ϸ� �Ϸ� Ȯ�� �г� ǥ��
                comQuestPanel.SetActive(true);
            }
            else
            {
                // �����ϸ� ������ �г� ǥ��
                inProgressPanel.SetActive(true);
            }
        }
        else
        {
            // �������� �ƴϸ� �׳� ������ �г�
            inProgressPanel.SetActive(true);
        }
    }

    private bool IsCollectionQuest(Quest quest)
    {
        int[] collectionIDs = { 1, 2, 3 }; // �丶��, ������, ��
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
    /// comQuestPanel �ȿ��� '�Ϸ�' ��ư�� ������ ��
    /// </summary>
    public void ConfirmComplete()
    {
        if (IsCollectionQuest(currentQuest))
        {
            int itemID = 0;
            int requiredQuantity = 3; // �ʿ��� ����
            switch (currentQuest.questID)
            {
                case 1: itemID = 3; break; // �丶��
                case 2: itemID = 6; break; // ������
                case 3: itemID = 9; break; // ��
            }

            Item item = InventoryManager.Instance.GetItemByID(itemID.ToString());

            if (item != null && item.quantityInt >= requiredQuantity)
            {
                // ����ϸ� �Ҹ� �� �Ϸ� ó��
                CountManager.Instance.RemoveItemByID(itemID.ToString(), requiredQuantity);
                Debug.Log($"����Ʈ �Ϸ�: ������ {item.itemName} {requiredQuantity}�� �Һ��");

                // ����Ʈ ���� �Ϸ�
                currentQuest.state = QuestState.Completed;

                HideAllPanels();
                completePanel.SetActive(true);
                UpdateCompletePanel();

                // ���� ����
                RewardManager.Instance.GiveRewards(currentQuest.reward);
            }
            else
            {
                // �����ϸ� ������ �гη� ���ư���
                Debug.Log("������ ����. ������ �г� ǥ��.");
                HideAllPanels();
                inProgressPanel.SetActive(true);
            }

            return; // ������ ����Ʈ ó�� �Ϸ�
        }

        // �������� �ƴ� ��� �Ϲ� �Ϸ� ó��
        currentQuest.state = QuestState.Completed;
        HideAllPanels();
        completePanel.SetActive(true);
        UpdateCompletePanel();
        RewardManager.Instance.GiveRewards(currentQuest.reward);
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
