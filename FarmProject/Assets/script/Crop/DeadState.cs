using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState
{
    public void HandleWatering(Crop crop)
    {
        // �̹� ���� ���¿��� ���� �ָ� �ƹ� �ϵ� �Ͼ�� ����
        Debug.Log("�̹� ���� �۹��� ���� �־����� ��ȭ�� �����ϴ�.");
    }
}