using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// PlayerMove.cs
/// 
/// - 플레이어 캐릭터 이동, 점프, 달리기, 회전, 발소리 재생 등을 처리.
/// - 심기(Planting)와 수확(Picking) 상태 중에는 이동 및 회전 제한.
/// - 캐릭터 이름 표시, 선택한 캐릭터(Male/Female) 활성화 처리.
/// - 발소리 SFX 재생, 심기/수확 애니메이션 코루틴 포함.
/// </summary>
public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 720f; // 회전 속도
    public float jumpForce = 7f; // 점프 힘

    private Animator animator; 
    private Rigidbody rb;     
    private bool isGrounded = true; // 지면 착지 여부
    private bool isPlanting = false; // 심기 상태 여부
    private bool isPicking = false;  // 수확 상태 여부

    // UI에 표시할 캐릭터 이름 
    public TextMeshProUGUI characterNameText;

    private Coroutine footstepCoroutine; // 발소리 코루틴 참조
    private bool isMoving;               // 이동 중 여부
    private AudioSource footstepSource;  // 발소리 AudioSource

    // 남자 캐릭터 오브젝트
    public GameObject maleCharacter;

    // 여자 캐릭터 오브젝트
    public GameObject femaleCharacter;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // PlayerPrefs에서 캐릭터 이름 불러오기
        string characterName = PlayerPrefs.GetString("CharacterName", "Default Name");
        if (characterNameText != null)
        {
            characterNameText.text = characterName;
        }

        // 선택된 캐릭터 불러오기 및 활성화
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "MaleCharacter");
        maleCharacter.SetActive(false);
        femaleCharacter.SetActive(false);

        if (selectedCharacter == "MaleCharacter") maleCharacter.SetActive(true);
        else if (selectedCharacter == "FemaleCharacter") femaleCharacter.SetActive(true);

        footstepSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // 심기 또는 수확 중이면 이동 및 회전 중지
        if (isPlanting || isPicking) return;

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
                if (footstepCoroutine != null) StopCoroutine(footstepCoroutine);
                if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
            }
        }

        // 점프 처리
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }
    }

    /// <summary>
    /// 발소리 재생 코루틴, 소리 관련
    /// </summary>
    /// <param name="isRunning">달리기 여부</param>
    private IEnumerator PlayFootsteps(bool isRunning)
    {
        int walkSFX = 12;
        int runSFX = 9;

        while (isMoving && isGrounded)
        {
            if (!footstepSource.isPlaying)
            {
                AudioClipData data = AudioManager.Instance.sfxList[isRunning ? runSFX : walkSFX];
                footstepSource.clip = data.clip;
                footstepSource.volume = data.volume;
                footstepSource.loop = false;
                footstepSource.pitch = data.useRandomPitch
                    ? Random.Range(data.minPitch, data.maxPitch)
                    : data.pitch;

                footstepSource.Play();
            }

            float interval = isRunning ? 0f : 0f; // 발소리 간격 (조정 가능)
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject != null && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
        else
        {
            Debug.LogWarning("Collision or collision.gameObject is null");
        }
    }

    // 심기 애니메이션 코루틴
    public IEnumerator PlantingAnimationCoroutine(float animationDuration)
    {
        if (!isPlanting)
        {
            isPlanting = true;
            animator.SetTrigger("isPlanting");
            StartCoroutine(DelayedPlantSound());

            yield return new WaitForSeconds(animationDuration);

            isPlanting = false;
            animator.ResetTrigger("isPlanting");
        }
    }

    private IEnumerator DelayedPlantSound()
    {
        yield return new WaitForSeconds(3f);
        AudioManager.Instance.PlaySFXRepeated(6, 4, 1f);
    }

    // 수확 애니메이션 코루틴
    public IEnumerator PickingAnimationCoroutine(float animationDuration)
    {
        if (!isPicking)
        {
            isPicking = true;
            animator.SetTrigger("isPicking");
            AudioManager.Instance.PlaySFXRepeated(5, 2, 1.5f);

            yield return new WaitForSeconds(animationDuration);

            isPicking = false;
            animator.ResetTrigger("isPicking");
        }
    }
}
