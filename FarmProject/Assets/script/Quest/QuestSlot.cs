using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// QuestSlot.cs
/// 퀘스트 목록 UI에서 각 퀘스트 슬롯을 나타내는 클래스
/// 
/// - 마우스를 올리면 퀘스트 상세창에 상세 정보를 보여줌
/// - 자물쇠 아이콘을 켜고/끄는 기능 포함
/// </summary>
public class QuestSlot : MonoBehaviour, IPointerEnterHandler
{
    public Quest questData;

    // 자물쇠 오브젝트
    public GameObject lockImage;

    /// <summary>
    /// 마우스를 슬롯 위에 올렸을 때 호출되는 이벤트
    /// 퀘스트 상세 패널에 정보를 표시
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestDetailPanel.Instance.ShowDetails(questData);
    }


    // 자물쇠 이미지를 끄는 함수
    public void Unlock()
    {
        if (lockImage != null)
            lockImage.SetActive(false);
    }
}
