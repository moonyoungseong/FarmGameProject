using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WindmillInteraction.cs
/// 풍차 수리 퀘스트를 진행하는 동안 플레이어 이동, 사다리 상호작용,
/// 퀘스트 완료 처리, 풍차 팬 회전, UI 활성화를 담당하는 클래스.
/// 
/// - 트리거 진입/이탈 시 사다리 UI 활성화
/// - 플레이어 수리 애니메이션 재생
/// - 수리 완료 후 퀘스트 완료 처리
/// - 풍차 팬 회전 시작
/// - 플레이어를 특정 좌표로 부드럽게 이동시키는 기능 포함
/// </summary>
public class WindmillInteraction : MonoBehaviour
{
    private MovementQuestCommand movementQuest;

    public GameObject player1;               // 플레이어 남자
    public GameObject player2;               // 플레이어 여자
    public GameObject ladder1;               // 1번 사다리 UI (밑)
    public GameObject ladder2;               // 2번 사다리 UI (위)
    public GameObject FinishImage;           // 수리 완료 UI
    public GameObject[] LadderAbout;         // 사다리 타고 올라간 후 제거할 UI/오브젝트들
    public GameObject windmillFan;           // 풍차 회전하는 fan 오브젝트

    public bool isRepaired = false;          // 풍차 팬 회전 가능 여부 (수리 완료 신호)
    public bool repairDone = false;          // 퀘스트 완료 여부

    public Animator playerAnimator;          

    private Transform player;                // 실제 현재 사용 중인 플레이어 Transform

    /// <summary>
    /// 초기에 활성화된 플레이어를 자동 선택해서 player 변수에 할당하고  
    /// 퀘스트를 MovementQuestCommand로 등록
    /// </summary>
    void Start()
    {
        // 현재 활성화된 플레이어 선택
        if (player1.activeInHierarchy)
            player = player1.transform;
        else if (player2.activeInHierarchy)
            player = player2.transform;

        // 선택된 플레이어 애니메이터 가져오기
        playerAnimator = player.GetComponent<Animator>();

        // 퀘스트 초기 설정
        Quest windmillQuest = QuestManager.Instance.GetQuestByID(10);
        movementQuest = new MovementQuestCommand(windmillQuest, "풍차 수리", QuestManager.Instance.questListController);
    }

    // 사다리 구역에 플레이어가 진입하면 UI를 활성화한다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WindZone"))
        {
#if UNITY_EDITOR
            Debug.Log("플레이어가 WindZone(첫 번째 사다리)에 진입");
#endif
            ladder1.SetActive(true);
        }

        if (other.CompareTag("WindZone2"))
        {
#if UNITY_EDITOR
            Debug.Log("플레이어가 WindZone2(두 번째 사다리)에 진입");
#endif
            ladder2.SetActive(true);
        }
    }

    /// <summary>
    /// 사다리 구역에서 벗어날 때 UI를 숨기고,
    /// 두 번째 사다리에서 벗어날 경우 풍차 회전 코루틴을 시작
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WindZone"))
        {
            ladder1.SetActive(false);
        }

        if (other.CompareTag("WindZone2"))
        {
            ladder2.SetActive(false);

            // 사다리 관련 UI/오브젝트 비활성화
            LadderAbout[0].SetActive(false);
            LadderAbout[1].SetActive(false);

            // 일정 시간 후 풍차 회전 시작
            StartCoroutine(StartFanRotation(3f));
        }
    }

    /// <summary>
    /// 플레이어를 지정된 두 좌표로 이동시킨다.
    /// 첫 번째는 즉시 이동, 두 번째는 부드럽게 이동.
    /// </summary>
    public void GotoMove()
    {
        if (player != null)
        {
            // 즉시 이동
            MoveInit(player, new Vector3(10.56f, 2.71f, 34.41f), new Vector3(0f, 169.016f, 0f));

            // 부드럽게 이동
            Vector3 smoothPosition = new Vector3(11.15f, 9.4f, 31.7f);
            StartCoroutine(SmoothMoveWithAnimation(player, smoothPosition, 3f));
        }
    }

    /// <summary>
    /// 풍차 수리 애니메이션을 5초 동안 재생하는 함수.
    /// 완료 후 UI 표시 및 퀘스트 완료 처리 호출.
    /// </summary>
    public void FIxWind()
    {
        if (playerAnimator != null)
            StartCoroutine(PlayFixWindAnimation());
    }

    private IEnumerator PlayFixWindAnimation()
    {
        playerAnimator.SetBool("isFixing", true);

        yield return new WaitForSeconds(5f);

        // 수리 완료 UI
        playerAnimator.SetBool("isFixing", false);
        FinishImage.SetActive(true);

        // 퀘스트 완료 처리
        if (movementQuest != null)
        {
            FixWindmill();
        }
    }

    // 풍차 수리 완료 플래그를 true로 변경한다.
    public void FixWindmill()
    {
        repairDone = true;
    }

    public bool IsFixed()
    {
        return isRepaired;
    }

    // 플레이어를 즉시 특정 위치/회전으로 순간이동시키는 함수
    public void MoveInit(Transform target, Vector3 targetPosition, Vector3 targetRotation)
    {
        target.position = targetPosition;
        target.rotation = Quaternion.Euler(targetRotation);
    }

    /// <summary>
    /// 플레이어를 시간에 걸쳐 부드럽게 이동시키는 코루틴  
    /// 이동 중 climbing 애니메이션 재생
    /// </summary>
    private IEnumerator SmoothMoveWithAnimation(Transform target, Vector3 targetPosition, float duration)
    {
        playerAnimator.SetBool("isClimbing", true);
        playerAnimator.speed = 1f / duration;

        Vector3 startPos = target.position;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            target.position = Vector3.Lerp(startPos, targetPosition, time / duration);
            yield return null;
        }

        target.position = targetPosition;

        // 애니메이션 종료
        playerAnimator.SetBool("isClimbing", false);
        playerAnimator.speed = 1f;
    }

    /// <summary>
    /// 실제 풍차 팬을 회전시키는 함수  
    /// isRepaired == true일 때만 회전
    /// </summary>
    public void RotateFan(float speed)
    {
        if (windmillFan != null && isRepaired)
            windmillFan.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }

    // 일정 시간 대기 후 풍차 팬 회전을 시작하도록 isRepaired를 true로 변경한다.
    private IEnumerator StartFanRotation(float delay)
    {
        yield return new WaitForSeconds(delay);
        isRepaired = true;
    }

    // 매 프레임 풍차 팬 회전 여부를 검사 후 회전
    void Update()
    {
        if (isRepaired)
            RotateFan(30f);
    }
}
