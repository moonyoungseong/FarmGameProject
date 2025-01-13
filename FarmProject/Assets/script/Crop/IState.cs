using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void HandleWatering(Crop crop); // 물 주기 처리
    void HandleDisaster(Crop crop); // 자연재해 처리
}
