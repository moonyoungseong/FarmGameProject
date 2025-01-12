using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState
{
    public void HandleWatering(Crop crop)
    {
        // 이미 죽은 상태에서 물을 주면 아무 일도 일어나지 않음
        Debug.Log("이미 죽은 작물에 물을 주었지만 변화가 없습니다.");
    }
}
