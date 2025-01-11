using UnityEngine;
using UnityEngine.UI;

public class HarvestableCrop : MonoBehaviour
{
    public CropAttributes cropAttributes; // �۹� �Ӽ� ������
    public Button harvestButton;          // ��Ȯ ��ư

    private void Start()
    {
        // ��ư�� Ŭ�� �̺�Ʈ ����
        if (harvestButton != null)
        {
            harvestButton.onClick.AddListener(HarvestCrop);
        }
        else
        {
            Debug.LogError("��Ȯ ��ư�� �������� �ʾҽ��ϴ�.");
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
