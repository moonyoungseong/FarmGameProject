using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerFollowCamera.cs
/// 플레이어 캐릭터를 따라다니는 카메라를 제어하는 스크립트
/// 
/// - 두 캐릭터(player1, player2) 중 활성화된 캐릭터를 자동으로 추적.
/// - 카메라 위치와 회전을 부드럽게 보간(Lerp, Slerp)하여 자연스러운 추적 구현.
/// </summary>
public class PlayerFollowCamera : MonoBehaviour
{
    // 남자 캐릭터
    public Transform player1;

    // 여자 캐릭터
    public Transform player2;

    // 활성화된 캐릭터의 Transform
    private Transform activePlayer;

    // 카메라와 플레이어 간 거리 오프셋
    public Vector3 offset = new Vector3(0, 5, -10);

    public float followSpeed = 5f;
    public float rotationSpeed = 5f;

    // 카메라 고정 각도 설정
    public Vector3 cameraAngle = new Vector3(30, 0, 0);

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
