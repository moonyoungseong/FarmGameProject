using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestSlot : MonoBehaviour, IPointerEnterHandler
{
    public Quest questData;  // 이 슬롯이 가진 퀘스트 데이터
    public GameObject lockImage;  // Inspector에서 자물쇠 오브젝트 연결

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestDetailPanel.Instance.ShowDetails(questData);
    }

    public void Unlock()    // 자물쇠 푸는 함수
    {
        if (lockImage != null)
            lockImage.SetActive(false);
    }

    // 나중에 퀘스트 수락시 잠금 해재할때 쓸것 
    // 자물쇠 해제
    // FindObjectOfType<QuestListController>().UnlockQuestSlot(quest.questName);
}