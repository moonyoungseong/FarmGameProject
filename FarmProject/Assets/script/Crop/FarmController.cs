using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FarmController : MonoBehaviour
{
    public GameObject player1;               // 플레이어 1
    public GameObject player2;               // 플레이어 2
    public CropFactory cropFactory;          // CropFactory를 참조
    public CropAttributes CornAttributes;    // 옥수수 작물 속성 데이터
    public CropAttributes TomatoAttributes;  // 토마토 작물 속성 데이터
    public CropAttributes RiceAttributes;    // 쌀 작물 속성 데이터

    public LayerMask groundLayer;            // 바닥 레이어 설정
    private Transform player;                 // 플레이어의 Transform
    public float maxPlantDistance = 5f;      // 작물을 심을 수 있는 최대 거리
    public float maxPlantAngle = 30f;        // 플레이어 전방 기준 허용 각도
    public float plantCooldown = 1f;         // 작물을 심을 수 있는 대기시간
    public float minPlantDistance = 1f;      // 다른 작물과의 최소 거리 제한

    private float lastPlantTime = 0f;        // 마지막으로 작물을 심은 시간
    public GameObject NoSeedUI;               // 작물 없을때 나오는 UI
    private List<Vector3> plantedCropPositions = new List<Vector3>();

    // 현재 선택된 작물을 관리할 변수
    private CropAttributes selectedCropAttributes;

    // 기존에 있던 playerMoveScript 변수 사용
    private PlayerMove playerMoveScript;

    void Start()
    {
        // 씬 로딩 시 활성화된 플레이어에 따라 playerMoveScript 할당
        if (player1.activeInHierarchy)
        {
            playerMoveScript = player1.GetComponent<PlayerMove>(); // Player1의 PlayerMove 스크립트 할당
            player = player1.transform;  // 플레이어 트랜스폼 설정
        }
        else if (player2.activeInHierarchy)
        {
            playerMoveScript = player2.GetComponent<PlayerMove>(); // Player2의 PlayerMove 스크립트 할당
            player = player2.transform;  // 플레이어 트랜스폼 설정
        }

        selectedCropAttributes = null; // 기본 상태에서는 아무 것도 심지 않음
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && CanPlant())
        {
            if (selectedCropAttributes == null)
            {
                Debug.Log("선택된 작물이 없습니다.");
                return; // 작물이 선택되지 않은 경우 심기 취소
            }

            // 씨앗이 없으면 심지 않도록 처리
            if (!HasSeed(selectedCropAttributes))
            {
                NoSeedUI.SetActive(true);

                // 2초 후에 UI 비활성화
                StartCoroutine(DisableUIAfterTime(2f));

                Debug.Log("선택한 작물의 씨앗이 없습니다.");
                return;
            }

            Vector3 mousePosition = GetMouseWorldPosition();
            if (IsWithinPlantingRange(mousePosition) && IsFarEnoughFromOtherCrops(mousePosition))
            {
                cropFactory.CreateCrop(selectedCropAttributes, mousePosition);
                plantedCropPositions.Add(mousePosition);
                lastPlantTime = Time.time;

                // 인벤토리에서 씨앗의 수량을 1개씩 줄임
                ReduceSeedCount(selectedCropAttributes);

                StartCoroutine(playerMoveScript.PlantingAnimationCoroutine(10f)); // 애니메이션 실행
            }
        }
    }

    // 씨앗이 있는지 확인하는 함수
    bool HasSeed(CropAttributes cropAttributes)
    {
        Item seedItem = InventoryManager.Instance.MyItemList.Find(item => item.itemName == cropAttributes.SeedName);
        return seedItem != null && int.Parse(seedItem.quantity) > 0;
    }

    private System.Collections.IEnumerator DisableUIAfterTime(float delayTime)
    {
        // 2초 대기
        yield return new WaitForSeconds(delayTime);

        // UI 비활성화
        NoSeedUI.SetActive(false);
    }


    // 씨앗을 심을 때 인벤토리에서 해당 씨앗의 개수를 줄이는 함수
    void ReduceSeedCount(CropAttributes cropAttributes)
    {
        // 인벤토리에서 해당 씨앗 아이템 찾기
        Item seedItem = InventoryManager.Instance.MyItemList.Find(item => item.itemName == cropAttributes.SeedName);

        if (seedItem != null && int.Parse(seedItem.quantity) > 0)
        {
            // 수량 감소
            int newQuantity = int.Parse(seedItem.quantity) - 1;
            seedItem.quantity = newQuantity.ToString();

            // 수량이 0이면 인벤토리에서 제거
            if (newQuantity == 0)
            {
                InventoryManager.Instance.MyItemList.Remove(seedItem);
                Debug.Log($"{cropAttributes.SeedName} 씨앗이 0개가 되어 인벤토리에서 제거되었습니다.");
            }

            // 인벤토리 저장
            InventoryManager.Instance.Save();
        }
        else
        {
            Debug.Log("씨앗이 부족하거나 존재하지 않습니다.");
        }
    }


    public void SelectCorn()
    {
        selectedCropAttributes = CornAttributes;
        Debug.Log("옥수수를 선택했습니다.");
    }

    public void SelectTomato()
    {
        selectedCropAttributes = TomatoAttributes;
        Debug.Log("토마토를 선택했습니다.");
    }

    public void SelectRice()
    {
        selectedCropAttributes = RiceAttributes;
        Debug.Log("쌀을 선택했습니다.");
    }

    public void SelectNone()
    {
        selectedCropAttributes = null;
        Debug.Log("아무 것도 선택하지 않았습니다.");
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    bool IsWithinPlantingRange(Vector3 position)
    {
        float distance = Vector3.Distance(player.position, position);
        Vector3 directionToMouse = (position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToMouse);

        return distance <= maxPlantDistance && angle <= maxPlantAngle;
    }

    bool IsFarEnoughFromOtherCrops(Vector3 position)
    {
        foreach (Vector3 cropPosition in plantedCropPositions)
        {
            if (Vector3.Distance(position, cropPosition) < minPlantDistance)
            {
                Debug.Log("다른 작물이 너무 가까워서 심을 수 없습니다.");
                return false;
            }
        }
        return true;
    }

    bool CanPlant()
    {
        return Time.time >= lastPlantTime + plantCooldown;
    }
}


