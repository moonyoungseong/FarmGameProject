using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestSlot : MonoBehaviour, IPointerEnterHandler
{
    public Quest questData;  // 이 슬롯이 가진 퀘스트 데이터

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestDetailPanel.Instance.ShowDetails(questData);
    }
}