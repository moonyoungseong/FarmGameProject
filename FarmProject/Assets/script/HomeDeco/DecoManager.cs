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
        UpdateDecoItems();
        HomeList();
    }

    public void HomeList()
    {
        // ������ŭ ���� ���̱�
        for (int i = 0; i < DecoSlot.Length; i++)
        {
            if (i < DecoItems.Count)
            {
                // ���Կ� ������ �̸��� ������ ����
                Transform slotTransform = DecoSlot[i].transform;
                slotTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = DecoItems[i].itemName;
                slotTransform.Find("Count").GetComponent<TextMeshProUGUI>().text = DecoItems[i].quantity + "��";
                slotTransform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + DecoItems[i].itemIcon);
                DecoSlot[i].SetActive(true);
            }
            else
            {
                DecoSlot[i].SetActive(false);
            }
        }
    }

    public void UpdateDecoItems()
    {
        // InventoryManager�� MyItemList���� HomeDeco ������ ���͸�
        DecoItems = InventoryManager.Instance.MyItemList.FindAll(item => item.itemType == "HomeDeco");
    }

    // �ŷ� �̺�Ʈ �߻� �� ȣ��
    public void OnTradeCompleted()
    {
        UpdateDecoItems(); // DecoItems ����
        HomeList();        // UI ������Ʈ
    }
}
