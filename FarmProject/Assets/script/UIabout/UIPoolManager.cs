using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIPoolManager.cs
/// NPC 이름을 기반으로 해당 NPC의 UI 패널을 관리하는 매니저
/// 
/// - Inspector에서 UI 매핑 리스트를 받아 Dictionary로 변환하여 빠른 UI 접근을 제공
/// - 특정 NPC의 UI를 활성화하거나 비활성화하는 기능을 담당
/// </summary>
public class UIPoolManager : MonoBehaviour
{
    // Inspector에서 설정 가능한 NPC - UI 매핑 구조체
    [System.Serializable]
    public class NPCUI
    {
        public string npcName;
        public GameObject uiPanel;
    }

    // Inspector에서 입력된 NPC-UI 매핑 리스트
    public List<NPCUI> npcUIMappings;

    // 내부적으로 사용할 Dictionary (npcName → uiPanel)
    private Dictionary<string, GameObject> uiPool;

    // 게임 시작 시 List의 데이터를 Dictionary로 변환
    void Awake()
    {
        uiPool = new Dictionary<string, GameObject>();
        foreach (var mapping in npcUIMappings)
        {
            if (mapping.uiPanel != null && !uiPool.ContainsKey(mapping.npcName))
            {
                uiPool[mapping.npcName] = mapping.uiPanel;
                mapping.uiPanel.SetActive(false); // 초기에는 UI를 숨김
            }
        }
    }

    // NPC 이름을 기반으로 해당하는 UI 패널을 찾아 활성화한 뒤 반환
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

    // NPC 이름을 기반으로 해당하는 UI 패널을 비활성
    public void HideUI(string npcName)
    {
        if (uiPool.TryGetValue(npcName, out GameObject uiPanel))
        {
            uiPanel.SetActive(false);
        }
    }
}
