using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    public Transform player;             // 플레이어의 Transform
    public Vector3 offset = new Vector3(0, 5, -10); // 카메라와 플레이어 간 거리
    public float followSpeed = 5f;       // 따라가는 속도
    public float rotationSpeed = 5f;     // 회전 속도

    public Vector3 cameraAngle = new Vector3(30, 0, 0); // 카메라 각도 설정

    void LateUpdate()
    {
        if (player == null) return;

        // 목표 위치 계산 (플레이어 위치 + 오프셋)
        Vector3 targetPosition = player.position + player.rotation * offset;

        // 부드럽게 목표 위치로 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // 카메라 각도와 플레이어 방향을 적용
        Quaternion desiredRotation = Quaternion.Euler(cameraAngle.x, player.eulerAngles.y, cameraAngle.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
