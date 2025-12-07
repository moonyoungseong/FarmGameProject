using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ToggleUI.cs
///
/// - 특정 UI를 활성화/비활성화하는 간단한 UI 토글 스크립트 ( 농기구 버튼 펼치기 )
/// - 버튼 OnClick 이벤트에 연결하여 사용
/// </summary>
public class ToggleUI : MonoBehaviour
{
    // UI 오브젝트
    public GameObject targetUI;

    // UI의 현재 활성 여부를 저장하는 변수
    private bool isUIActive = false;

    // UI의 활성화 상태를 반전
    public void ToggleUIVisibility()
    {
        isUIActive = !isUIActive;          // 현재 상태 반전
        targetUI.SetActive(isUIActive);    // UI 활성화/비활성화 적용
    }
}
