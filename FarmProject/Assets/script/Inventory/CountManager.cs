using UnityEngine;
using System.Collections.Generic;

public class CountManager : MonoBehaviour
{
    void Start()
    {
        // InventoryManager�� ������ �ε��� ��ģ �� �̺�Ʈ�� ���� �˸� �ޱ�
        InventoryManager.Instance.OnDataLoaded += OnInventoryDataLoaded;
    }

    void OnInventoryDataLoaded()
    {
        // AllItemList ��������
        List<Item> allItems = InventoryManager.Instance.AllItemList;

        // MyItemList ��������
        List<Item> myItems = InventoryManager.Instance.MyItemList;

        // CurItemList ��������
        List<Item> curItemList = InventoryManager.Instance.CurItemList;

        // Ư�� ������ ���
        foreach (var item in allItems)
        {
            //Debug.Log($"������ �̸�: {item.itemName}");
        }
    }
}
