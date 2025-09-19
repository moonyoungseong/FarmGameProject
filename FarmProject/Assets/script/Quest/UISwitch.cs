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
    public GameObject comQuestPanel; // 수집형 완료 전 확인 패널

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
    /// 진행 중 퀘스트 처리
    /// </summary>
    private void HandleInProgressQuest(Quest quest)
    {
        if (IsCollectionQuest(quest))
        {
            int itemID = 0;
            int requiredQuantity = 3; // 수집형 퀘스트 고정 수량

            switch (quest.questID)
            {
                case 1: itemID = 3; break; // 토마토
                case 2: itemID = 6; break; // 옥수수
                case 3: itemID = 9; break; // 쌀
            }

            Item item = InventoryManager.Instance.GetItemByID(itemID.ToString());

            if (item != null && item.quantityInt >= requiredQuantity)
            {
                // 충분하면 완료 확인 패널 표시
                comQuestPanel.SetActive(true);
            }
            else
            {
                // 부족하면 진행중 패널 표시
                inProgressPanel.SetActive(true);
            }
        }
        else
        {
            // 수집형이 아니면 그냥 진행중 패널
            inProgressPanel.SetActive(true);
        }
    }

    private bool IsCollectionQuest(Quest quest)
    {
        int[] collectionIDs = { 1, 2, 3 }; // 토마토, 옥수수, 쌀
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
    /// comQuestPanel 안에서 '완료' 버튼이 눌렸을 때
    /// </summary>
    public void ConfirmComplete()
    {
        if (IsCollectionQuest(currentQuest))
        {
            int itemID = 0;
            int requiredQuantity = 3; // 필요한 수량
            switch (currentQuest.questID)
            {
                case 1: itemID = 3; break; // 토마토
                case 2: itemID = 6; break; // 옥수수
                case 3: itemID = 9; break; // 쌀
            }

            Item item = InventoryManager.Instance.GetItemByID(itemID.ToString());

            if (item != null && item.quantityInt >= requiredQuantity)
            {
                // 충분하면 소모 후 완료 처리
                CountManager.Instance.RemoveItemByID(itemID.ToString(), requiredQuantity);
                Debug.Log($"퀘스트 완료: 아이템 {item.itemName} {requiredQuantity}개 소비됨");

                // 퀘스트 상태 완료
                currentQuest.state = QuestState.Completed;

                HideAllPanels();
                completePanel.SetActive(true);
                UpdateCompletePanel();

                // 보상 지급
                RewardManager.Instance.GiveRewards(currentQuest.reward);
            }
            else
            {
                // 부족하면 진행중 패널로 돌아가기
                Debug.Log("아이템 부족. 진행중 패널 표시.");
                HideAllPanels();
                inProgressPanel.SetActive(true);
            }

            return; // 수집형 퀘스트 처리 완료
        }

        // 수집형이 아닌 경우 일반 완료 처리
        currentQuest.state = QuestState.Completed;
        HideAllPanels();
        completePanel.SetActive(true);
        UpdateCompletePanel();
        RewardManager.Instance.GiveRewards(currentQuest.reward);
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
