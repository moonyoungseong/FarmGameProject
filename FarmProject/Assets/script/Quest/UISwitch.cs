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
    public GameObject comQuestPanel;    // 완료 전에 확인하는 패널

    [Header("시작 패널 UI")]
    public TMP_Text startNpcNameText;
    public TMP_Text startDescriptionText;
    public Image[] startRewardIcons; // 3개 고정

    [Header("완료 패널 UI")]
    public Image[] completeRewardIcons; // 3개 고정

    [Header("퀘스트 버튼들")]
    public List<Button> questButtons; // Inspector에서 버튼 10개 연결

    public Quest currentQuest;

    /// <summary>
    /// 버튼에서 호출: questID에 맞는 퀘스트를 불러와 패널 갱신
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
    /// 진행중 퀘스트 처리
    /// 퀘스트 종류별 조건 함수로 구분
    /// </summary>
    private void HandleInProgressQuest(Quest quest)
    {
        bool canComplete = false;

        // 수집형 조건 체크
        if (IsCollectionQuest(quest))
            canComplete = CheckCollectionQuest(quest);

        // 다른 퀘스트 종류 조건 추가 가능
        // else if (IsDialogueQuest(quest))
        //     canComplete = CheckDialogueQuest(quest);

        if (canComplete)
            comQuestPanel.SetActive(true);  // 완료 질문 패널
        else
            inProgressPanel.SetActive(true); // 진행중 패널
    }

    #region 퀘스트 조건 체크 함수

    private bool IsCollectionQuest(Quest quest)
    {
        // 수집형 퀘스트 ID 기반 구분
        int[] collectionIDs = { 1, 2, 3 }; // 토마토, 옥수수, 쌀 등
        return System.Array.Exists(collectionIDs, id => id == quest.questID);
    }

    private bool CheckCollectionQuest(Quest quest)
    {
        int requiredItemID = 0;
        int requiredQuantity = 0;

        switch (quest.questID)
        {
            case 1: // 토마토 3개
                requiredItemID = 3;
                requiredQuantity = 3;
                break;
            case 2: // 옥수수 3개
                requiredItemID = 6;
                requiredQuantity = 3;
                break;
            case 3: // 쌀 3개
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
    /// comQuestPanel 안에서 '완료' 버튼이 눌렸을 때
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
    /// 이전 버튼에서 호출
    /// currentQuest.questID 기준으로 버튼 활성화
    /// </summary>
    public void ShowCurrentQuestButton()
    {
        if (questButtons == null || questButtons.Count == 0) return;

        for (int i = 0; i < questButtons.Count; i++)
        {
            if (currentQuest != null && currentQuest.questID == i + 1) // ID가 1~10 가정
                questButtons[i].gameObject.SetActive(true);
            else
                questButtons[i].gameObject.SetActive(false);
        }
    }
}
