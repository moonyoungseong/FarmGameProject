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
        // InventoryManager가 데이터 로딩을 마친 후 이벤트를 통해 알림 받기
        InventoryManager.Instance.OnDataLoaded += OnInventoryDataLoaded;
    }

    void OnInventoryDataLoaded()
    {
        // MyItemList 가져오기
        myItems = InventoryManager.Instance.MyItemList;

        // 데이터 로딩 완료 후 작업
        Debug.Log("아이템 데이터가 로드되었습니다.");
    }

    public void GetItemClick()
    {
        // 데이터가 로드되었는지 확인
        if (myItems == null || myItems.Count == 0)
        {
            Debug.Log("아이템 목록이 비어 있거나 초기화되지 않았습니다.");
            return;
        }

        Item curItem = myItems.Find(x => x.itemName == ItemNameInput.text);
        if (curItem != null)
        {
            Debug.Log($"아이템 이름: {curItem.itemName}");
        }
        else
        {
            Debug.Log("아이템을 찾을 수 없습니다.");
        }
    }

    public void ReMoveClick()
    {
        // 데이터가 로드되었는지 확인
        if (myItems == null || myItems.Count == 0)
        {
            Debug.Log("아이템 목록이 비어 있거나 초기화되지 않았습니다.");
            return;
        }

        Item curItem = myItems.Find(x => x.itemName == ItemNameInput.text);
        if (curItem != null)
        {
            myItems.Remove(curItem); // 아이템 제거
            Debug.Log($"아이템 {curItem.itemName}이(가) 제거되었습니다.");
        }
        else
        {
            Debug.Log("아이템을 찾을 수 없습니다.");
        }
    }
}

