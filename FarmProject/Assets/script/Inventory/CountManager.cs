using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CountManager : MonoBehaviour
{
    public static CountManager Instance { get; private set; } // �̱��� ���ٿ�

    public TMP_InputField ItemNameInput, ItemNumberInput;
    private List<Item> myItems;
    private List<Item> allItems;

    private void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
    }

    void Start()
    {
        // InventoryManager�� ������ �ε��� ��ģ �� �̺�Ʈ�� ���� �˸� �ޱ�
        InventoryManager.Instance.OnDataLoaded += OnInventoryDataLoaded;
    }

    void OnInventoryDataLoaded()
    {
        // MyItemList ��������
        myItems = InventoryManager.Instance.MyItemList;
        allItems = InventoryManager.Instance.AllItemList;

        // ������ �ε� �Ϸ� �� �۾�
        Debug.Log("������ �����Ͱ� �ε�Ǿ����ϴ�.");
    }

    public void GetItemClick()
    {
        // myItems�� ����ִ� ��� ó��
        if (myItems == null || myItems.Count == 0)
        {
            Debug.Log("������ ����� ��� �ֽ��ϴ�. �⺻ �������� �߰��մϴ�.");
            AddSpecificItemToMyItems("����", 1);  // "����" �������� 1�� �߰�
        }

        Item curItem = myItems.Find(x => x.itemName == ItemNameInput.text);
        if (curItem != null)
        {
            Debug.Log($"������ �̸�: {curItem.itemName}");

            int parsedPrice;
            if (int.TryParse(curItem.buyPrice, out parsedPrice))
            {
                int total = parsedPrice * int.Parse(ItemNumberInput.text);
                if (GoldManager.Instance.GetGold() >= total)
                {
                    //curItem.quantity = (int.Parse(curItem.quantity) + int.Parse(ItemNumberInput.text)).ToString();
                    //GoldManager.Instance.SubtractGold(parsedPrice * int.Parse(ItemNumberInput.text)); // ���� ���ϱ�
                    // ������ ������ int�� ó�� �� ����
                    int currentQuantity = int.Parse(curItem.quantity); // ���ڿ��� int�� ��ȯ
                    currentQuantity += int.Parse(ItemNumberInput.text); // ���� ������ ���� ���ϱ�
                    curItem.quantity = currentQuantity.ToString(); // ���ŵ� ������ �ٽ� ���ڿ��� ����

                    GoldManager.Instance.SubtractGold(parsedPrice * int.Parse(ItemNumberInput.text)); // ���� ���ϱ�
                }
                else
                {
                    StartCoroutine(GoldManager.Instance.ShowInsufficientGoldUI()); // ���� ���� UI ����
                }
            }
        }
        else
        {
            Debug.Log("�������� ã�� �� �����ϴ�.");
            // ��ü���� ���� �������� ã�� �� �����ۿ� �߰�
            Item curAllItem = allItems.Find(x => x.itemName == ItemNameInput.text);
            if (curAllItem != null)
            {
                Debug.Log($"������ {curAllItem.itemName}��(��) �߰��մϴ�.");
                myItems.Add(curAllItem);
            }
            else
            {
                Debug.Log("�������� ��ü ��Ͽ��� ã�� �� �����ϴ�.");
            }
        }

        toEnd();
    }

    public void ReMoveClick()
    {
        // �����Ͱ� �ε�Ǿ����� Ȯ��
        if (myItems == null || myItems.Count == 0)
        {
            Debug.Log("������ ����� ��� �ְų� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        Item curItem = myItems.Find(x => x.itemName == ItemNameInput.text);
        if (curItem != null)
        {
            int curNumber = int.Parse(curItem.quantity) - int.Parse(ItemNumberInput.text);

            if (curNumber <= 0) myItems.Remove(curItem); // ������ ����
            else curItem.quantity = curNumber.ToString();

            int parsedPrice;
            if (int.TryParse(curItem.sellPrice, out parsedPrice))
            {
                GoldManager.Instance.AddGold(parsedPrice * int.Parse(ItemNumberInput.text)); // ���� ���ϱ�
            }

            Debug.Log($"������ {curItem.itemName}��(��) ���ŵǾ����ϴ�.");
        }
        else
        {
            Debug.Log("�������� ã�� �� �����ϴ�.");
        }
        toEnd();
    }

    public void toEnd() // �ŷ�â �� �κ� ������
    {
        myItems.Sort((p1, p2) =>
        {
            int id1 = int.TryParse(p1.itemID, out int result1) ? result1 : int.MaxValue; // ���� ��ȯ ���� �� �ִ밪���� ó��
            int id2 = int.TryParse(p2.itemID, out int result2) ? result2 : int.MaxValue;

            return id1.CompareTo(id2);
        });

        InventoryManager.Instance.Save();
        ItemNameInput.text = "";
        ItemNumberInput.text = "";
    }

    // Ư�� �����۸� �߰��ϴ� �޼���
    private void AddSpecificItemToMyItems(string itemName, int quantity)
    {
        if (allItems == null || allItems.Count == 0) return;

        // "����"��� �������� ã��, ���ٸ� �߰�
        Item itemToAdd = allItems.Find(x => x.itemName == itemName);
        if (itemToAdd != null)
        {
            // myItems�� �ش� �������� ���ٸ� �߰�
            if (myItems.Find(x => x.itemName == itemName) == null)
            {
                itemToAdd.quantity = quantity.ToString(); // ���� ����
                myItems.Add(itemToAdd);
                Debug.Log($"������ {itemName}��(��) �߰��Ǿ����ϴ�.");
            }
            else
            {
                Debug.Log($"������ {itemName}�� �̹� �����մϴ�.");
            }
        }
        else
        {
            Debug.Log($"������ {itemName}��(��) ��ü ��Ͽ��� ã�� �� �����ϴ�.");
        }
    }

    public void GiveRewardItem(string itemName, int quantity)       // ����Ʈ �����
    {
        if (myItems == null || allItems == null)
        {
            Debug.LogWarning("������ ����� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        // �� �κ��丮���� ã��
        Item curItem = myItems.Find(x => x.itemName == itemName);
        if (curItem != null)
        {
            // �̹� �����ϸ� ������ ����
            int currentQuantity = curItem.quantityInt;
            currentQuantity += quantity;
            curItem.quantity = currentQuantity.ToString();

            Debug.Log($"���� ����: {curItem.itemName}, ���� += {quantity}");
        }
        else
        {
            // allItems���� ã�Ƽ� ���� �߰�
            Item curAllItem = allItems.Find(x => x.itemName == itemName);
            if (curAllItem != null)
            {
                Item newItem = new Item(
                    curAllItem.itemName,
                    curAllItem.itemID,
                    curAllItem.itemIcon,
                    quantity.ToString(),
                    curAllItem.buyPrice,
                    curAllItem.sellPrice,
                    curAllItem.itemType,
                    curAllItem.description
                );

                myItems.Add(newItem);
                Debug.Log($"���� ���� �ű� ������ �߰�: {newItem.itemName}, ����={quantity}");
            }
            else
            {
                Debug.LogWarning($"���� ������ {itemName}�� ��ü ��Ͽ��� ã�� �� �����ϴ�.");
            }
        }

        toEnd();
    }

    public void RemoveItemByID(string itemID, int quantityToConsume)    // ������ ����Ʈ ��� �Ҹ��
    {
        if (myItems == null || myItems.Count == 0)
        {
            Debug.LogWarning("������ ����� ��� �ְų� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        Item item = myItems.Find(x => x.itemID == itemID);
        if (item != null)
        {
            int newQuantity = item.quantityInt - quantityToConsume;
            if (newQuantity <= 0)
            {
                myItems.Remove(item);
                Debug.Log($"������ {item.itemName}��(��) �Ҹ�Ǿ� ���ŵǾ����ϴ�.");
            }
            else
            {
                item.quantity = newQuantity.ToString();
                Debug.Log($"������ {item.itemName} ���� ����: {quantityToConsume}, ���� ����: {newQuantity}");
            }

            InventoryManager.Instance.Save();
        }
        else
        {
            Debug.LogWarning($"������ ID {itemID}��(��) �κ��丮���� ã�� �� �����ϴ�.");
        }
    }

}
