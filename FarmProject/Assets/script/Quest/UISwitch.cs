using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISwitch : MonoBehaviour
{
    [Header("패널")]
    public GameObject startPanel;
    public GameObject inProgressPanel;
    public GameObject completePanel;

    [Header("시작 패널 UI")]
    public TMP_Text startNpcNameText;
    public TMP_Text startDescriptionText;
    public Image[] startRewardIcons; // 3개 고정

    [Header("완료 패널 UI")]
    public Image[] completeRewardIcons; // 3개 고정

    private Quest currentQuest;

    /// <summary>
    /// 버튼에서 호출: questID에 맞는 퀘스트를 불러와 패널 갱신
    /// </summary>
    public void ShowQuest(int questID)
    {
        // QuestManager에서 불러오기
        currentQuest = QuestManager.Instance.GetQuestByID(questID);

        if (currentQuest == null)
        {
            Debug.LogError($" 퀘스트 {questID} 를 찾을 수 없습니다.");
            return;
        }

        // 모든 패널 비활성화
        startPanel.SetActive(false);
        inProgressPanel.SetActive(false);
        completePanel.SetActive(false);

        // 상태에 따라 패널 활성화
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
