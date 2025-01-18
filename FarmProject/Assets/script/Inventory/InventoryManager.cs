using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Item
{
    public Item(string _itemName, string _itemID, string _itemIcon, string _quantity, string _buyPrice, string _sellPrice, string _itemType, string _description)
    { itemName = _itemName; itemID = _itemID; itemIcon = _itemIcon; quantity = _quantity; buyPrice = _buyPrice; sellPrice = _sellPrice; itemType = _itemType; description = _description; }

    public string itemName, itemID, itemIcon, quantity, buyPrice, sellPrice, itemType, description;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // 싱글톤 인스턴스

    public TextAsset ItemDatabase;
    public List<Item> AllItemList = new List<Item>();
    public List<Item> MyItemList = new List<Item>();
    public List<Item> CurItemList = new List<Item>(); // MyItemList이 Json으로 저장
    public string curType = "Crop"; // 아이템 분류
    public GameObject[] Slot;
    public GameObject ExplainPanel;  // 슬롯에 올렸을 때 띄울 UI
    IEnumerator PointerCoroutine;

    public event Action OnDataLoaded; // 데이터가 로드된 후 호출될 이벤트

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

    void Start()
    {
        // 전체 아이템 리스트 불러오기
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7]));
        }
        Load();
    }

    public void TabClick(string tabName)
    {
        curType = tabName;
        CurItemList = MyItemList.FindAll(x => x.itemType == tabName);

        // 개수만큼 슬롯 보이기
        for (int i=0; i<Slot.Length; i++)
        {
            //Slot[i].SetActive(i < CurItemList.Count);

            if (i < CurItemList.Count)
            {
                // 슬롯에 아이템 이름과 아이콘 설정
                Transform slotTransform = Slot[i].transform;
                slotTransform.Find("ItemNameText").GetComponent<TextMeshProUGUI>().text = CurItemList[i].itemName;
                slotTransform.Find("Item").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + CurItemList[i].itemIcon); // ItemIconImage는 슬롯 내의 Image 컴포넌트
                Slot[i].SetActive(true);
            }
            else
            {
                Slot[i].SetActive(false);
            }
        }
    }

    public void PointerEnter(int slotNum)
    {
        PointerCoroutine = PointerEnterDelay(slotNum);
        StartCoroutine(PointerCoroutine);

        ExplainPanel.GetComponentInChildren<TextMeshProUGUI>().text = CurItemList[slotNum].itemName;
        ExplainPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "보유 수 : " + CurItemList[slotNum].quantity + "개";
        ExplainPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "판매가격 : " + CurItemList[slotNum].sellPrice + "$";
        ExplainPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "설명 : " + CurItemList[slotNum].description;
    }

    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
    }

    public void PointerExit(int slotNum)
    {
        if (PointerCoroutine != null)
        {
            StopCoroutine(PointerCoroutine);
        }
        ExplainPanel.SetActive(false);
    }

    public Item GetItemByID(string itemID)
{
    // JSON 데이터에서 아이템 정보를 검색
    return MyItemList.Find(item => item.itemID == itemID);
}

    public void Save()
    {
        string jdata = JsonConvert.SerializeObject(MyItemList);
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);

        TabClick(curType);
    }

    public void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
        // 데이터 로딩이 끝났을 때 이벤트 호출
        OnDataLoaded?.Invoke();
    }
}
