using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class Item
{
    public Item(string _itemName, string _itemID, string _itemIcon, string _quantity, string _buyPrice, string _sellPrice, string _itemType, string _description)
    { itemName = _itemName; itemID = _itemID; itemIcon = _itemIcon; quantity = _quantity; buyPrice = _buyPrice; sellPrice = _sellPrice; itemType = _itemType; description = _description; }

    public string itemName, itemID, itemIcon, quantity, buyPrice, sellPrice, itemType, description;
}

public class InventoryManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList, CurItemList; // MyItemList이 Json으로 저장
    public string curType = "Crop"; // 아이템 분류

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
    }

    void Save()
    {
        string jdata = JsonConvert.SerializeObject(AllItemList);
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);
    }

    void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);
    }
}
