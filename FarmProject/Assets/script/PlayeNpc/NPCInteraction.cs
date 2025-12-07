using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPCInteraction.cs
/// 
/// - 플레이어와 NPC 충돌 시 NPC 관련 UI를 활성화/비활성화하는 스크립트.
/// - 충돌 감지(OnCollisionEnter)로 UIPoolManager를 통해 UI를 관리.
/// - DeleteUI() 호출 시 UI 비활성화.
/// - NPC 오브젝트에 붙여서 사용
/// </summary>
public class NPCInteraction : MonoBehaviour
{
    public string npcName;

    // UI 풀 매니저 참조
    private UIPoolManager uiPoolManager;

    void Start()
    {
        // 씬에서 UIPoolManager 객체 찾기
        uiPoolManager = FindObjectOfType<UIPoolManager>();
        if (uiPoolManager == null)
        {
            Debug.LogError("UIPoolManager not found in the scene.");
        }
    }

    // 플레이어가 NPC와 충돌했을 때 호출
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // NPC 이름 기반으로 UI 활성화
            uiPoolManager?.GetUI(npcName);
        }
    }

    // NPC UI를 비활성화
    public void DeleteUI()
    {
        // NPC 이름 기반으로 UI 숨기기
        uiPoolManager?.HideUI(npcName);
    }
}
