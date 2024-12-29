using UnityEngine;
using System.Collections;

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

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 심기 중에는 이동 및 회전을 멈추기
        if (isPlanting)
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
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
}
