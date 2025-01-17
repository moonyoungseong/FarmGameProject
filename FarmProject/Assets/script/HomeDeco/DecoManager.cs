using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoManager : MonoBehaviour
{
    private List<Item> myItems;
    private List<Item> allItems;
    public List<Item> CurDecoList = new List<Item>(); // MyItemList이 Json으로 저장

    // Start is called before the first frame update
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
        Debug.Log("꾸미기 데이터가 로드되었습니다.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
