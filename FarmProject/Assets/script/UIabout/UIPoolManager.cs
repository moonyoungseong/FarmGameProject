using System.Collections.Generic;
using UnityEngine;

public class UIPoolManager : MonoBehaviour
{
    // Inspector���� ���� �Ҵ��� Dictionary
    [System.Serializable]
    public class NPCUI
    {
        public string npcName;     // NPC �̸�
        public GameObject uiPanel; // �ش� NPC�� UI �г�
    }

    public List<NPCUI> npcUIMappings; // NPC�� UI ���� ����Ʈ
    private Dictionary<string, GameObject> uiPool; // ���� ������

    void Awake()
    {
        // List�� Dictionary�� ��ȯ�Ͽ� ���� �˻� �����ϵ���
        uiPool = new Dictionary<string, GameObject>();
        foreach (var mapping in npcUIMappings)
        {
            if (mapping.uiPanel != null && !uiPool.ContainsKey(mapping.npcName))
            {
                uiPool[mapping.npcName] = mapping.uiPanel;
                mapping.uiPanel.SetActive(false); // �ʱ� ����: ��Ȱ��ȭ
            }
        }
    }

    // Ư�� NPC�� UI �г� ��������
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

    // UI ��Ȱ��ȭ
    public void HideUI(string npcName)
    {
        if (uiPool.TryGetValue(npcName, out GameObject uiPanel))
        {
            uiPanel.SetActive(false);
        }
    }
}
