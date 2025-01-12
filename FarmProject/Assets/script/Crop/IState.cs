using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void HandleWatering(Crop crop); // 물 주기 처리
}
