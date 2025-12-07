using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// DecoManager.cs
/// 집 꾸미기 아이템을 관리하고 설치하는 시스템
/// 
/// - 인벤토리에서 HomeDeco 아이템만 필터링하여 슬롯 UI 생성
/// - 마우스를 따라다니는 장식물 프리팹 스폰
/// - 일정 시간 후 설치 가능하도록 대기 처리
/// - 실제 위치에 아이템 설치 + 파티클 효과
/// - 아이템 수량 감소 및 인벤토리 저장 처리
/// </summary>
public class DecoManager : MonoBehaviour
{
    // 보유 중인 HomeDeco 아이템 목록
    public List<Item> DecoItems = new List<Item>();

    // UI 슬롯 오브젝트 배열
    public GameObject[] DecoSlot;

    public ParticleSystem spawnParticlePrefab;

    // 현재 마우스를 따라다니고 있는 프리팹
    private GameObject spawnedDecoItem;

    // 현재 아이템 배치 중인지 여부>
    private bool isPlacingItem = false;

    // 아이템 설치 가능 여부, 일정 시간 대기 후 true가 됨
    private bool canPlaceItem = false;

    // 땅을 감지하기 위한 LayerMask
    public LayerMask groundLayer;

    void Start()
    {
        UpdateDecoItems();
        HomeList();
    }

    // DecoItems 리스트를 기반으로 UI 슬롯 갱신
    public void HomeList()
    {
        // 수량 0인 아이템 삭제
        DecoItems.RemoveAll(item => int.Parse(item.quantity) <= 0);

        for (int i = 0; i < DecoSlot.Length; i++)
        {
            if (i < DecoItems.Count)
            {
                Transform slot = DecoSlot[i].transform;

                slot.Find("Name").GetComponent<TextMeshProUGUI>().text = DecoItems[i].itemName;
                slot.Find("Count").GetComponent<TextMeshProUGUI>().text = DecoItems[i].quantity + "개";
                slot.Find("Image").GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("Icons/" + DecoItems[i].itemIcon);

                DecoSlot[i].SetActive(true);
            }
            else
            {
                DecoSlot[i].SetActive(false);
            }
        }
    }

    // 인벤토리에서 HomeDeco 타입만 필터링하여 리스트 갱신
    public void UpdateDecoItems()
    {
        DecoItems = InventoryManager.Instance.MyItemList.FindAll(item => item.itemType == "HomeDeco");
    }

    // 거래 완료 시 리스트 및 UI 갱신
    public void OnTradeCompleted()
    {
        UpdateDecoItems();
        HomeList();
    }


    // 선택한 홈데코 아이템을 마우스를 따라다니도록 스폰
    public void SpawnDecoItemFollowMouse(int index)
    {
        if (index < 0 || index >= DecoItems.Count)
        {
            Debug.LogError("잘못된 인덱스입니다.");
            return;
        }

        string prefabName = DecoItems[index].itemIcon.Replace("_i", "");
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Item/DecoItem/" + prefabName);

        if (prefab == null)
        {
            Debug.LogError($"프리팹 {prefabName}을 찾을 수 없습니다.");
            return;
        }

        // 기존 스폰 제거
        if (spawnedDecoItem != null)
            Destroy(spawnedDecoItem);

        // 마우스 위치로 레이 쏘기
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 worldPosition = hit.point;
            worldPosition.z = 0f;

            spawnedDecoItem = Instantiate(prefab, worldPosition, Quaternion.identity);

            isPlacingItem = true;
            canPlaceItem = false;

            StartCoroutine(EnablePlacementAfterDelay(1f));

            // 아이템 1개 감소
            ReduceItemQuantity(index);
        }
    }

    // 딜레이 후 아이템 설치 가능하게 설정하는 코루틴
    IEnumerator EnablePlacementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlaceItem = true;
    }

    void Update()
    {
        // 마우스 따라다니기
        if (spawnedDecoItem != null && isPlacingItem)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                spawnedDecoItem.transform.position = hit.point;
            }
        }

        // 설치 클릭 처리
        if (Input.GetMouseButtonDown(0) && spawnedDecoItem != null && isPlacingItem)
        {
            if (canPlaceItem)
            {
                PlaceDecoItem();
                AudioManager.Instance.PlaySFX(4);
            }
            else
            {
                Debug.LogWarning("아직 설치할 수 없습니다.");
            }
        }
    }

    /// 설치 시 파티클 재생
    void PlaySpawnParticle(Vector3 position)
    {
        if (spawnParticlePrefab == null)
        {
            Debug.LogWarning("파티클 프리팹이 없습니다.");
            return;
        }

        ParticleSystem particle = Instantiate(spawnParticlePrefab, position, Quaternion.identity);
        particle.Play();

        Destroy(particle.gameObject, particle.main.duration);
    }

    // 장식 아이템 설치 처리
    void PlaceDecoItem()
    {
        if (spawnedDecoItem != null)
        {
            PlaySpawnParticle(spawnedDecoItem.transform.position);

            isPlacingItem = false;
            spawnedDecoItem = null;
        }
    }

    // 아이템 1개 감소 처리
    void ReduceItemQuantity(int index)
    {
        if (index < 0 || index >= DecoItems.Count)
        {
            Debug.LogWarning("아이템 인덱스가 유효하지 않습니다.");
            return;
        }

        var item = DecoItems[index];
        int newNumber = int.Parse(item.quantity) - 1;

        if (newNumber <= 0)
        {
            // 수량 0 → 리스트에서 제거
            DecoItems.RemoveAt(index);
            RemoveItemFromMyItems(item.itemName);
        }
        else
        {
            item.quantity = newNumber.ToString();
            UpdateItemInMyItems(item.itemName, item.quantity);
        }

        // UI 갱신 및 저장
        HomeList();
        InventoryManager.Instance.Save();
    }

    // 인벤토리의 MyItemList에서 아이템 삭제
    void RemoveItemFromMyItems(string itemName)
    {
        var item = InventoryManager.Instance.MyItemList.Find(x => x.itemName == itemName);
        if (item != null)
        {
            InventoryManager.Instance.MyItemList.Remove(item);
        }
    }

    // 인벤토리의 MyItemList에서 아이템 수량 업데이트
    void UpdateItemInMyItems(string itemName, string quantity)
    {
        var item = InventoryManager.Instance.MyItemList.Find(x => x.itemName == itemName);
        if (item != null)
        {
            item.quantity = quantity;
        }
    }
}
