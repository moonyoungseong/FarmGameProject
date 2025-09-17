using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISwitch : MonoBehaviour
{
    [Header("�г�")]
    public GameObject startPanel;
    public GameObject inProgressPanel;
    public GameObject completePanel;

    [Header("���� �г� UI")]
    public TMP_Text startNpcNameText;
    public TMP_Text startDescriptionText;
    public Image[] startRewardIcons; // 3�� ����

    [Header("�Ϸ� �г� UI")]
    public Image[] completeRewardIcons; // 3�� ����

    private Quest currentQuest;

    /// <summary>
    /// ��ư���� ȣ��: questID�� �´� ����Ʈ�� �ҷ��� �г� ����
    /// </summary>
    public void ShowQuest(int questID)
    {
        // QuestManager���� �ҷ�����
        currentQuest = QuestManager.Instance.GetQuestByID(questID);

        if (currentQuest == null)
        {
            Debug.LogError($" ����Ʈ {questID} �� ã�� �� �����ϴ�.");
            return;
        }

        // ��� �г� ��Ȱ��ȭ
        startPanel.SetActive(false);
        inProgressPanel.SetActive(false);
        completePanel.SetActive(false);

        // ���¿� ���� �г� Ȱ��ȭ
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

    private void UpdateStartPanel()
    {
        startNpcNameText.text = currentQuest.giverNPC;
        startDescriptionText.text = currentQuest.description;

        for (int i = 0; i < startRewardIcons.Length; i++)
        {
            if (i < currentQuest.reward.Count)
            {
                Sprite sprite = Resources.Load<Sprite>($"Icons/{currentQuest.reward[i].icon}");
                if (sprite != null)
                {
                    startRewardIcons[i].sprite = sprite;
                    startRewardIcons[i].gameObject.SetActive(true);
                }
                else
                {
                    startRewardIcons[i].gameObject.SetActive(false);
                }
            }
            else
            {
                startRewardIcons[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateCompletePanel()
    {
        for (int i = 0; i < completeRewardIcons.Length; i++)
        {
            if (i < currentQuest.reward.Count)
            {
                Sprite sprite = Resources.Load<Sprite>($"Icons/{currentQuest.reward[i].icon}");
                if (sprite != null)
                {
                    completeRewardIcons[i].sprite = sprite;
                    completeRewardIcons[i].gameObject.SetActive(true);
                }
                else
                {
                    completeRewardIcons[i].gameObject.SetActive(false);
                }
            }
            else
            {
                completeRewardIcons[i].gameObject.SetActive(false);
            }
        }
    }
}
