using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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
    private bool isPicking = false;  // �۹� ĳ�� ���¸� �����ϴ� ����

    public TextMeshProUGUI characterNameText; // ĳ���� �̸�

    private Coroutine footstepCoroutine;
    private bool isMoving;
    private AudioSource footstepSource;

    public GameObject maleCharacter;  // ���� ĳ���� ������Ʈ
    public GameObject femaleCharacter; // ���� ĳ���� ������Ʈ

    private CollectQuestCommand collectQuestCommand; // ����Ʈ �׽�Ʈ

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        string characterName = PlayerPrefs.GetString("CharacterName", "Default Name");

        if (characterNameText != null)
        {
            characterNameText.text = characterName;
        }

        // ĳ���� �̸��� PlayerPrefs���� �ҷ���
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "MaleCharacter");

        // ĳ���� Ȱ��ȭ/��Ȱ��ȭ
        maleCharacter.SetActive(false);  // �⺻������ ���� ĳ���� ��Ȱ��ȭ
        femaleCharacter.SetActive(false); // �⺻������ ���� ĳ���� ��Ȱ��ȭ

        if (selectedCharacter == "MaleCharacter")
        {
            maleCharacter.SetActive(true);  // ���� ĳ���� Ȱ��ȭ
        }
        else if (selectedCharacter == "FemaleCharacter")
        {
            femaleCharacter.SetActive(true);  // ���� ĳ���� Ȱ��ȭ
        }

        // ���÷� ����Ʈ �ʱ�ȭ
        Quest collectQuest = new Quest
        {
            questName = "�丶�� 3�� ����",
            reward = new List<Reward>  // ���� ���
            {
                new Reward { itemID = 1, icon = "�丶�� ������" }
            }
        };

        collectQuestCommand = new CollectQuestCommand(collectQuest, "�丶��", 3, QuestManager.Instance.questListController);

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.spatialBlend = 0; // 2D�� ���
    }

    void Update()
    {
        // �ɱ� �߿��� �̵� �� ȸ���� ���߱�
        if (isPlanting || isPicking)
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

            if (!isMoving)
            {
                isMoving = true;
                footstepCoroutine = StartCoroutine(PlayFootsteps(isRunning));
            }
        }
        else if (isGrounded)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);

            if (isMoving)
            {
                isMoving = false;
                if (footstepCoroutine != null)
                    StopCoroutine(footstepCoroutine);

                AudioManager.Instance.StopAllSFX();
            }
        }

        // ���� ó��
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }
    }

    private IEnumerator PlayFootsteps(bool isRunning)
    {
        // �ȱ�/�ٱ� ȿ���� �и�
        int walkSFX = 12; // AudioManager�� SFX ����Ʈ���� �ȱ� �߼Ҹ� �ε���
        int runSFX = 9;  // AudioManager�� SFX ����Ʈ���� �ٱ� �߼Ҹ� �ε���

        float interval = isRunning ? 0.35f : 0.6f;

        while (isMoving && isGrounded)
        {
            AudioManager.Instance.PlaySFX(isRunning ? runSFX : walkSFX);
            yield return new WaitForSeconds(interval);
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

    // �ڷ�ƾ���� �ɴ� ���� ó��
    public IEnumerator PlantingAnimationCoroutine(float animationDuration)
    {
        if (!isPlanting)
        {
            isPlanting = true;  // �ɱ� ����
            animator.SetTrigger("isPlanting");  // �ɴ� �ִϸ��̼� Ʈ���� ����
            StartCoroutine(DelayedPlantSound());

            yield return new WaitForSeconds(animationDuration);  // �ִϸ��̼� ���� �ð� ���

            isPlanting = false;  // �ɱ� ����
            animator.ResetTrigger("isPlanting");  // Ʈ���� �ʱ�ȭ
        }
    }

    private IEnumerator DelayedPlantSound()
    {
        yield return new WaitForSeconds(3f); // 3�� ���
        AudioManager.Instance.PlaySFXRepeated(6, 4, 1f);
    }

    // �ڷ�ƾ���� ��Ȯ ���� ó��
    public IEnumerator PickingAnimationCoroutine(float animationDuration)
    {
        if (!isPicking)
        {
            isPicking = true;
            animator.SetTrigger("isPicking");  // �ִϸ��̼� Ʈ���� ����
            AudioManager.Instance.PlaySFXRepeated(5, 2, 1.5f);  // �۹� ��Ȯ�ϴ� ȿ����
            Debug.Log("Picking started."); // ���� �� �α�

            yield return new WaitForSeconds(animationDuration);  // �ִϸ��̼� ���� �ð� ���

            isPicking = false;  // ��Ȯ ���� �� �̵� ����
            animator.ResetTrigger("isPicking");  // Ʈ���� �ʱ�ȭ
            Debug.Log("Picking ended. isPicking: " + isPicking); // ���� �� �α�
        }
    }
}
