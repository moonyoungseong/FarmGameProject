using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CountManager : MonoBehaviour
{
    public TMP_InputField ItemNameInput, ItemNumberInput;
    private List<Item> myItems;

    void Start()
    {
        // InventoryManager�� ������ �ε��� ��ģ �� �̺�Ʈ�� ���� �˸� �ޱ�
        InventoryManager.Instance.OnDataLoaded += OnInventoryDataLoaded;
    }

    void OnInventoryDataLoaded()
    {
        // MyItemList ��������
        myItems = InventoryManager.Instance.MyItemList;

        // ������ �ε� �Ϸ� �� �۾�
        Debug.Log("������ �����Ͱ� �ε�Ǿ����ϴ�.");
    }

    public void GetItemClick()
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
            Debug.Log($"������ �̸�: {curItem.itemName}");
        }
        else
        {
            Debug.Log("�������� ã�� �� �����ϴ�.");
        }
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
            myItems.Remove(curItem); // ������ ����
            Debug.Log($"������ {curItem.itemName}��(��) ���ŵǾ����ϴ�.");
        }
        else
        {
            Debug.Log("�������� ã�� �� �����ϴ�.");
        }
    }
}

