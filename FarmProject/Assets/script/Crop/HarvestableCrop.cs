using UnityEngine;
using UnityEngine.UI;

public class HarvestableCrop : MonoBehaviour
{
    public CropAttributes cropAttributes; // �۹� �Ӽ� ������
    public Button harvestButton;          // ��Ȯ ��ư
    public Transform player1;              // �� �÷��̾� Transform
    public Transform player2;               // �� �÷��̾� Transform
    private Transform player;                 // �÷��̾��� Transform
    public float interactDistance = 2f;   // �۹��� ��ȣ�ۿ� ������ �Ÿ�

    // ������ �ִ� playerMoveScript ���� ���
    private PlayerMove playerMoveScript;

    private void Start()
    {
        if (player1 == null)
        {
            player1 = GameObject.Find("Player").transform;
        }
        else if (player2 == null) player2 = GameObject.Find("Player_W").transform;

        // �� �ε� �� Ȱ��ȭ�� �÷��̾ ���� playerMoveScript �Ҵ�
        if (player1.gameObject.activeInHierarchy)
        {
            playerMoveScript = player1.GetComponent<PlayerMove>(); // Player1�� PlayerMove ��ũ��Ʈ �Ҵ�
            player = player1.transform;  // �÷��̾� Ʈ������ ����
        }
        else if (player2.gameObject.activeInHierarchy)
        {
            playerMoveScript = player2.GetComponent<PlayerMove>(); // Player2�� PlayerMove ��ũ��Ʈ �Ҵ�
            player = player2.transform;  // �÷��̾� Ʈ������ ����
        }

        // ��ư Ŭ�� �̺�Ʈ ����
        if (harvestButton != null)
        {
            harvestButton.onClick.AddListener(HarvestCrop);
            harvestButton.gameObject.SetActive(false); // ó���� ��ư ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("��Ȯ ��ư�� �������� �ʾҽ��ϴ�.");
        }
    }

    private void Update()
    {
        // �÷��̾�� �۹� �� �Ÿ� ���
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);

            // ��ȣ�ۿ� �Ÿ� �̳��� ��ư Ȱ��ȭ, �ƴϸ� ��Ȱ��ȭ
            if (distanceToPlayer <= interactDistance)
            {
                if (!harvestButton.gameObject.activeSelf)
                {
                    harvestButton.gameObject.SetActive(true);
                }
            }
            else
            {
                if (harvestButton.gameObject.activeSelf)
                {
                    harvestButton.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("�÷��̾� Transform�� �������� �ʾҽ��ϴ�.");
        }
    }

    public void HarvestCrop()
    {
        if (cropAttributes == null)
        {
            Debug.LogError("�۹� �Ӽ� �����Ͱ� �����ϴ�.");
            return;
        }

        Debug.Log($"{cropAttributes.name}��(��) ��Ȯ�߽��ϴ�!");

        //StartCoroutine(playerMoveScript.PickingAnimationCoroutine(5f)); // �ִϸ��̼� ����

        // ��Ȯ ó�� ����
        AddCropToInventory(cropAttributes);

        // ��Ȯ �� ������Ʈ ����
        Destroy(gameObject);
    }

    private void AddCropToInventory(CropAttributes cropAttributes)
    {
        // AllItemList���� �ش� �۹��� ã��
        Item cropItem = InventoryManager.Instance.AllItemList.Find(item => item.itemID == cropAttributes.id);

        if (cropItem != null)
        {
            // MyItemList���� �ش� �۹��� ã��
            Item inventoryItem = InventoryManager.Instance.MyItemList.Find(item => item.itemID == cropAttributes.id);

            if (inventoryItem != null)
            {
                // �̹� �����ϴ� �۹��� ��� ���� ����
                if (int.TryParse(inventoryItem.quantity, out int currentQuantity))
                {
                    inventoryItem.quantity = (currentQuantity + 1).ToString(); // cropAttributes.yieldAmount
                }
                else
                {
                    Debug.LogError($"'{inventoryItem.itemName}'�� ���� �����Ͱ� �߸��Ǿ����ϴ�.");
                }
            }
            else
            {
                // MyItemList�� ���� ��� AllItemList���� ã�� cropItem�� MyItemList�� �߰�
                Item newItem = new Item(
                    cropItem.itemName,
                    cropItem.itemID,
                    cropItem.itemIcon,
                    cropItem.quantity,
                    cropItem.buyPrice,
                    cropItem.sellPrice,
                    cropItem.itemType,
                    cropItem.description
                );

                InventoryManager.Instance.MyItemList.Add(newItem);
            }
        }
        else
        {
            Debug.LogError($"AllItemList���� �۹� ID '{cropAttributes.id}'�� ���� ������ ã�� �� �����ϴ�.");
            return;
        }

        // �κ��丮 ���� �� UI ����
        InventoryManager.Instance.Save();
    }
}
