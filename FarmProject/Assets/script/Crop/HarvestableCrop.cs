using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HarvestableCrop.cs
/// 플레이어가 가까이 접근하면 수확 버튼이 나타나고,
/// 버튼을 누르면 수확 애니메이션 실행 + 인벤토리에 아이템 추가 + 작물 파괴 처리.
///
/// - Player1 또는 Player2 중 활성 플레이어를 자동으로 감지
/// - interactDistance 범위 내에서만 버튼 활성화
/// - 인벤토리(MyItemList)에 이미 있으면 수량 증가, 없으면 새 아이템 추가
/// </summary>
public class HarvestableCrop : MonoBehaviour
{
    public CropAttributes cropAttributes; 
    public Button harvestButton;          // 수확 버튼 UI
    public Transform player1;             // 남자 플레이어 Transform
    public Transform player2;             // 여자 플레이어 Transform
    private Transform player;             

    public float interactDistance = 2f;   // 플레이어와 작물 거리 조건

    private PlayerMove playerMoveScript;  // Picking 애니메이션 코루틴 실행용

    // 활성 플레이어를 찾고 PlayerMove 스크립트 획득, 수확 버튼 클릭 이벤트 등록
    private void Start()
    {
        // Player 오브젝트 자동 할당
        if (player1 == null)
        {
            player1 = GameObject.Find("Player").transform;
        }
        else if (player2 == null)
        {
            player2 = GameObject.Find("Player_W").transform;
        }

        // 어떤 플레이어가 활성화되어 있는지 확인 후 스크립트 할당
        if (player1.gameObject.activeInHierarchy)
        {
            playerMoveScript = player1.GetComponent<PlayerMove>();
            player = player1.transform;
        }
        else if (player2.gameObject.activeInHierarchy)
        {
            playerMoveScript = player2.GetComponent<PlayerMove>();
            player = player2.transform;
        }

        // 버튼 설정
        if (harvestButton != null)
        {
            harvestButton.onClick.AddListener(HarvestCrop);
            harvestButton.gameObject.SetActive(false); // 처음에는 숨김
        }
        else
        {
            Debug.LogError("수확 버튼이 설정되지 않았습니다.");
        }
    }

    // 매 프레임마다 플레이어와 거리 체크, 일정 거리 이내면 버튼 활성화, 벗어나면 비활성화.
    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);

            // 범위 안 → 버튼 On
            if (distanceToPlayer <= interactDistance)
            {
                if (!harvestButton.gameObject.activeSelf)
                    harvestButton.gameObject.SetActive(true);
            }
            // 범위 밖 → 버튼 Off
            else
            {
                if (harvestButton.gameObject.activeSelf)
                    harvestButton.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("플레이어 Transform이 설정되지 않았습니다.");
        }
    }

    /// <summary>
    /// 수확 버튼을 누르면 호출됨:
    /// - 수확 로그 출력
    /// - 수확 애니메이션 실행
    /// - 인벤토리에 작물 추가
    /// - 6.5초 후 오브젝트 삭제
    /// </summary>
    public void HarvestCrop()
    {
        if (cropAttributes == null)
        {
            Debug.LogError("작물 속성 데이터가 없습니다.");
            return;
        }

        // 수확 애니메이션 실행 (6초)
        StartCoroutine(playerMoveScript.PickingAnimationCoroutine(6f));

        // 인벤토리 추가 처리
        AddCropToInventory(cropAttributes);

        // 일정 시간 뒤 객체 삭제
        Invoke("DestroyCrop", 6.5f);
    }

    // 인벤토리에 수확된 작물을 추가하는 메서드
    private void AddCropToInventory(CropAttributes cropAttributes)
    {
        // AllItemList에서 해당 작물 아이템을 찾음
        Item cropItem = InventoryManager.Instance.AllItemList.Find(item => item.itemID == cropAttributes.id);

        if (cropItem != null)
        {
            // MyItemList에서 해당 아이템이 이미 있는지 체크
            Item inventoryItem = InventoryManager.Instance.MyItemList.Find(item => item.itemID == cropAttributes.id);

            if (inventoryItem != null)
            {
                // 기존 항목 존재 → 수량 증가
                if (int.TryParse(inventoryItem.quantity, out int currentQuantity))
                {
                    inventoryItem.quantity = (currentQuantity + 1).ToString();
                }
                else
                {
                    Debug.LogError($"'{inventoryItem.itemName}'의 수량 데이터가 잘못되었습니다.");
                }
            }
            else
            {
                // 인벤토리에 없으면 새로 추가
                Item newItem = new Item(
                    cropItem.itemName,
                    cropItem.itemID,
                    cropItem.itemIcon,
                    cropItem.quantity,
                    cropItem.buyPrice,
                    cropItem.sellPrice,
                    cropItem.itemType,
                    cropItem.description
                );

                InventoryManager.Instance.MyItemList.Add(newItem);
            }
        }
        else
        {
            Debug.LogError($"AllItemList에서 '{cropAttributes.id}' 작물을 찾을 수 없습니다.");
            return;
        }

        // 인벤토리 저장 및 UI 갱신
        InventoryManager.Instance.Save();
    }


    // 수확 애니메이션 후 작물 오브젝트 삭제
    private void DestroyCrop()
    {
        Destroy(gameObject);
    }
}
