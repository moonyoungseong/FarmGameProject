using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    public Transform player1;       // ù ��° ĳ������ Transform
    public Transform player2;       // �� ��° ĳ������ Transform
    private Transform activePlayer; // Ȱ��ȭ�� ĳ������ Transform
    public Vector3 offset = new Vector3(0, 5, -10); // ī�޶�� �÷��̾� �� �Ÿ�
    public float followSpeed = 5f;       // ���󰡴� �ӵ�
    public float rotationSpeed = 5f;     // ȸ�� �ӵ�

    public Vector3 cameraAngle = new Vector3(30, 0, 0); // ī�޶� ���� ����

    void Update()
    {
        // Ȱ��ȭ�� ĳ���� Ž��
        if (player1.gameObject.activeSelf)
        {
            activePlayer = player1;
        }
        else if (player2.gameObject.activeSelf)
        {
            activePlayer = player2;
        }
        else
        {
            activePlayer = null; // �� �� ��Ȱ��ȭ�� ���
        }
    }

    void LateUpdate()
    {
        if (activePlayer == null) return;

        // ��ǥ ��ġ ��� (�÷��̾� ��ġ + ������)
        Vector3 targetPosition = activePlayer.position + activePlayer.rotation * offset;

        // �ε巴�� ��ǥ ��ġ�� �̵�
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // ī�޶� ������ �÷��̾� ������ ����
        Quaternion desiredRotation = Quaternion.Euler(cameraAngle.x, activePlayer.eulerAngles.y, cameraAngle.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
