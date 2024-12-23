using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    public Transform player;             // �÷��̾��� Transform
    public Vector3 offset = new Vector3(0, 5, -10); // ī�޶�� �÷��̾� �� �Ÿ�
    public float followSpeed = 5f;       // ���󰡴� �ӵ�
    public float rotationSpeed = 5f;     // ȸ�� �ӵ�

    public Vector3 cameraAngle = new Vector3(30, 0, 0); // ī�޶� ���� ����

    void LateUpdate()
    {
        if (player == null) return;

        // ��ǥ ��ġ ��� (�÷��̾� ��ġ + ������)
        Vector3 targetPosition = player.position + player.rotation * offset;

        // �ε巴�� ��ǥ ��ġ�� �̵�
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // ī�޶� ������ �÷��̾� ������ ����
        Quaternion desiredRotation = Quaternion.Euler(cameraAngle.x, player.eulerAngles.y, cameraAngle.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
