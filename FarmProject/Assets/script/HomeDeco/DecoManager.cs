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
    private bool isPlacingItem = false;  // 아이템 설치 중 여부

    public LayerMask groundLayer;

    void Start()
    {
        UpdateDecoItems();
        HomeList();
    }

    public void HomeList()
    {
        // 개수만큼 슬롯 보이기
        for (int i = 0; i < DecoSlot.Length; i++)
        {
            if (i < DecoItems.Count)
            {
                // 슬롯에 아이템 이름과 아이콘 설정
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
        // InventoryManager의 MyItemList에서 HomeDeco 아이템 필터링
        DecoItems = InventoryManager.Instance.MyItemList.FindAll(item => item.itemType == "HomeDeco");
    }

    // 거래 이벤트 발생 시 호출
    public void OnTradeCompleted()
    {
        UpdateDecoItems(); // DecoItems 갱신
        HomeList();        // UI 업데이트
    }

    // 마우스 위치를 따라 프리팹 소환
    public void SpawnDecoItemFollowMouse(int index)
    {
        if (index < 0 || index >= DecoItems.Count)
        {
            Debug.LogError("잘못된 인덱스입니다.");
            return;
        }

        // DecoItem의 프리팹 이름 가져오기
        string prefabName = DecoItems[index].itemIcon.Replace("_i", ""); // 필요 시 "_i" 제거
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Item/DecoItem/" + prefabName);

        if (prefab != null)
        {
            // 기존에 소환된 아이템이 있으면 삭제
            if (spawnedDecoItem != null)
            {
                Destroy(spawnedDecoItem);
            }

            // Raycast로 마우스 위치에 대한 땅의 위치를 찾음
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이케스트가 땅에 닿으면 오브젝트 생성
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 worldPosition = hit.point; // Raycast가 맞은 지점으로 위치 조정

                // Z값을 고정시키고, X, Y 값만 업데이트
                worldPosition.z = 0f; // 땅의 Z값 고정

                // 프리팹 소환
                spawnedDecoItem = Instantiate(prefab, worldPosition, Quaternion.identity);
                Debug.Log($"프리팹 {prefabName} 소환 완료!");

                isPlacingItem = true;  // 아이템 설치 중
            }
        }
        else
        {
            Debug.LogError($"프리팹 {prefabName}을(를) 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        // 마우스 포인터를 따라 소환된 프리팹 이동
        if (spawnedDecoItem != null && isPlacingItem)
        {
            // Raycast로 마우스 위치에 대한 땅의 위치를 찾음
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast가 땅에만 반응하도록 레이어 마스크를 사용
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // Raycast가 맞은 지점으로 위치 조정
                Vector3 worldPosition = hit.point;

                // Y값을 레이케스트로 구한 값으로 고정
                worldPosition.y = hit.point.y; // 레이케스트로 구한 y값으로 고정

                // X와 Z값은 마우스 위치에 따라 이동
                worldPosition.x = hit.point.x;
                worldPosition.z = hit.point.z;

                // 오브젝트의 위치를 worldPosition으로 설정
                spawnedDecoItem.transform.position = worldPosition;
            }
        }

        // 마우스 클릭 시 아이템 설치
        if (Input.GetMouseButtonDown(0) && spawnedDecoItem != null && isPlacingItem)
        {
            PlaceDecoItem(); // 아이템 설치
  
            isPlacingItem = false;  // 아이템 설치 후 상태 변경
            spawnedDecoItem = null; // 아이템 삭제
        }
    }

    void PlaySpawnParticle(Vector3 position)
    {
        if (spawnParticlePrefab != null)  // 파티클 프리팹이 null인지 체크
        {
            // 파티클을 특정 위치에 재생
            ParticleSystem particle = Instantiate(spawnParticlePrefab, position, Quaternion.identity);
            particle.Play(); // 파티클 시작

            Debug.Log("파티클이 시작되었습니다: " + particle.isPlaying); // 파티클이 재생되는지 확인

            Destroy(particle.gameObject, particle.main.duration); // 파티클이 끝나면 자동으로 삭제
        }
        else
        {
            Debug.LogWarning("파티클 프리팹이 null입니다. 파티클이 재생되지 않았습니다.");
        }
    }


    // 아이템을 설치하고 새로운 아이템을 준비
    void PlaceDecoItem()
    {
        if (spawnedDecoItem != null)
        {

            if (spawnedDecoItem != null) // spawnedDecoItem이 null이 아니면 파티클 재생
            {
                PlaySpawnParticle(spawnedDecoItem.transform.position); // 파티클 재생
            }
            else
            {
                Debug.LogWarning("spawnedDecoItem이 null이므로 파티클을 재생할 수 없습니다.");
            }

            // 아이템을 설치 (여기서는 그냥 놓는다고 가정)
            Debug.Log("아이템 설치 완료!");

            // 새로운 아이템을 소환할 준비
            isPlacingItem = false;  // 설치 완료

            // 다음 프리팹을 설치할 준비
            spawnedDecoItem = null; // 이전 프리팹을 null로 설정하여 초기화           
        }
    }
}
