using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    public Transform player1;       // 첫 번째 캐릭터의 Transform
    public Transform player2;       // 두 번째 캐릭터의 Transform
    private Transform activePlayer; // 활성화된 캐릭터의 Transform
    public Vector3 offset = new Vector3(0, 5, -10); // 카메라와 플레이어 간 거리
    public float followSpeed = 5f;       // 따라가는 속도
    public float rotationSpeed = 5f;     // 회전 속도

    public Vector3 cameraAngle = new Vector3(30, 0, 0); // 카메라 각도 설정

    void Update()
    {
        // 활성화된 캐릭터 탐지
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
            activePlayer = null; // 둘 다 비활성화된 경우
        }
    }

    void LateUpdate()
    {
        if (activePlayer == null) return;

        // 목표 위치 계산 (플레이어 위치 + 오프셋)
        Vector3 targetPosition = activePlayer.position + activePlayer.rotation * offset;

        // 부드럽게 목표 위치로 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // 카메라 각도와 플레이어 방향을 적용
        Quaternion desiredRotation = Quaternion.Euler(cameraAngle.x, activePlayer.eulerAngles.y, cameraAngle.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
