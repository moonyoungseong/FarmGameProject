using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private IState currentState;

    private void Start()
    {
        // �ʱ� ���´� ���� ���·� ����
        currentState = new SeedState();
    }

    // ���¸� �����ϴ� �޼���
    public void SetState(IState newState)
    {
        currentState = newState;
    }

    // ���� �ִ� �޼���
    public void WaterCrop()
    {
        currentState.HandleWatering(this); // ���� ���¿��� ���� �ִ� �ൿ�� ó��
    }
}
