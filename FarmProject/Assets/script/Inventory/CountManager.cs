using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// CountManager.cs
/// 인벤토리 거래, 아이템 추가/삭제, 수량 변경, 보상 지급 등
/// '아이템 수량 관리 전반'을 담당하는 매니저.
/// 
/// - 싱글톤으로 운영됨 (씬 전환 유지)
/// - InventoryManager의 아이템 데이터 로드 완료 이벤트(OnDataLoaded)와 연동
/// - 상점 구매/판매, 퀘스트 보상, 퀘스트 재료 소모 기능 포함
/// </summary>
public class CountManager : MonoBehaviour
{
    public static CountManager Instance { get; private set; }

    // 거래 UI에서 아이템 이름 입력
    public TMP_InputField ItemNameInput;

    // 거래 UI에서 아이템 개수 입력
    public TMP_InputField ItemNumberInput;

    // 내 인벤토리 아이템 리스트
    private List<Item> myItems;

    // 전체 아이템(데이터베이스 역할) 리스트
    private List<Item> allItems;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 시작 시 InventoryManager가 데이터 로딩을 완료하면 호출되는 이벤트 등록
    void Start()
    {
        InventoryManager.Instance.OnDataLoaded += OnInventoryDataLoaded;
    }


    /// <summary>
    /// InventoryManager 데이터 로딩 완료 시 호출.
    /// myItems와 allItems 가져오는 시점은 여기서 처리.
    /// </summary>
    void OnInventoryDataLoaded()
    {
        myItems = InventoryManager.Instance.MyItemList;
        allItems = InventoryManager.Instance.AllItemList;
    }


    /// <summary>
    /// 상점 구매 버튼 클릭 시 호출.
    /// 입력된 아이템 이름을 검색하여:
    /// - 내 인벤에 있으면 수량 증가
    /// - 없으면 전체 목록(allItems)에서 찾아 추가
    /// - 골드 부족 시 UI 출력
    /// </summary>
    public void GetItemClick()
    {
        if (myItems == null || myItems.Count == 0)
        {
            AddSpecificItemToMyItems("바위", 1);
        }

        Item curItem = myItems.Find(x => x.itemName == ItemNameInput.text);

        if (curItem != null)
        {
            int parsedPrice;
            if (int.TryParse(curItem.buyPrice, out parsedPrice))
            {
                int total = parsedPrice * int.Parse(ItemNumberInput.text);

                if (GoldManager.Instance.GetGold() >= total)
                {
                    int currentQuantity = int.Parse(curItem.quantity);
                    currentQuantity += int.Parse(ItemNumberInput.text);
                    curItem.quantity = currentQuantity.ToString();

                    GoldManager.Instance.SubtractGold(total);
                }
                else
                {
                    StartCoroutine(GoldManager.Instance.ShowInsufficientGoldUI());
                }
            }
        }
        else
        {
            Item curAllItem = allItems.Find(x => x.itemName == ItemNameInput.text);

            if (curAllItem != null)
            {
                myItems.Add(curAllItem);
            }
        }
        toEnd();
    }

    /// <summary>
    /// 상점 판매 버튼 클릭 시 호출.
    /// 아이템 수량 감소 또는 0 이하일 경우 삭제.
    /// 골드 지급까지 처리.
    /// </summary>
    public void ReMoveClick()
    {
        if (myItems == null || myItems.Count == 0)
        {
            return;
        }

        Item curItem = myItems.Find(x => x.itemName == ItemNameInput.text);

        if (curItem != null)
        {
            int curNumber = int.Parse(curItem.quantity) - int.Parse(ItemNumberInput.text);

            if (curNumber <= 0)
                myItems.Remove(curItem);
            else
                curItem.quantity = curNumber.ToString();

            int parsedPrice;
            if (int.TryParse(curItem.sellPrice, out parsedPrice))
                GoldManager.Instance.AddGold(parsedPrice * int.Parse(ItemNumberInput.text));
        }

        toEnd();
    }

    // 거래 후 인벤토리 정렬, 저장, UI 초기화.
    public void toEnd()
    {
        myItems.Sort((p1, p2) =>
        {
            int id1 = int.TryParse(p1.itemID, out int result1) ? result1 : int.MaxValue;
            int id2 = int.TryParse(p2.itemID, out int result2) ? result2 : int.MaxValue;

            return id1.CompareTo(id2);
        });

        InventoryManager.Instance.Save();
        ItemNameInput.text = "";
        ItemNumberInput.text = "";
    }

    // 특정 아이템을 myItems에 강제로 추가하는 기능 (주로 테스트, 기본 아이템 용도)
    private void AddSpecificItemToMyItems(string itemName, int quantity)
    {
        if (allItems == null || allItems.Count == 0) return;

        Item itemToAdd = allItems.Find(x => x.itemName == itemName);

        if (itemToAdd != null)
        {
            if (myItems.Find(x => x.itemName == itemName) == null)
            {
                itemToAdd.quantity = quantity.ToString();
                myItems.Add(itemToAdd);
            }
        }
    }

    /// <summary>
    /// 퀘스트 보상 아이템 지급.
    /// 존재할 경우 수량 증가, 없을 경우 신규 생성하여 myItems에 추가.
    /// </summary>
    public void GiveRewardItem(string itemName, int quantity)
    {
        if (myItems == null || allItems == null)
        {
            Debug.LogWarning("아이템 목록이 초기화되지 않았습니다.");
            return;
        }

        Item curItem = myItems.Find(x => x.itemName == itemName);

        if (curItem != null)
        {
            int currentQuantity = curItem.quantityInt;
            currentQuantity += quantity;
            curItem.quantity = currentQuantity.ToString();
        }
        else
        {
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
            }
            else
            {
                Debug.LogWarning($"보상 아이템 {itemName}을 전체 목록에서 찾을 수 없습니다.");
            }
        }

        toEnd();
    }


    /// <summary>
    /// 수집형 퀘스트 재료 소모 기능.
    /// itemID 기준으로 검색하여 수량 감소 또는 삭제.
    /// </summary>
    public void RemoveItemByID(string itemID, int quantityToConsume)
    {
        if (myItems == null || myItems.Count == 0)
        {
            Debug.LogWarning("아이템 목록이 비어 있습니다.");
            return;
        }

        Item item = myItems.Find(x => x.itemID == itemID);

        if (item != null)
        {
            int newQuantity = item.quantityInt - quantityToConsume;

            if (newQuantity <= 0)
            {
                myItems.Remove(item);
            }
            else
            {
                item.quantity = newQuantity.ToString();
            }

            InventoryManager.Instance.Save();
        }
        else
        {
            Debug.LogWarning($"아이템 ID {itemID}을 찾을 수 없습니다.");
        }
    }
}
