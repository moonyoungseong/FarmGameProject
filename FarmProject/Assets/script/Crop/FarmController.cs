using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// FarmController.cs
/// 플레이어 입력(마우스 좌클릭)을 통해 씬에 작물을 심는 기능을 담당하는 컨트롤러.
/// 
/// - 플레이어(활성화된 player1 또는 player2)에 따라 심기 가능한 범위 검사 수행
/// - 선택된 작물(CropAttributes)에 대해 씨앗 보유 확인 후 CropFactory로 작물 생성
/// - 심기 쿨타임, 최소 거리 제한, 씨앗 소모(인벤토리 업데이트) 처리
/// - 씨앗이 없을 때 안내 UI(NoSeedUI) 표시
/// </summary>
public class FarmController : MonoBehaviour
{
    // 배치된 남자 캐릭터
    public GameObject player1;

    // 배치된 여자 캐릭터
    public GameObject player2;

    public CropFactory cropFactory;

    // 옥수수 작물 속성 데이터
    public CropAttributes CornAttributes;

    // 토마토 작물 속성 데이터
    public CropAttributes TomatoAttributes;

    // 쌀 작물 속성 데이터
    public CropAttributes RiceAttributes;

    // 땅(바닥) 레이어 마스크
    public LayerMask groundLayer;

    private Transform player;

    // 작물을 심을 수 있는 최대 거리(플레이어 기준)
    public float maxPlantDistance = 5f;

    // 플레이어 전방을 기준으로 허용되는 최대 각도
    public float maxPlantAngle = 30f;

    // 작물 심기 쿨타임(초)
    public float plantCooldown = 1f;

    // 다른 작물과의 최소 거리 제한
    public float minPlantDistance = 1f;

    // 마지막으로 작물을 심은 시간
    private float lastPlantTime = 0f;

    // 씨앗이 없을 때 표시할 UI
    public GameObject NoSeedUI;

    // 이미 심어진 작물들의 월드 위치 목록(중복 심기 방지용)
    private List<Vector3> plantedCropPositions = new List<Vector3>();

    // 현재 선택된 작물 속성
    private CropAttributes selectedCropAttributes;

    private PlayerMove playerMoveScript;

   
    // 씬 로드 시 활성화된 플레이어를 찾아 player, playerMoveScript를 초기화한다.
    // 기본적으로 selectedCropAttributes는 null(미선택)으로 설정된다.
    private void Start()
    {
        // 씬에서 활성화된 플레이어를 기준으로 PlayerMove와 Transform을 할당
        if (player1 != null && player1.activeInHierarchy)
        {
            playerMoveScript = player1.GetComponent<PlayerMove>();
            player = player1.transform;
        }
        else if (player2 != null && player2.activeInHierarchy)
        {
            playerMoveScript = player2.GetComponent<PlayerMove>();
            player = player2.transform;
        }

        selectedCropAttributes = null;
    }


    // 마우스 좌클릭 입력과 UI 포인터 상태를 검사하여 심기 동작을 수행한다.
    // 선택된 작물, 씨앗 보유, 위치 유효성(거리/각도/다른 작물과의 거리), 쿨타임을 검사.
    private void Update()
    {
        // 왼쪽 마우스 버튼 클릭, UI 위가 아니어야 하며, 심기 가능(CanPlant)
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && CanPlant())
        {
            Vector3 mousePosition = GetMouseWorldPosition();

            // 선택된 작물이 없을 때 처리
            if (selectedCropAttributes == null)
            {
                return;
            }

            // 인벤토리에 씨앗이 있는지 확인
            if (!HasSeed(selectedCropAttributes))
            {
                if (NoSeedUI != null)
                {
                    NoSeedUI.SetActive(true);
                    StartCoroutine(DisableUIAfterTime(2f));
                }
                return;
            }

            // 심기 범위 및 다른 작물과의 거리 검사
            if (IsWithinPlantingRange(mousePosition) && IsFarEnoughFromOtherCrops(mousePosition))
            {
                // 실제 Crop 객체 생성
                if (cropFactory != null)
                {
                    cropFactory.CreateCrop(selectedCropAttributes, mousePosition);
                    plantedCropPositions.Add(mousePosition);
                    lastPlantTime = Time.time;
                    ReduceSeedCount(selectedCropAttributes);

                    // 심기 애니메이션(코루틴)이 있는 경우 호출
                    if (playerMoveScript != null)
                    {
                        // 10f는 애니메이션 지속 시간/파라미터
                        StartCoroutine(playerMoveScript.PlantingAnimationCoroutine(10f));
                    }
                }
                else
                {
                    Debug.LogWarning("CropFactory가 할당되어 있지 않습니다.");
                }
            }
        }
    }



    // 주어진 CropAttributes의 SeedName을 인벤토리에서 찾아 수량 확인.
    bool HasSeed(CropAttributes cropAttributes)
    {
        if (cropAttributes == null) return false;

        Item seedItem = InventoryManager.Instance.MyItemList.Find(item => item.itemName == cropAttributes.SeedName);
        if (seedItem == null) return false;

        // Item.quantity가 string으로 저장되어 있으므로 파싱해서 비교
        if (int.TryParse(seedItem.quantity, out int qty))
        {
            return qty > 0;
        }

        Debug.LogWarning($"Seed quantity parsing failed for {cropAttributes.SeedName}.");
        return false;
    }

    // 지정된 시간(delayTime) 후 NoSeedUI를 비활성화한다.
    private System.Collections.IEnumerator DisableUIAfterTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (NoSeedUI != null)
            NoSeedUI.SetActive(false);
    }

    // 인벤토리에서 해당 작물의 씨앗 수량을 1 감소, 수량이 0이면 인벤토리에서 제거 후 저장한다.
    void ReduceSeedCount(CropAttributes cropAttributes)
    {
        if (cropAttributes == null) return;

        Item seedItem = InventoryManager.Instance.MyItemList.Find(item => item.itemName == cropAttributes.SeedName);

        if (seedItem != null && int.TryParse(seedItem.quantity, out int qty) && qty > 0)
        {
            int newQuantity = qty - 1;
            seedItem.quantity = newQuantity.ToString();

            if (newQuantity == 0)
            {
                InventoryManager.Instance.MyItemList.Remove(seedItem);
            }

            // 변경사항 저장
            InventoryManager.Instance.Save();
        }
    }

    
    // 옥수수 선택 상태로 설정
    public void SelectCorn()
    {
        selectedCropAttributes = CornAttributes;
    }

    // 토마토 선택 상태로 설정
    public void SelectTomato()
    {
        selectedCropAttributes = TomatoAttributes;
    }

    // 쌀 선택 상태로 설정
    public void SelectRice()
    {
        selectedCropAttributes = RiceAttributes;
    }

    // 선택된 작물을 해제
    public void SelectNone()
    {
        selectedCropAttributes = null;
    }

    /// <summary>
    /// 메인 카메라 기준으로 마우스 위치를 Raycast하여 월드 좌표를 반환한다.
    /// groundLayer에 히트하지 못하면 Vector3.zero 반환.
    /// </summary>
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    // 플레이어와의 거리와 전방 기준 각도를 검사하여 주어진 위치가 심기 가능한 범위인지 판단
    bool IsWithinPlantingRange(Vector3 position)
    {
        if (player == null) return false;

        float distance = Vector3.Distance(player.position, position);
        Vector3 directionToMouse = (position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToMouse);

        return distance <= maxPlantDistance && angle <= maxPlantAngle;
    }

    // 이미 심어진 작물들과의 최소 거리를 검사하여 근접 여부를 판단한다.
    bool IsFarEnoughFromOtherCrops(Vector3 position)
    {
        foreach (Vector3 cropPosition in plantedCropPositions)
        {
            if (Vector3.Distance(position, cropPosition) < minPlantDistance)
            {
                return false;
            }
        }
        return true;
    }
   
    // plantCooldown을 기반으로 현재 심기 동작이 가능한지 여부를 반환
    bool CanPlant()
    {
        return Time.time >= lastPlantTime + plantCooldown;
    }
}
