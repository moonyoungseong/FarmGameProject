using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoManager : MonoBehaviour
{
    private List<Item> myItems;
    private List<Item> allItems;
    public List<Item> CurDecoList = new List<Item>(); // MyItemList�� Json���� ����

    // Start is called before the first frame update
    void Start()
    {
        // InventoryManager�� ������ �ε��� ��ģ �� �̺�Ʈ�� ���� �˸� �ޱ�
        InventoryManager.Instance.OnDataLoaded += OnInventoryDataLoaded;
    }

    void OnInventoryDataLoaded()
    {
        // MyItemList ��������
        myItems = InventoryManager.Instance.MyItemList;
        allItems = InventoryManager.Instance.AllItemList;

        // ������ �ε� �Ϸ� �� �۾�
        Debug.Log("�ٹ̱� �����Ͱ� �ε�Ǿ����ϴ�.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
