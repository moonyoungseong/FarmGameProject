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

    public void HandleDisaster(Crop crop)
    {
        // 씨앗 상태에서 자연재해 발생 시 영향 없음 (또는 초기 상태에서 죽음 처리 가능)
        Debug.Log("자연재해로 인한 영향은 없습니다.");
    }
}
