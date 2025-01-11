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
        // myItems가 비어있는 경우 처리
        if (myItems == null || myItems.Count == 0)
        {
            Debug.Log("아이템 목록이 비어 있습니다. 기본 아이템을 추가합니다.");
            AddSpecificItemToMyItems("바위", 1);  // "바위" 아이템을 1개 추가
        }

        Item curItem = myItems.Find(x => x.itemName == ItemNameInput.text);
        if (curItem != null)
        {
            Debug.Log($"아이템 이름: {curItem.itemName}");

            int parsedPrice;
            if (int.TryParse(curItem.buyPrice, out parsedPrice))
            {
                int total = parsedPrice * int.Parse(ItemNumberInput.text);
                if (GoldManager.Instance.GetGold() >= total)
                {
                    curItem.quantity = (int.Parse(curItem.quantity) + int.Parse(ItemNumberInput.text)).ToString();
                    GoldManager.Instance.SubtractGold(parsedPrice * int.Parse(ItemNumberInput.text)); // 가격 구하기
                }
                else
                {
                    StartCoroutine(GoldManager.Instance.ShowInsufficientGoldUI()); // 가격 부족 UI 띄우기
                }
            }
        }
        else
        {
            Debug.Log("아이템을 찾을 수 없습니다.");
            // 전체에서 얻을 아이템을 찾아 내 아이템에 추가
            Item curAllItem = allItems.Find(x => x.itemName == ItemNameInput.text);
            if (curAllItem != null)
            {
                Debug.Log($"아이템 {curAllItem.itemName}을(를) 추가합니다.");
                myItems.Add(curAllItem);
            }
            else
            {
                Debug.Log("아이템을 전체 목록에서 찾을 수 없습니다.");
            }
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

        InventoryManager.Instance.Save();
        ItemNameInput.text = "";
        ItemNumberInput.text = "";
    }

    // 특정 아이템만 추가하는 메서드
    private void AddSpecificItemToMyItems(string itemName, int quantity)
    {
        if (allItems == null || allItems.Count == 0) return;

        // "바위"라는 아이템을 찾고, 없다면 추가
        Item itemToAdd = allItems.Find(x => x.itemName == itemName);
        if (itemToAdd != null)
        {
            // myItems에 해당 아이템이 없다면 추가
            if (myItems.Find(x => x.itemName == itemName) == null)
            {
                itemToAdd.quantity = quantity.ToString(); // 수량 설정
                myItems.Add(itemToAdd);
                Debug.Log($"아이템 {itemName}이(가) 추가되었습니다.");
            }
            else
            {
                Debug.Log($"아이템 {itemName}은 이미 존재합니다.");
            }
        }
        else
        {
            Debug.Log($"아이템 {itemName}을(를) 전체 목록에서 찾을 수 없습니다.");
        }
    }
}
