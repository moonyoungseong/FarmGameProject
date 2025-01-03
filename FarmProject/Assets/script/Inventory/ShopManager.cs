using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    void Start()
    {
        // InventoryManager가 데이터 로딩을 마친 후 이벤트를 통해 알림 받기
        InventoryManager.Instance.OnDataLoaded += OnInventoryDataLoaded;
    }

    void OnInventoryDataLoaded()
    {
        // AllItemList 가져오기
        List<Item> allItems = InventoryManager.Instance.AllItemList;

        // MyItemList 가져오기
        List<Item> myItems = InventoryManager.Instance.MyItemList;

        // 특정 아이템 사용
        foreach (var item in allItems)
        {
            Debug.Log($"아이템 이름: {item.itemName}");
        }
    }
}
