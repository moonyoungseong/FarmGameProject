using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    // �̱��� ���� (���ϸ� ���)
    public static RewardManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GiveReward(Reward reward)
    {
        CountManager countManager = GameObject.FindObjectOfType<CountManager>();
        if (countManager == null)
        {
            Debug.LogWarning("CountManager�� ã�� �� �����ϴ�!");
            return;
        }

        // ���� 1�� ���� (���ϸ� reward.quantity ��� ����)
        countManager.ItemNameInput.text = reward.itemname;
        countManager.ItemNumberInput.text = "1";
        countManager.GetItemClick();

        Debug.Log($"���� ����: ������ �̸�={reward.itemname}, ����=1");
    }

    public void GiveRewards(System.Collections.Generic.List<Reward> rewards)
    {
        if (rewards == null || rewards.Count == 0)
        {
            Debug.LogWarning("������ �����ϴ�.");
            return;
        }

        foreach (var reward in rewards)
        {
            GiveReward(reward);
        }
    }
}
