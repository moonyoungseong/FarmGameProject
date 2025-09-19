using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    // 싱글톤 패턴 (원하면 사용)
    public static RewardManager Instance;

    public UISwitch uiSwitch; // 인스펙터에서 연결

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //public void GiveReward(Reward reward)
    //{
    //    CountManager countManager = GameObject.FindObjectOfType<CountManager>();
    //    if (countManager == null)
    //    {
    //        Debug.LogWarning("CountManager를 찾을 수 없습니다!");
    //        return;
    //    }

    //    // 수량 1개 지급 (원하면 reward.quantity 사용 가능)
    //    countManager.ItemNameInput.text = reward.itemname;
    //    countManager.ItemNumberInput.text = "1";
    //    countManager.GetItemClick();

    //    Debug.Log($"보상 지급: 아이템 이름={reward.itemname}, 수량=1");
    //}

    public void GiveReward(Reward reward)
    {
        CountManager countManager = GameObject.FindObjectOfType<CountManager>();
        if (countManager == null)
        {
            Debug.LogWarning("CountManager를 찾을 수 없습니다!");
            return;
        }

        int rewardQty = reward.quantity > 0 ? reward.quantity : 1;
        countManager.GiveRewardItem(reward.itemname, rewardQty);

        Debug.Log($"보상 지급 완료: {reward.itemname}, 수량={rewardQty}");
    }

    public void GiveRewards(System.Collections.Generic.List<Reward> rewards)
    {
        if (rewards == null || rewards.Count == 0)
        {
            Debug.LogWarning("보상이 없습니다.");
            return;
        }

        foreach (var reward in rewards)
        {
            GiveReward(reward);
        }
    }

    public void GiveCurrentQuestRewards()
    {
        if (uiSwitch != null && uiSwitch.currentQuest != null)
        {
            GiveRewards(uiSwitch.currentQuest.reward);
        }
    }
}
