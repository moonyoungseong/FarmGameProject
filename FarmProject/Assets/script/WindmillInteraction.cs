using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillInteraction : MonoBehaviour
{
    private MovementQuestCommand movementQuest;

    public GameObject player1;               // �÷��̾� 1
    public GameObject player2;               // �÷��̾� 2
    public GameObject ladder1;               // ��ٸ��� UI
    public GameObject ladder2;               // ��ٸ��� UI
    public GameObject FinishImage;           // �Ϸ� UI
    public GameObject[] LadderAbout; // ��ٸ� Ÿ�� ���� �����Ұ͵�
    public GameObject windmillFan; // ǳ�� �� ������Ʈ

    private bool isRepaired = false; // ���� ���¸� ����

    public Animator playerAnimator; // Animator�� �Ҵ��ؾ� ��

    private Transform player;                // ���� Ȱ��ȭ�� �÷��̾��� Transform�� ����

    void Start()
    {
        // �� �ε� �� Ȱ��ȭ�� �÷��̾ ���� player �Ҵ�
        if (player1.activeInHierarchy)
        {
            player = player1.transform;  // �÷��̾� 1�� Transform ����
        }
        else if (player2.activeInHierarchy)
        {
            player = player2.transform;  // �÷��̾� 2�� Transform ����
        }

        // Animator�� �÷��̾��� ������Ʈ�� ����
        playerAnimator = player.GetComponent<Animator>();

        // ����: Windmill ���� ����Ʈ ����
        Quest windmillQuest = QuestManager.Instance.GetQuestByID(10);
        movementQuest = new MovementQuestCommand(windmillQuest, "ǳ�� ����");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���ſ� ������ ������Ʈ�� Ư�� �±׸� ���� ���
        if (other.CompareTag("WindZone"))
        {
#if UNITY_EDITOR
            Debug.Log("�÷��̾ Ư�� Trigger Zone�� ���Խ��ϴ�!");
#endif

            ladder1.SetActive(true);
        }

        // Ʈ���ſ� ������ ������Ʈ�� Ư�� �±׸� ���� ���
        if (other.CompareTag("WindZone2"))
        {
#if UNITY_EDITOR
            Debug.Log("�÷��̾ Ư�� Trigger Zone�� ���Խ��ϴ�!");
#endif
            ladder2.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ʈ���ſ��� ���� ������Ʈ�� Ư�� �±׸� ���� ���
        if (other.CompareTag("WindZone"))
        {
#if UNITY_EDITOR
            Debug.Log("�÷��̾ Ư�� Trigger Zone���� �������ϴ�!");
#endif
            ladder1.SetActive(false);
        }

        // Ʈ���ſ��� ���� ������Ʈ�� Ư�� �±׸� ���� ���
        if (other.CompareTag("WindZone2"))
        {
#if UNITY_EDITOR
            Debug.Log("�÷��̾ Ư�� Trigger Zone���� �������ϴ�!");
#endif
            ladder2.SetActive(false);
            LadderAbout[0].SetActive(false);
            LadderAbout[1].SetActive(false);

            // �� ȸ���� �����ϱ� ���� �ڷ�ƾ ȣ��
            StartCoroutine(StartFanRotation(3f)); // 3�� �Ŀ� �� ȸ�� ����
        }
    }

    public void GotoMove()
    {
        // ���� Ȱ��ȭ�� �÷��̾ �ִٸ� �̵� �� ȸ�� ����
        if (player != null)
        {
            // ù ��° ��ġ�� ��� �̵�
            Vector3 instantPosition = new Vector3(10.56f, 2.71f, 34.41f);
            Vector3 instantRotation = new Vector3(0f, 169.016f, 0f);
            MoveInit(player, instantPosition, instantRotation);

            // �� ��° ��ġ�� õõ�� �̵�
            Vector3 smoothPosition = new Vector3(11.15f, 9.4f, 31.7f);
            Vector3 smoothRotation = new Vector3(0f, 180f, 0f);

            // �� ��° ��ġ�� õõ�� �̵� (�ִϸ��̼� ����)
            StartCoroutine(SmoothMoveWithAnimation(player, smoothPosition, 3f)); // 3�� ���� �̵�
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
        // �ִϸ��̼� ����
        playerAnimator.SetBool("isFixing", true);

        // 5�� ���� ���
        yield return new WaitForSeconds(5f);

        // �ִϸ��̼� ����
        playerAnimator.SetBool("isFixing", false);

        FinishImage.SetActive(true);        // �Ϸ� �ȳ���

        //  �̵��� ����Ʈ �Ϸ� üũ
        if (movementQuest != null)
        {
            bool result = movementQuest.ReachTarget();
            Debug.Log("���� ����Ʈ �Ϸ� ����: " + result);
        }
    }

    // MoveInit �Լ�: ��� �̵��� ȸ�� ����
    public void MoveInit(Transform target, Vector3 targetPosition, Vector3 targetRotation)
    {
        if (target != null)
        {
            // ��� ��ġ ����
            target.position = targetPosition;

            // ��� ȸ�� ���� (Vector3�� Quaternion���� ��ȯ)
            target.rotation = Quaternion.Euler(targetRotation);

#if UNITY_EDITOR
            Debug.Log($"�÷��̾ {targetPosition} ��ġ�� ��� �̵��߽��ϴ�.");
#endif
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("�÷��̾ �������� �ʾҽ��ϴ�.");
#endif
        }
    }

    private IEnumerator SmoothMoveWithAnimation(Transform target, Vector3 targetPosition, float duration)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isClimbing", true); // �̵� �ִϸ��̼� ����
            playerAnimator.speed = 1f / duration;      // �ִϸ��̼� �ӵ� ����
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
            playerAnimator.SetBool("isClimbing", false); // �̵� �ִϸ��̼� ����
            playerAnimator.speed = 1f;                  // �ִϸ��̼� �ӵ� �ʱ�ȭ
        }
    }

    // ǳ�� ���� ȸ����Ű�� �Լ�
    public void RotateFan(float speed)
    {
        if (windmillFan != null && isRepaired)
        {
            // ǳ�� ���� ���������� ȸ��
            windmillFan.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        }
    }

    // �ڷ�ƾ: ���� �ð� ��� �� �� ȸ�� ����
    private IEnumerator StartFanRotation(float delay)
    {
        yield return new WaitForSeconds(delay);

        isRepaired = true; // ���� ���� �Ϸ�Ǿ��ٰ� ǥ��

#if UNITY_EDITOR
        Debug.Log("ǳ�� ���� ȸ���� �����մϴ�!");
#endif
    }

    void Update()
    {
        // ���� �� ��� ȸ��
        if (isRepaired)
        {
            RotateFan(30f);
        }
    }
}
