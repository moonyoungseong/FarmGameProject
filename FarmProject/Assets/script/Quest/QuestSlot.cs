using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestSlot : MonoBehaviour, IPointerEnterHandler
{
    public Quest questData;  // �� ������ ���� ����Ʈ ������
    public GameObject lockImage;  // Inspector���� �ڹ��� ������Ʈ ����

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestDetailPanel.Instance.ShowDetails(questData);
    }

    public void Unlock()    // �ڹ��� Ǫ�� �Լ�
    {
        if (lockImage != null)
            lockImage.SetActive(false);
    }

    // ���߿� ����Ʈ ������ ��� �����Ҷ� ���� 
    // �ڹ��� ����
    // FindObjectOfType<QuestListController>().UnlockQuestSlot(quest.questName);
}