using UnityEngine;

public class NpcPointMove : MonoBehaviour
{
    public Transform[] points; // �̵��� ����Ʈ���� �迭
    public float speed = 3f;   // �̵� �ӵ�
    public float stopTime = 1f; // ����Ʈ���� ���ߴ� �ð�

    private int currentPointIndex = 0; // ���� �̵� ���� ����Ʈ�� �ε���
    private bool isWaiting = false;    // ��� �������� Ȯ��

    void Update()
    {
        if (points.Length == 0 || isWaiting) return;

        // ���� ����Ʈ������ �Ÿ� ���
        float distance = Vector3.Distance(transform.position, points[currentPointIndex].position);

        if (distance < 0.1f) // ����Ʈ�� �������� ��
        {
            StartCoroutine(WaitAtPoint());
        }
        else
        {
            // ����Ʈ �������� �̵�
            Vector3 direction = (points[currentPointIndex].position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // NPC�� �̵� �������� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    private System.Collections.IEnumerator WaitAtPoint()
    {
        isWaiting = true;

        // ���ߴ� �ð���ŭ ���
        yield return new WaitForSeconds(stopTime);

        // ���� ����Ʈ�� �̵�
        currentPointIndex = (currentPointIndex + 1) % points.Length;

        isWaiting = false;
    }
}
