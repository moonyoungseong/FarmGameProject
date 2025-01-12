using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 720f;
    public float jumpForce = 7f;  // 점프 힘
    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isPlanting = false;  // 심는 상태를 추적하는 변수
    private bool isPicking = false;  // 작물 캐는 상태를 추적하는 변수

    public TextMeshProUGUI characterNameText; // 캐릭터 이름

    public GameObject maleCharacter;  // 남자 캐릭터 오브젝트
    public GameObject femaleCharacter; // 여자 캐릭터 오브젝트

    private CollectQuestCommand collectQuestCommand; // 퀘스트 테스트

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        string characterName = PlayerPrefs.GetString("CharacterName", "Default Name");

        if (characterNameText != null)
        {
            characterNameText.text = characterName;
        }

        // 캐릭터 이름을 PlayerPrefs에서 불러옴
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "MaleCharacter");

        // 캐릭터 활성화/비활성화
        maleCharacter.SetActive(false);  // 기본적으로 남자 캐릭터 비활성화
        femaleCharacter.SetActive(false); // 기본적으로 여자 캐릭터 비활성화

        if (selectedCharacter == "MaleCharacter")
        {
            maleCharacter.SetActive(true);  // 남자 캐릭터 활성화
        }
        else if (selectedCharacter == "FemaleCharacter")
        {
            femaleCharacter.SetActive(true);  // 여자 캐릭터 활성화
        }

        // 예시로 퀘스트 초기화
        Quest collectQuest = new Quest
        {
            questName = "토마토 3개 수집",
            reward = new List<Reward>  // 보상 목록
            {
                new Reward { itemID = 1, icon = "토마토 아이콘" }
            }
        };

        collectQuestCommand = new CollectQuestCommand(collectQuest, "토마토", 3);
    }

    void Update()
    {
        // 심기 중에는 이동 및 회전을 멈추기
        if (isPlanting | isPicking)
        {
            return;  // 심기 중에는 아무 것도 하지 않음
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool jumpInput = Input.GetKeyDown(KeyCode.Space);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;

        // 이동 처리
        if (moveDirection.magnitude > 0.1f && isGrounded)
        {
            transform.position += moveDirection.normalized * currentSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            animator.SetBool("isWalking", !isRunning);
            animator.SetBool("isRunning", isRunning);
        }
        else if (isGrounded)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        // 점프 처리
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }

        // 예시로 'T' 키를 눌러서 토마토를 수집하는 것처럼 처리
        if (Input.GetKeyDown(KeyCode.T))
        {
            collectQuestCommand.CollectItem("토마토");  // 아이템을 수집
            collectQuestCommand.Execute();              // 퀘스트 진행 체크
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject != null)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            Debug.LogWarning("Collision or collision.gameObject is null");
        }
    }

    // 코루틴으로 심는 동작 처리
    public IEnumerator PlantingAnimationCoroutine(float animationDuration)
    {
        if (!isPlanting)
        {
            isPlanting = true;  // 심기 시작
            animator.SetTrigger("isPlanting");  // 심는 애니메이션 트리거 실행

            yield return new WaitForSeconds(animationDuration);  // 애니메이션 지속 시간 대기

            isPlanting = false;  // 심기 종료
            animator.ResetTrigger("isPlanting");  // 트리거 초기화
        }
    }

    // 코루틴으로 수확 동작 처리
    public IEnumerator PickingAnimationCoroutine(float animationDuration)
    {
        if (!isPicking)
        {
            isPicking = true;
            animator.SetTrigger("isPicking");  // 애니메이션 트리거 실행

            yield return new WaitForSeconds(animationDuration);  // 애니메이션 지속 시간 대기

            isPicking = false;  // 수확 종료 후 이동 가능
            animator.ResetTrigger("isPicking");  // 트리거 초기화
        }
    }
}
