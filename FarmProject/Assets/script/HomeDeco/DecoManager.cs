using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecoManager : MonoBehaviour
{
    public List<Item> DecoItems = new List<Item>();
    public GameObject[] DecoSlot;
    public ParticleSystem spawnParticlePrefab; // 파티클 시스템 프리팹

    private GameObject spawnedDecoItem; // 소환된 프리팹을 저장할 변수
    private bool isPlacingItem = false; // 아이템 설치 중 여부
    private bool canPlaceItem = false;  // 아이템 설치 가능 여부

    public LayerMask groundLayer;

    void Start()
    {
        UpdateDecoItems();
        HomeList();
    }

    public void HomeList()
    {
        // 수량이 0인 아이템을 리스트에서 제거
        DecoItems.RemoveAll(item => int.Parse(item.quantity) <= 0);

        // 개수만큼 슬롯 보이기
        for (int i = 0; i < DecoSlot.Length; i++)
        {
            if (i < DecoItems.Count)
            {
                Transform slotTransform = DecoSlot[i].transform;
                slotTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = DecoItems[i].itemName;
                slotTransform.Find("Count").GetComponent<TextMeshProUGUI>().text = DecoItems[i].quantity + "개";
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
        DecoItems = InventoryManager.Instance.MyItemList.FindAll(item => item.itemType == "HomeDeco");
    }

    public void OnTradeCompleted()
    {
        UpdateDecoItems();
        HomeList();
        // myItems에서 수량 업데이트
        //UpdateItemInMyItems(item.itemName, item.quantity);
    }

    public void SpawnDecoItemFollowMouse(int index)
    {
        if (index < 0 || index >= DecoItems.Count)
        {
            Debug.LogError("잘못된 인덱스입니다.");
            return;
        }

        string prefabName = DecoItems[index].itemIcon.Replace("_i", "");
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Item/DecoItem/" + prefabName);

        if (prefab != null)
        {
            if (spawnedDecoItem != null)
            {
                Destroy(spawnedDecoItem);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 worldPosition = hit.point;
                worldPosition.z = 0f;

                spawnedDecoItem = Instantiate(prefab, worldPosition, Quaternion.identity);
                Debug.Log($"프리팹 {prefabName} 소환 완료!");

                isPlacingItem = true;
                canPlaceItem = false; // 처음에는 설치 불가능
                StartCoroutine(EnablePlacementAfterDelay(1f)); // 2초 뒤 설치 가능

                // 설치한 아이템의 개수를 줄임
                ReduceItemQuantity(index);
            }
        }
        else
        {
            Debug.LogError($"프리팹 {prefabName}을(를) 찾을 수 없습니다.");
        }
    }

    IEnumerator EnablePlacementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlaceItem = true;
        Debug.Log("아이템 설치가 가능합니다!");
    }

    void Update()
    {
        if (spawnedDecoItem != null && isPlacingItem)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 worldPosition = hit.point;
                worldPosition.y = hit.point.y;
                worldPosition.x = hit.point.x;
                worldPosition.z = hit.point.z;

                spawnedDecoItem.transform.position = worldPosition;
            }
        }

        if (Input.GetMouseButtonDown(0) && spawnedDecoItem != null && isPlacingItem)
        {
            if (canPlaceItem) // 설치 가능 여부 확인
            {
                PlaceDecoItem();
                AudioManager.Instance.PlaySFX(4);// 생성시 효과음
            }
            else
            {
                Debug.LogWarning("아직 설치할 수 없습니다. 기다려주세요!");
            }
        }
    }

    void PlaySpawnParticle(Vector3 position)
    {
        if (spawnParticlePrefab != null)
        {
            ParticleSystem particle = Instantiate(spawnParticlePrefab, position, Quaternion.identity);
            particle.Play();
            Destroy(particle.gameObject, particle.main.duration);
        }
        else
        {
            Debug.LogWarning("파티클 프리팹이 null입니다. 파티클이 재생되지 않았습니다.");
        }
    }

    void PlaceDecoItem()
    {
        if (spawnedDecoItem != null)
        {
            PlaySpawnParticle(spawnedDecoItem.transform.position);
            Debug.Log("아이템 설치 완료!");

            // 설치 상태 초기화
            isPlacingItem = false;
            spawnedDecoItem = null;
        }
    }

    void ReduceItemQuantity(int index)
    {
        if (index >= 0 && index < DecoItems.Count)
        {
            var item = DecoItems[index];

            // 수량 감소
            int curNumber = int.Parse(item.quantity) - 1;
            Debug.Log($"아이템 {item.itemName} 수량 감소 전: {item.quantity}, 수량 감소 후: {curNumber}");

            if (curNumber <= 0)
            {
                // 수량이 0 이하이면 리스트에서 제거
                DecoItems.RemoveAt(index);
                Debug.Log($"아이템 {item.itemName} 수량이 0 이하로 감소, 삭제됨.");

                // myItems에서도 해당 아이템의 수량이 0이면 삭제
                RemoveItemFromMyItems(item.itemName);
            }
            else
            {
                // 수량 업데이트
                item.quantity = curNumber.ToString();
                Debug.Log($"아이템 {item.itemName} 수량 업데이트됨: {item.quantity}");

                // myItems에서 수량 업데이트
                UpdateItemInMyItems(item.itemName, item.quantity);
            }

            // UI 업데이트
            HomeList();

            // 인벤토리 저장
            InventoryManager.Instance.Save();
        }
        else
        {
            Debug.LogWarning("아이템 인덱스가 유효하지 않습니다.");
        }
    }

    // myItems에서 해당 아이템 삭제
    void RemoveItemFromMyItems(string itemName)
    {
        var itemToRemove = InventoryManager.Instance.MyItemList.Find(x => x.itemName == itemName);
        if (itemToRemove != null)
        {
            InventoryManager.Instance.MyItemList.Remove(itemToRemove);
            Debug.Log($"아이템 {itemName}이(가) myItems에서 삭제되었습니다.");
        }
    }

    // myItems에서 해당 아이템의 수량 업데이트
    void UpdateItemInMyItems(string itemName, string quantity)
    {
        var itemToUpdate = InventoryManager.Instance.MyItemList.Find(x => x.itemName == itemName);
        if (itemToUpdate != null)
        {
            itemToUpdate.quantity = quantity;
            Debug.Log($"아이템 {itemName}의 수량이 myItems에서 업데이트되었습니다.");
        }
    }

}
