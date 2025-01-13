using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutingState : IState
{
    public void HandleWatering(Crop crop)
    {
        // ���� �ָ� �۹��� �ڶ󳪰ų� ���� �� ����
        Debug.Log("�� �ڶ� �۹��� ���� �־����ϴ�. �ڶ�� ���·� ���մϴ�.");
        crop.SetState(new FullyGrownState()); // �� �ڶ� ���·� ����
    }

    public void HandleDisaster(Crop crop)
    {
        // �ڿ����� �߻� �� ���� ��ȭ ó�� (��: ������ �������� ����)
        Debug.Log("�ڿ����ط� ���� �� �ڶ� �۹��� �׽��ϴ�.");
        crop.SetState(new DeadState()); // ���� ���·� ����
    }
}
