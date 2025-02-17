using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject targetUI;  // 열고 닫을 UI 오브젝트
    private bool isUIActive = false;  // UI가 활성화 되어 있는지 여부

    // 버튼 클릭 시 호출될 함수
    public void ToggleUIVisibility()
    {
        isUIActive = !isUIActive;  // UI 활성화 상태를 반전
        targetUI.SetActive(isUIActive);  // UI 활성화/비활성화
    }
}
