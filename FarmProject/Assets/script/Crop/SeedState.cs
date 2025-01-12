using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedState : IState
{
    public void HandleWatering(Crop crop)
    {
        // 물을 주면 작물이 덜 자란 상태로 변화
        Debug.Log("씨앗에 물을 주었습니다. 작물이 덜 자란 상태로 변합니다.");
        crop.SetState(new SproutingState()); // 덜 자란 상태로 변경
    }
}
