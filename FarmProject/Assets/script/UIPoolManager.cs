using System.Collections.Generic;
using UnityEngine;

public class UIPoolManager : MonoBehaviour
{
    // Inspector에서 직접 할당할 Dictionary
    [System.Serializable]
    public class NPCUI
    {
        public string npcName;     // NPC 이름
        public GameObject uiPanel; // 해당 NPC의 UI 패널
    }

    public List<NPCUI> npcUIMappings; // NPC와 UI 매핑 리스트
    private Dictionary<string, GameObject> uiPool; // 내부 관리용

    void Awake()
    {
        // List를 Dictionary로 변환하여 빠른 검색 가능하도록
        uiPool = new Dictionary<string, GameObject>();
        foreach (var mapping in npcUIMappings)
        {
            if (mapping.uiPanel != null && !uiPool.ContainsKey(mapping.npcName))
            {
                uiPool[mapping.npcName] = mapping.uiPanel;
                mapping.uiPanel.SetActive(false); // 초기 상태: 비활성화
            }
        }
    }

    // 특정 NPC의 UI 패널 가져오기
    public GameObject GetUI(string npcName)
    {
        if (uiPool.TryGetValue(npcName, out GameObject uiPanel))
        {
            uiPanel.SetActive(true);
            return uiPanel;
        }

        Debug.LogWarning($"UI for NPC '{npcName}' not found.");
        return null;
    }

    // UI 비활성화
    public void HideUI(string npcName)
    {
        if (uiPool.TryGetValue(npcName, out GameObject uiPanel))
        {
            uiPanel.SetActive(false);
        }
    }
}
