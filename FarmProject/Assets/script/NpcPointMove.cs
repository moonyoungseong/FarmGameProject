using UnityEngine;

public class NpcPointMove : MonoBehaviour
{
    public Transform[] points; // 이동할 포인트들의 배열
    public float speed = 3f;   // 이동 속도
    public float stopTime = 1f; // 포인트에서 멈추는 시간

    private int currentPointIndex = 0; // 현재 이동 중인 포인트의 인덱스
    private bool isWaiting = false;    // 대기 상태인지 확인

    void Update()
    {
        if (points.Length == 0 || isWaiting) return;

        // 현재 포인트까지의 거리 계산
        float distance = Vector3.Distance(transform.position, points[currentPointIndex].position);

        if (distance < 0.1f) // 포인트에 도착했을 때
        {
            StartCoroutine(WaitAtPoint());
        }
        else
        {
            // 포인트 방향으로 이동
            Vector3 direction = (points[currentPointIndex].position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // NPC를 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    private System.Collections.IEnumerator WaitAtPoint()
    {
        isWaiting = true;

        // 멈추는 시간만큼 대기
        yield return new WaitForSeconds(stopTime);

        // 다음 포인트로 이동
        currentPointIndex = (currentPointIndex + 1) % points.Length;

        isWaiting = false;
    }
}
