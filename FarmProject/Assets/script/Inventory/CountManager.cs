using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CountManager : MonoBehaviour
{
    public TMP_InputField ItemNameInput, ItemNumberInput;
    private List<Item> myItems;
    private List<Item> allItems;

    void Start()
    {
        // InventoryManager가 데이터 로딩을 마친 후 이벤트를 통해 알림 받기
        InventoryManager.Instance.OnDataLoaded += OnInventoryDataLoaded;
    }

    void OnInventoryDataLoaded()
    {
        // MyItemList 가져오기
        myItems = InventoryManager.Instance.MyItemList;

        allItems = InventoryManager.Instance.AllItemList;

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
            curItem.quantity = (int.Parse(curItem.quantity) + int.Parse(ItemNumberInput.text)).ToString();

            int parsedPrice;
            if (int.TryParse(curItem.buyPrice, out parsedPrice))
            {
                GoldManager.Instance.SubtractGold(parsedPrice * int.Parse(ItemNumberInput.text)); // 가격 구하기
            }
        }
        else
        {
            Debug.Log("아이템을 찾을 수 없습니다.");
            // 전체에서 얻을 아이템을 찾아 내 아이템에 추가
            Item curAllItem = allItems.Find(x => x.itemName == ItemNameInput.text);
            if (curAllItem != null) myItems.Add(curAllItem);
        }
        toEnd();
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
            int curNumber = int.Parse(curItem.quantity) - int.Parse(ItemNumberInput.text);

            if (curNumber <= 0) myItems.Remove(curItem); // 아이템 제거
            else curItem.quantity = curNumber.ToString();

            int parsedPrice;
            if (int.TryParse(curItem.sellPrice, out parsedPrice))
            {
                GoldManager.Instance.AddGold(parsedPrice * int.Parse(ItemNumberInput.text)); // 가격 구하기
            }

            Debug.Log($"아이템 {curItem.itemName}이(가) 제거되었습니다.");
        }
        else
        {
            Debug.Log("아이템을 찾을 수 없습니다.");
        }
        toEnd();
    }

    public void toEnd() // 거래창 및 인벤 정리용
    {
        myItems.Sort((p1, p2) =>
        {
            int id1 = int.TryParse(p1.itemID, out int result1) ? result1 : int.MaxValue; // 숫자 변환 실패 시 최대값으로 처리
            int id2 = int.TryParse(p2.itemID, out int result2) ? result2 : int.MaxValue;

            return id1.CompareTo(id2);
        });
        //myItems.Sort((p1, p2) => p1.itemID.CompareTo(p2.itemID));
        InventoryManager.Instance.Save();
        ItemNameInput.text = "";
        ItemNumberInput.text = "";
    }
}

