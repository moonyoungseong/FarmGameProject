using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutingState : IState
{
    public void HandleWatering(Crop crop)
    {
        // 물을 주면 작물이 자라나거나 죽을 수 있음
        Debug.Log("덜 자란 작물에 물을 주었습니다. 자라는 상태로 변합니다.");
        crop.SetState(new FullyGrownState()); // 다 자란 상태로 변경
    }

    public void HandleDisaster(Crop crop)
    {
        // 자연재해 발생 시 상태 변화 처리 (예: 강수량 부족으로 죽음)
        Debug.Log("자연재해로 인해 덜 자란 작물이 죽습니다.");
        crop.SetState(new DeadState()); // 죽은 상태로 변경
    }
}
