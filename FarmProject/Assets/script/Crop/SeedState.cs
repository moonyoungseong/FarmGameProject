using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedState : IState
{
    public void HandleWatering(Crop crop)
    {
        // ���� �ָ� �۹��� �� �ڶ� ���·� ��ȭ
        Debug.Log("���ѿ� ���� �־����ϴ�. �۹��� �� �ڶ� ���·� ���մϴ�.");
        crop.SetState(new SproutingState()); // �� �ڶ� ���·� ����
    }
}
