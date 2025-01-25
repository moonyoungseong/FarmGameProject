using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillInteraction : MonoBehaviour
{
    public GameObject player1;               // 플레이어 1
    public GameObject player2;               // 플레이어 2
    public GameObject ladder1;               // 사다리밑 UI
    public GameObject ladder2;               // 사다리위 UI
    public GameObject FinishImage;           // 완료 UI
    public GameObject[] LadderAbout; // 사다리 타고 나서 지워할것들
    public GameObject windmillFan; // 풍차 팬 오브젝트

    private bool isRepaired = false; // 수리 상태를 저장

    public Animator playerAnimator; // Animator를 할당해야 함

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

        // Animator를 플레이어의 컴포넌트로 설정
        playerAnimator = player.GetComponent<Animator>();
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

        // 트리거에 진입한 오브젝트가 특정 태그를 가진 경우
        if (other.CompareTag("WindZone2"))
        {
            Debug.Log("플레이어가 특정 Trigger Zone에 들어왔습니다!");
            // 필요한 동작 추가
            ladder2.SetActive(true);
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

        // 트리거에서 나간 오브젝트가 특정 태그를 가진 경우
        if (other.CompareTag("WindZone2"))
        {
            Debug.Log("플레이어가 특정 Trigger Zone에서 나갔습니다!");
            // 필요한 동작 추가
            ladder2.SetActive(false);
            LadderAbout[0].SetActive(false);
            LadderAbout[1].SetActive(false);

            // 팬 회전을 시작하기 위해 코루틴 호출
            StartCoroutine(StartFanRotation(3f)); // 3초 후에 팬 회전 시작
        }
    }

    public void GotoMove()
    {
        // 현재 활성화된 플레이어가 있다면 이동 및 회전 수행
        if (player != null)
        {
            // 첫 번째 위치로 즉시 이동
            Vector3 instantPosition = new Vector3(10.56f, 2.71f, 34.41f);
            Vector3 instantRotation = new Vector3(0f, 169.016f, 0f);
            MoveInit(player, instantPosition, instantRotation);

            // 두 번째 위치로 천천히 이동
            Vector3 smoothPosition = new Vector3(11.15f, 9.4f, 31.7f);
            Vector3 smoothRotation = new Vector3(0f, 180f, 0f);

            // 두 번째 위치로 천천히 이동 (애니메이션 포함)
            StartCoroutine(SmoothMoveWithAnimation(player, smoothPosition, 3f)); // 3초 동안 이동
        }
    }

    public void FIxWind()
    {
        if (playerAnimator != null)
        {
            StartCoroutine(PlayFixWindAnimation());
            
        }
    }

    private IEnumerator PlayFixWindAnimation()
    {
        // 애니메이션 시작
        playerAnimator.SetBool("isFixing", true);

        // 5초 동안 대기
        yield return new WaitForSeconds(5f);

        // 애니메이션 정지
        playerAnimator.SetBool("isFixing", false);

        FinishImage.SetActive(true);        // 완료 안내문
    }

    // MoveInit 함수: 즉시 이동과 회전 수행
    public void MoveInit(Transform target, Vector3 targetPosition, Vector3 targetRotation)
    {
        if (target != null)
        {
            // 즉시 위치 설정
            target.position = targetPosition;

            // 즉시 회전 설정 (Vector3를 Quaternion으로 변환)
            target.rotation = Quaternion.Euler(targetRotation);

            Debug.Log($"플레이어가 {targetPosition} 위치로 즉시 이동했습니다.");
        }
        else
        {
            Debug.LogWarning("플레이어가 설정되지 않았습니다.");
        }
    }

    private IEnumerator SmoothMoveWithAnimation(Transform target, Vector3 targetPosition, float duration)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isClimbing", true); // 이동 애니메이션 시작
            playerAnimator.speed = 1f / duration;      // 애니메이션 속도 조절
        }

        Vector3 startPosition = target.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            target.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        target.position = targetPosition;

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isClimbing", false); // 이동 애니메이션 정지
            playerAnimator.speed = 1f;                  // 애니메이션 속도 초기화
        }
    }

    // 풍차 팬을 회전시키는 함수
    public void RotateFan(float speed)
    {
        if (windmillFan != null && isRepaired)
        {
            // 풍차 팬을 지속적으로 회전
            windmillFan.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        }
    }

    // 코루틴: 일정 시간 대기 후 팬 회전 시작
    private IEnumerator StartFanRotation(float delay)
    {
        yield return new WaitForSeconds(delay);

        isRepaired = true; // 팬이 수리 완료되었다고 표시
        Debug.Log("풍차 팬이 회전을 시작합니다!");
    }

    void Update()
    {
        // 수리 후 계속 회전
        if (isRepaired)
        {
            RotateFan(30f);
        }
    }
}
