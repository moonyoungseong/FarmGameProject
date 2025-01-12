using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private IState currentState;

    private void Start()
    {
        // 초기 상태는 씨앗 상태로 설정
        currentState = new SeedState();
    }

    // 상태를 변경하는 메서드
    public void SetState(IState newState)
    {
        currentState = newState;
    }

    // 물을 주는 메서드
    public void WaterCrop()
    {
        currentState.HandleWatering(this); // 현재 상태에서 물을 주는 행동을 처리
    }
}
