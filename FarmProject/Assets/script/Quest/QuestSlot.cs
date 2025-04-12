using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestSlot : MonoBehaviour, IPointerEnterHandler
{
    public Quest questData;  // �� ������ ���� ����Ʈ ������

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestDetailPanel.Instance.ShowDetails(questData);
    }
}