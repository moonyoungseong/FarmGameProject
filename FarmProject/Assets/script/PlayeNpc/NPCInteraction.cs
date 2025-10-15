using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcName;  // NPC�� �̸�
    private UIPoolManager uiPoolManager;

    void Start()
    {
        // UIPoolManager ã��
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
            uiPoolManager?.GetUI(npcName); // NPC �̸� ������� UI Ȱ��ȭ
        }
    }

    public void DeleteUI()
    {
        uiPoolManager?.HideUI(npcName); // NPC �̸� ������� UI ��Ȱ��ȭ
    }
}
