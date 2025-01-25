using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HoneyCollector : MonoBehaviour
{
    public TextMeshProUGUI timerText; // ���� �ð��� ǥ���� UI Text
    public Button collectButton; // �� ���� ��ư
    public Button waitButton; // ��� ��ư
    public GameObject notificationUI; // Ư�� UI (�˸� �޽��� ��)

    private int honeyCount = 0; // ���� ������ �� ����
    private float timer = 10f; // Ÿ�̸� �ʱⰪ (10��)

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ���
        collectButton.onClick.AddListener(CollectHoney);
        waitButton.onClick.AddListener(OnWaitButtonClicked);

        UpdateUI();
        UpdateButtonStates(); // �ʱ� ��ư ���� ����
        notificationUI.SetActive(false); // UI �ʱ� ���´� ��Ȱ��ȭ
    }

    void Update()
    {
        // Ÿ�̸� ����
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
                timer = 0;

            UpdateUI();
        }
        else if (honeyCount == 0) // �ð��� 0�� �Ǹ� �� ����
        {
            honeyCount = 3;
            UpdateButtonStates(); // ��ư ���� ������Ʈ
        }
    }

    void CollectHoney()
    {
        if (honeyCount > 0)
        {
            // ���� �κ��丮�� �߰� (���⼭�� Debug�� Ȯ��)
            Debug.Log($"�� {honeyCount}���� �����߽��ϴ�!");

            honeyCount = 0; // ������ �� �ʱ�ȭ
            timer = 10f; // Ÿ�̸� �ʱ�ȭ (10��)
            UpdateUI();
            UpdateButtonStates(); // ��ư ���� ������Ʈ
        }
    }

    void OnWaitButtonClicked()
    {
        Debug.Log("��� ��ư Ŭ����");
        StartCoroutine(ShowNotification()); // �˸� UI ǥ�� �ڷ�ƾ ȣ��
    }

    IEnumerator ShowNotification()
    {
        notificationUI.SetActive(true); // UI Ȱ��ȭ
        yield return new WaitForSeconds(2f); // 2�� ���
        notificationUI.SetActive(false); // UI ��Ȱ��ȭ
    }

    void UpdateUI()
    {
        // �ð� ������ ��:�� �Ǵ� �ʷ� ǥ��
        if (timer >= 60)
        {
            int minutes = Mathf.FloorToInt(timer / 60); // �� ���
            int seconds = Mathf.FloorToInt(timer % 60); // �� ���
            timerText.text = $"���� �ð�: {minutes}�� {seconds}��";
        }
        else
        {
            int seconds = Mathf.FloorToInt(timer); // �ʸ� ǥ��
            timerText.text = $"���� �ð�: {seconds}��";
        }
    }

    void UpdateButtonStates()
    {
        // �ð��� ���� ������ ��� ��ư�� Ȱ��ȭ
        if (timer > 0 || honeyCount == 0)
        {
            waitButton.gameObject.SetActive(true);
            collectButton.gameObject.SetActive(false);
        }
        else
        {
            // �ð��� 0�� �ǰ� ���� �����Ǿ����� ���� ��ư Ȱ��ȭ
            waitButton.gameObject.SetActive(false);
            collectButton.gameObject.SetActive(true);
        }
    }
}
