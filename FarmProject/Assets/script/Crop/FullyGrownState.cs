using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullyGrownState : IState
{
    public void HandleWatering(Crop crop)
    {
        // ���� �ָ� �۹��� ���� �� ����
        Debug.Log("������ �ڶ� �۹��� ���� �־����ϴ�. �۹��� �׽��ϴ�.");
        crop.SetState(new DeadState()); // ���� ���·� ����
    }

    public void HandleDisaster(Crop crop)
    {
        // �ڿ����� �߻� �� ���� ��ȭ ó�� (��: ��ǳ���� �۹� �ı�)
        Debug.Log("�ڿ����ط� ���� ������ �ڶ� �۹��� �׽��ϴ�.");
        crop.SetState(new DeadState()); // ���� ���·� ����
    }
}
