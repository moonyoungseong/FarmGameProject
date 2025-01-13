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

    private void Update()
    {
        // ���⿡ �ð��� ���� ���� ������ �̹� �����Ǿ� �ִٰ� �����ϰ�, ���¿� ���� �� �ֱ⸸ ó��
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

    // �ڿ����ذ� �߻��ϴ� �޼���
    public void ApplyDisaster()
    {
        currentState.HandleDisaster(this); // ���� ���¿��� �ڿ����ظ� ó��
    }
}
