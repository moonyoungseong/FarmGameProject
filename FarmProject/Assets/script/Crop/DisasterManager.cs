using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public Crop crop;

    // �ڿ����� �߻� (����)
    public void TriggerDisaster()
    {
        // �ڿ����� Ȯ�� (0~1 ������ ���� ������ ����)
        float disasterChance = Random.Range(0f, 1f);

        if (disasterChance > 0.7f) // ��: 70% Ȯ���� �ڿ����� �߻�
        {
            Debug.Log("�ڿ����ذ� �߻��߽��ϴ�!");
            crop.ApplyDisaster(); // �ڿ����� ó��
        }
    }
}
