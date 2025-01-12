using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutingState : IState
{
    public void HandleWatering(Crop crop)
    {
        // 물을 주면 작물이 자라나거나 죽을 수 있음
        Debug.Log("덜 자란 작물에 물을 주었습니다. 자라는 상태로 변합니다.");
        //crop.SetState(new GrowingState()); // 자란 상태로 변경
    }
}
