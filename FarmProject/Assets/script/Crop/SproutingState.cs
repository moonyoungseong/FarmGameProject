using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutingState : IState
{
    public void HandleWatering(Crop crop)
    {
        // ���� �ָ� �۹��� �ڶ󳪰ų� ���� �� ����
        Debug.Log("�� �ڶ� �۹��� ���� �־����ϴ�. �ڶ�� ���·� ���մϴ�.");
        //crop.SetState(new GrowingState()); // �ڶ� ���·� ����
    }
}
