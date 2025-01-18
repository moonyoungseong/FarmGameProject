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
        // InventoryManager�� MyItemList���� HomeDeco ������ ���͸�
        DecoItems = InventoryManager.Instance.MyItemList.FindAll(item => item.itemType == "HomeDeco");

        //Test();
        HomeList();
    }

    public void HomeList()
    {
        // ������ŭ ���� ���̱�
        for (int i = 0; i < DecoSlot.Length; i++)
        {
            //Slot[i].SetActive(i < CurItemList.Count);

            if (i < DecoItems.Count)
            {
                // ���Կ� ������ �̸��� ������ ����
                Transform slotTransform = DecoSlot[i].transform;
                slotTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = DecoItems[i].itemName;
                slotTransform.Find("Count").GetComponent<TextMeshProUGUI>().text = DecoItems[i].quantity + "��";
                slotTransform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + DecoItems[i].itemIcon); // ItemIconImage�� ���� ���� Image ������Ʈ
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
        Debug.Log($"HomeDeco ������ ����: {DecoItems.Count}");

        // HomeDeco ����Ʈ ���
        foreach (var item in DecoItems)
        {
            Debug.Log($"������ �̸�: {item.itemName}, ������ ID: {item.itemID}");
        }
    }

}
