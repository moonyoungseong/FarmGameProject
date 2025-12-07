/// <summary>
/// 유튜브 : 고라니TV - 게임개발 채널
/// 출처: https://www.youtube.com/watch?v=GNSD1-y6SeM
/// 코드 부분 사용
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Item 클래스
/// - 아이템 속성 정의 (이름, ID, 아이콘, 수량, 가격, 타입, 설명)
/// - 문자열 quantity를 int로 변환 가능
/// </summary>
[System.Serializable]
public class Item
{
    public Item(string _itemName, string _itemID, string _itemIcon, string _quantity, string _buyPrice, string _sellPrice, string _itemType, string _description)
    {
        itemName = _itemName;
        itemID = _itemID;
        itemIcon = _itemIcon;
        quantity = _quantity;
        buyPrice = _buyPrice;
        sellPrice = _sellPrice;
        itemType = _itemType;
        description = _description;
    }

    public string itemName, itemID, itemIcon, quantity, buyPrice, sellPrice, itemType, description;

    public int quantityInt
    {
        get
        {
            if (int.TryParse(quantity, out int result))
                return result;
            return 0;
        }
    }
}

/// <summary>
/// InventoryManager.cs
/// 인벤토리 시스템을 총괄하는 매니저
/// 
/// - 전체 아이템(AllItemList)과 내 아이템(MyItemList) 관리
/// - JSON 저장/불러오기 지원
/// - 슬롯 UI 업데이트 및 설명 패널 표시
/// - 아이템 분류(TabClick) 기능 지원
/// - 데이터 로드 완료 이벤트(OnDataLoaded) 제공
/// - 싱글톤으로 운영되어 씬 전환 시 유지됨
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // 아이템 데이터베이스 텍스트
    public TextAsset ItemDatabase;

    // 모든 아이템 리스트
    public List<Item> AllItemList = new List<Item>();

    // 플레이어 소유 아이템 리스트
    public List<Item> MyItemList = new List<Item>();

    // 현재 표시할 아이템 리스트 (탭별)
    public List<Item> CurItemList = new List<Item>();

    // 현재 선택된 아이템 타입 (탭)
    public string curType = "Crop";

    // 슬롯 UI 오브젝트 배열
    public GameObject[] Slot;

    // 아이템 설명 패널
    public GameObject ExplainPanel;

    // 슬롯 포인터 지연 코루틴
    IEnumerator PointerCoroutine;

    // 데이터 로드 완료 시 호출되는 이벤트
    public event Action OnDataLoaded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 아이템 데이터 로드 및 초기 설정
    void Start()
    {
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7]));
        }
        Load();
    }

    /// <summary>
    /// 아이템 탭 클릭 시 처리
    /// - 해당 타입 아이템만 CurItemList에 저장하고 슬롯 UI 갱신
    /// </summary>
    /// <param name="tabName">선택한 탭 이름</param>
    public void TabClick(string tabName)
    {
        curType = tabName;
        CurItemList = MyItemList.FindAll(x => x.itemType == tabName);

        for (int i = 0; i < Slot.Length; i++)
        {
            if (i < CurItemList.Count)
            {
                Transform slotTransform = Slot[i].transform;
                slotTransform.Find("ItemNameText").GetComponent<TextMeshProUGUI>().text = CurItemList[i].itemName;
                slotTransform.Find("Item").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + CurItemList[i].itemIcon);
                Slot[i].SetActive(true);
            }
            else
            {
                Slot[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 슬롯에 마우스 포인터 진입 시 호출
    /// - 설명 패널 UI 활성화
    /// </summary>
    /// <param name="slotNum">슬롯 번호</param>
    public void PointerEnter(int slotNum)
    {
        PointerCoroutine = PointerEnterDelay(slotNum);
        StartCoroutine(PointerCoroutine);

        ExplainPanel.GetComponentInChildren<TextMeshProUGUI>().text = CurItemList[slotNum].itemName;
        ExplainPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "보유 수 : " + CurItemList[slotNum].quantity + "개";
        ExplainPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "판매가격 : " + CurItemList[slotNum].sellPrice + "$";
        ExplainPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "설명 : " + CurItemList[slotNum].description;
    }

    // 슬롯 포인터 진입 지연 후 UI 활성화 코루틴
    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
    }

    // 슬롯에서 마우스 포인터 벗어날 때 UI 비활성화
    public void PointerExit(int slotNum)
    {
        if (PointerCoroutine != null)
        {
            StopCoroutine(PointerCoroutine);
        }
        ExplainPanel.SetActive(false);
    }

    // 아이템 ID로 MyItemList에서 검색
    public Item GetItemByID(string itemID)
    {
        return MyItemList.Find(item => item.itemID == itemID);
    }

    // 아이템 이름으로 MyItemList에서 검색 (대소문자 무시)
    public Item GetItemByName(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return null;
        return MyItemList.Find(item => item.itemName != null &&
                                      item.itemName.Trim().ToLower() == itemName.Trim().ToLower());
    }

    // 현재 MyItemList를 JSON으로 저장
    public void Save()
    {
        string jdata = JsonConvert.SerializeObject(MyItemList);
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);
        TabClick(curType);
    }

    // JSON에서 MyItemList 불러오기
    public void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);
        TabClick(curType);
        OnDataLoaded?.Invoke();
    }
}
