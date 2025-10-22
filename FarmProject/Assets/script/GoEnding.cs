using UnityEngine;

public class GoEnding : MonoBehaviour
{
    [Header("���� �г�")]
    public GameObject endingPanel; // �ν����Ϳ��� ���� �г� ����

    [Header("���� ����")]
    public int goldThreshold = 100; // ���� ���� ���

    private void Start()
    {
        if (endingPanel != null)
            endingPanel.SetActive(false); // �ʱ⿡�� �г� �����

        // GoldManager ����
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += CheckGold;
            // ���� ���ε� �ٷ� üũ
            CheckGold(GoldManager.Instance.GetGold());
        }
        else
        {
            Debug.LogWarning("[GoEnding] GoldManager �ν��Ͻ��� �������� �ʽ��ϴ�.");
        }
    }

    private void CheckGold(int currentGold)
    {
        if (currentGold >= goldThreshold && endingPanel != null)
        {
            endingPanel.SetActive(true); // ���� �г� Ȱ��ȭ
        }
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ����
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= CheckGold;
    }
}
