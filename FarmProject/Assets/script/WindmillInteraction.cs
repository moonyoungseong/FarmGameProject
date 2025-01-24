using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillInteraction : MonoBehaviour
{
    public GameObject player1;               // 플레이어 1
    public GameObject player2;               // 플레이어 2
    public GameObject ladder1;

    private Transform player;                // 현재 활성화된 플레이어의 Transform을 저장

    void Start()
    {
        // 씬 로딩 시 활성화된 플레이어에 따라 player 할당
        if (player1.activeInHierarchy)
        {
            player = player1.transform;  // 플레이어 1의 Transform 설정
        }
        else if (player2.activeInHierarchy)
        {
            player = player2.transform;  // 플레이어 2의 Transform 설정
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 진입한 오브젝트가 특정 태그를 가진 경우
        if (other.CompareTag("WindZone"))
        {
            Debug.Log("플레이어가 특정 Trigger Zone에 들어왔습니다!");
            // 필요한 동작 추가
            ladder1.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 트리거에서 나간 오브젝트가 특정 태그를 가진 경우
        if (other.CompareTag("WindZone"))
        {
            Debug.Log("플레이어가 특정 Trigger Zone에서 나갔습니다!");
            // 필요한 동작 추가
            ladder1.SetActive(false);
        }
    }

    public void GotoMove()
    {
        // 현재 활성화된 플레이어가 있다면 이동 및 회전 수행
        if (player != null)
        {
            Vector3 newPosition = new Vector3(10.56f, 2.71f, 34.41f); // 이동할 위치
            Vector3 newRotation = new Vector3(0f, 169.016f, 0f); // 회전값 (Y축 90도)
            MoveInit(player, newPosition, newRotation);
        }
    }

    // MoveInit 함수: 위치와 회전값을 설정
    public void MoveInit(Transform target, Vector3 targetPosition, Vector3 targetRotation)
    {
        if (target != null)
        {
            // 위치 설정
            target.position = targetPosition;

            // 회전값 설정 (Vector3를 Quaternion으로 변환)
            target.rotation = Quaternion.Euler(targetRotation);

            Debug.Log($"플레이어가 {targetPosition} 위치로 이동하고, 회전값을 {targetRotation}로 설정했습니다.");
        }
        else
        {
            Debug.LogWarning("플레이어가 설정되지 않았습니다.");
        }
    }
}
