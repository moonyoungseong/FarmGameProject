using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RewardManager.cs
/// 퀘스트 보상 지급을 관리하는 매니저 클래스
/// 
/// - 싱글톤 패턴 사용
/// - 개별 보상 지급, 여러 보상 지급, 현재 퀘스트 보상 지급 기능 포함
/// </summary>
public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    /// <summary>
    /// 현재 선택된 퀘스트를 관리하는 UI 스위치 객체
    /// (인스펙터에서 지정)
    /// </summary>
    public UISwitch uiSwitch;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // CountManager를 찾아서 아이템 지급
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
    }

    // reward 리스트를 순회하며 GiveReward 호출
    public void GiveRewards(List<Reward> rewards)
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

    // UI에서 선택되어 있는 현재 퀘스트의 보상 지급
    public void GiveCurrentQuestRewards()
    {
        if (uiSwitch != null && uiSwitch.currentQuest != null)
        {
            GiveRewards(uiSwitch.currentQuest.reward);
        }
    }
}
