using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecoManager : MonoBehaviour
{
    public List<Item> DecoItems = new List<Item>();
    public GameObject[] DecoSlot;

    void Start()
    {
        // InventoryManager의 MyItemList에서 HomeDeco 아이템 필터링
        DecoItems = InventoryManager.Instance.MyItemList.FindAll(item => item.itemType == "HomeDeco");

        //Test();
        HomeList();
    }

    public void HomeList()
    {
        // 개수만큼 슬롯 보이기
        for (int i = 0; i < DecoSlot.Length; i++)
        {
            //Slot[i].SetActive(i < CurItemList.Count);

            if (i < DecoItems.Count)
            {
                // 슬롯에 아이템 이름과 아이콘 설정
                Transform slotTransform = DecoSlot[i].transform;
                slotTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = DecoItems[i].itemName;
                slotTransform.Find("Count").GetComponent<TextMeshProUGUI>().text = DecoItems[i].quantity + "개";
                slotTransform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + DecoItems[i].itemIcon); // ItemIconImage는 슬롯 내의 Image 컴포넌트
                DecoSlot[i].SetActive(true);
            }
            else
            {
                DecoSlot[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test()
    {
        Debug.Log($"HomeDeco 아이템 개수: {DecoItems.Count}");

        // HomeDeco 리스트 출력
        foreach (var item in DecoItems)
        {
            Debug.Log($"아이템 이름: {item.itemName}, 아이템 ID: {item.itemID}");
        }
    }

}
