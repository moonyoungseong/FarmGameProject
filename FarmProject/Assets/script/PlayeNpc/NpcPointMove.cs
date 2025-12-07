using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NpcPointMove.cs
/// 
/// - NPC를 지정된 포인트 배열을 따라 순환 이동시키는 스크립트.
/// - 포인트 도착 시 일정 시간 대기(stopTime).
/// - 플레이어와 충돌 시 잠시 멈춤(collisionStopTime).
/// - 이동 방향으로 NPC 회전.
/// </summary>
public class NpcPointMove : MonoBehaviour
{
    // 이동할 포인트들의 배열
    public Transform[] points;

    public float speed = 3f;

    // 포인트에서 멈추는 시간
    public float stopTime = 1f;

    // 충돌 시 멈추는 시간
    public float collisionStopTime = 2f;

    // 현재 이동 중인 포인트의 인덱스
    private int currentPointIndex = 0;

    private bool isWaiting = false;

    private bool isColliding = false;

    void Update()
    {
        // 포인트가 없거나 대기/충돌 상태면 이동하지 않음
        if (points.Length == 0 || isWaiting || isColliding) return;

        // 현재 포인트까지 거리 계산
        float distance = Vector3.Distance(transform.position, points[currentPointIndex].position);

        if (distance < 0.1f) // 포인트 도착
        {
            StartCoroutine(WaitAtPoint());
        }
        else
        {
            // 포인트 방향으로 이동
            Vector3 direction = (points[currentPointIndex].position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    // 포인트 도착 시 일정 시간 대기 후 다음 포인트로 이동
    private System.Collections.IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(stopTime);
        currentPointIndex = (currentPointIndex + 1) % points.Length; // 순환
        isWaiting = false;
    }

    // 플레이어 충돌 시 멈춤 처리
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StopForCollision());
        }
    }

    // 충돌로 인해 지정 시간 동안 멈춤
    private System.Collections.IEnumerator StopForCollision()
    {
        isColliding = true;
        yield return new WaitForSeconds(collisionStopTime);
        isColliding = false;
    }
}
