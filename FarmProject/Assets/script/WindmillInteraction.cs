using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillInteraction : MonoBehaviour
{
    public GameObject player1;               // �÷��̾� 1
    public GameObject player2;               // �÷��̾� 2
    public GameObject ladder1;

    private Transform player;                // ���� Ȱ��ȭ�� �÷��̾��� Transform�� ����

    void Start()
    {
        // �� �ε� �� Ȱ��ȭ�� �÷��̾ ���� player �Ҵ�
        if (player1.activeInHierarchy)
        {
            player = player1.transform;  // �÷��̾� 1�� Transform ����
        }
        else if (player2.activeInHierarchy)
        {
            player = player2.transform;  // �÷��̾� 2�� Transform ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���ſ� ������ ������Ʈ�� Ư�� �±׸� ���� ���
        if (other.CompareTag("WindZone"))
        {
            Debug.Log("�÷��̾ Ư�� Trigger Zone�� ���Խ��ϴ�!");
            // �ʿ��� ���� �߰�
            ladder1.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ʈ���ſ��� ���� ������Ʈ�� Ư�� �±׸� ���� ���
        if (other.CompareTag("WindZone"))
        {
            Debug.Log("�÷��̾ Ư�� Trigger Zone���� �������ϴ�!");
            // �ʿ��� ���� �߰�
            ladder1.SetActive(false);
        }
    }

    public void GotoMove()
    {
        // ���� Ȱ��ȭ�� �÷��̾ �ִٸ� �̵� �� ȸ�� ����
        if (player != null)
        {
            Vector3 newPosition = new Vector3(10.56f, 2.71f, 34.41f); // �̵��� ��ġ
            Vector3 newRotation = new Vector3(0f, 169.016f, 0f); // ȸ���� (Y�� 90��)
            MoveInit(player, newPosition, newRotation);

            // 2�� �Ŀ� �ٸ� ��ġ�� �̵�
            StartCoroutine(DelayMoveInit(player, 2f));
        }
    }

    // MoveInit �Լ�: ��ġ�� ȸ������ ����
    public void MoveInit(Transform target, Vector3 targetPosition, Vector3 targetRotation)
    {
        if (target != null)
        {
            // ��ġ ����
            target.position = targetPosition;

            // ȸ���� ���� (Vector3�� Quaternion���� ��ȯ)
            target.rotation = Quaternion.Euler(targetRotation);

            Debug.Log($"�÷��̾ {targetPosition} ��ġ�� �̵��ϰ�, ȸ������ {targetRotation}�� �����߽��ϴ�.");
        }
        else
        {
            Debug.LogWarning("�÷��̾ �������� �ʾҽ��ϴ�.");
        }
    }

    // 2�� �Ŀ� ���ο� ��ġ�� MoveInit�� ȣ���ϴ� �ڷ�ƾ
    private IEnumerator DelayMoveInit(Transform target, float delay)
    {
        // ������ �ð�(��) ��ŭ ���
        yield return new WaitForSeconds(delay);

        // ���ο� ��ġ�� ȸ���� ����
        Vector3 newPosition = new Vector3(20f, 100f, 50f); // �� ��° �̵��� ��ġ
        Vector3 newRotation = new Vector3(0f, 180f, 0f); // �� ��° ȸ���� (Y�� 90��)

        // ��� �� MoveInit ȣ��
        MoveInit(target, newPosition, newRotation);
    }
}
