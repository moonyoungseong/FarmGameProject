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

    private InventoryManager inventory;

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ���
        collectButton.onClick.AddListener(CollectHoney);
        waitButton.onClick.AddListener(OnWaitButtonClicked);

        UpdateUI();
        UpdateButtonStates(); // �ʱ� ��ư ���� ����
        notificationUI.SetActive(false); // UI �ʱ� ���´� ��Ȱ��ȭ

        // �̱����� ���� InventoryManager�� ����
        inventory = InventoryManager.Instance;
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

            AddItemToInventory("��", 3);

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

    public void AddItemToInventory(string itemName, int quantityToAdd)
    {
        Debug.Log($"AddItemToInventory ȣ���: {itemName}, �߰��� ����: {quantityToAdd}");              // �� ��������� ȣ�� ��

        // AllItemList���� ������ �˻�
        Item targetItem = inventory.AllItemList.Find(item => item.itemName == itemName);

        if (targetItem != null)
        {
            Debug.Log($"������ {itemName}�� AllItemList���� �߰ߵ�");

            // �̹� MyItemList�� �ִ��� Ȯ��
            Item existingItem = inventory.MyItemList.Find(item => item.itemID == targetItem.itemID);

            if (existingItem != null)
            {
                Debug.Log($"{itemName}�� MyItemList�� �̹� ������, ���� ����: {existingItem.quantity}");

                // �����ϸ� ���� ����
                int currentQuantity = int.Parse(existingItem.quantity);
                existingItem.quantity = (currentQuantity + quantityToAdd).ToString();
                Debug.Log($"{itemName}�� ������ {quantityToAdd}�� �����߽��ϴ�. ���ο� ����: {existingItem.quantity}");
            }
            else
            {
                Debug.Log($"{itemName}�� MyItemList�� ����, ���� �߰�");

                // �������� ������ ���� �߰�
                targetItem.quantity = quantityToAdd.ToString();
                inventory.MyItemList.Add(targetItem);
                Debug.Log($"{itemName} {quantityToAdd}���� MyItemList�� �߰��Ǿ����ϴ�.");
            }

            // ���� ���� ����
            inventory.Save();
            Debug.Log($"�κ��丮 �����");
        }
        else
        {
            Debug.LogWarning($"{itemName}�̶�� �������� AllItemList���� ã�� �� �����ϴ�.");
        }
    }
}
