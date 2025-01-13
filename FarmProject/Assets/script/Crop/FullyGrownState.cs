using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullyGrownState : IState
{
    public void HandleWatering(Crop crop)
    {
        // 물을 주면 작물이 죽을 수 있음
        Debug.Log("완전히 자란 작물에 물을 주었습니다. 작물이 죽습니다.");
        crop.SetState(new DeadState()); // 죽은 상태로 변경
    }

    public void HandleDisaster(Crop crop)
    {
        // 자연재해 발생 시 상태 변화 처리 (예: 폭풍으로 작물 파괴)
        Debug.Log("자연재해로 인해 완전히 자란 작물이 죽습니다.");
        crop.SetState(new DeadState()); // 죽은 상태로 변경
    }
}
