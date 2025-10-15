using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcName;  // NPC의 이름
    private UIPoolManager uiPoolManager;

    void Start()
    {
        // UIPoolManager 찾기
        uiPoolManager = FindObjectOfType<UIPoolManager>();
        if (uiPoolManager == null)
        {
            Debug.LogError("UIPoolManager not found in the scene.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            uiPoolManager?.GetUI(npcName); // NPC 이름 기반으로 UI 활성화
        }
    }

    public void DeleteUI()
    {
        uiPoolManager?.HideUI(npcName); // NPC 이름 기반으로 UI 비활성화
    }
}
