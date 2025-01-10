using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 720f;
    public float jumpForce = 7f;  // ���� ��
    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isPlanting = false;  // �ɴ� ���¸� �����ϴ� ����

    private CollectQuestCommand collectQuestCommand; // ����Ʈ �׽�Ʈ

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // ���÷� ����Ʈ �ʱ�ȭ
        Quest collectQuest = new Quest
        {
            questName = "�丶�� 3�� ����",
            reward = new List<Reward>  // ���� ���
            {
                new Reward { itemID = 1, icon = "�丶�� ������" }
            }
        };

        collectQuestCommand = new CollectQuestCommand(collectQuest, "�丶��", 3);
    }

    void Update()
    {
        // �ɱ� �߿��� �̵� �� ȸ���� ���߱�
        if (isPlanting)
        {
            return;  // �ɱ� �߿��� �ƹ� �͵� ���� ����
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool jumpInput = Input.GetKeyDown(KeyCode.Space);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;

        // �̵� ó��
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

        // ���� ó��
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }

        // ���÷� 'T' Ű�� ������ �丶�並 �����ϴ� ��ó�� ó��
        if (Input.GetKeyDown(KeyCode.T))
        {
            collectQuestCommand.CollectItem("�丶��");  // �������� ����
            collectQuestCommand.Execute();              // ����Ʈ ���� üũ
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

    // �ڷ�ƾ���� �ɴ� ���� ó��
    public IEnumerator PlantingAnimationCoroutine(float animationDuration)
    {
        if (!isPlanting)
        {
            isPlanting = true;  // �ɱ� ����
            animator.SetTrigger("isPlanting");  // �ɴ� �ִϸ��̼� Ʈ���� ����

            yield return new WaitForSeconds(animationDuration);  // �ִϸ��̼� ���� �ð� ���

            isPlanting = false;  // �ɱ� ����
            animator.ResetTrigger("isPlanting");  // Ʈ���� �ʱ�ȭ
        }
    }
}
