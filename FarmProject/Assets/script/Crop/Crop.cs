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

    private void Update()
    {
        // 여기에 시간에 따른 성장 로직은 이미 구현되어 있다고 가정하고, 상태에 따른 물 주기만 처리
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

    // 자연재해가 발생하는 메서드
    public void ApplyDisaster()
    {
        currentState.HandleDisaster(this); // 현재 상태에서 자연재해를 처리
    }
}
