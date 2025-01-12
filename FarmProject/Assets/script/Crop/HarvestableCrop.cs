using UnityEngine;
using UnityEngine.UI;

public class HarvestableCrop : MonoBehaviour
{
    public CropAttributes cropAttributes; // 작물 속성 데이터
    public Button harvestButton;          // 수확 버튼
    public Transform player1;              // 남 플레이어 Transform
    public Transform player2;               // 여 플레이어 Transform
    private Transform player;                 // 플레이어의 Transform
    public float interactDistance = 2f;   // 작물과 상호작용 가능한 거리

    // 기존에 있던 playerMoveScript 변수 사용
    private PlayerMove playerMoveScript;

    private void Start()
    {
        if (player1 == null)
        {
            player1 = GameObject.Find("Player").transform;
        }
        else if (player2 == null) player2 = GameObject.Find("Player_W").transform;

        // 씬 로딩 시 활성화된 플레이어에 따라 playerMoveScript 할당
        if (player1.gameObject.activeInHierarchy)
        {
            playerMoveScript = player1.GetComponent<PlayerMove>(); // Player1의 PlayerMove 스크립트 할당
            player = player1.transform;  // 플레이어 트랜스폼 설정
        }
        else if (player2.gameObject.activeInHierarchy)
        {
            playerMoveScript = player2.GetComponent<PlayerMove>(); // Player2의 PlayerMove 스크립트 할당
            player = player2.transform;  // 플레이어 트랜스폼 설정
        }

        // 버튼 클릭 이벤트 연결
        if (harvestButton != null)
        {
            harvestButton.onClick.AddListener(HarvestCrop);
            harvestButton.gameObject.SetActive(false); // 처음엔 버튼 비활성화
        }
        else
        {
            Debug.LogError("수확 버튼이 설정되지 않았습니다.");
        }
    }

    private void Update()
    {
        // 플레이어와 작물 간 거리 계산
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);

            // 상호작용 거리 이내면 버튼 활성화, 아니면 비활성화
            if (distanceToPlayer <= interactDistance)
            {
                if (!harvestButton.gameObject.activeSelf)
                {
                    harvestButton.gameObject.SetActive(true);
                }
            }
            else
            {
                if (harvestButton.gameObject.activeSelf)
                {
                    harvestButton.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("플레이어 Transform이 설정되지 않았습니다.");
        }
    }

    public void HarvestCrop()
    {
        if (cropAttributes == null)
        {
            Debug.LogError("작물 속성 데이터가 없습니다.");
            return;
        }

        Debug.Log($"{cropAttributes.name}을(를) 수확했습니다!");

        //StartCoroutine(playerMoveScript.PickingAnimationCoroutine(5f)); // 애니메이션 실행

        // 수확 처리 로직
        AddCropToInventory(cropAttributes);

        // 수확 후 오브젝트 제거
        Destroy(gameObject);
    }

    private void AddCropToInventory(CropAttributes cropAttributes)
    {
        // AllItemList에서 해당 작물을 찾음
        Item cropItem = InventoryManager.Instance.AllItemList.Find(item => item.itemID == cropAttributes.id);

        if (cropItem != null)
        {
            // MyItemList에서 해당 작물을 찾음
            Item inventoryItem = InventoryManager.Instance.MyItemList.Find(item => item.itemID == cropAttributes.id);

            if (inventoryItem != null)
            {
                // 이미 존재하는 작물일 경우 수량 증가
                if (int.TryParse(inventoryItem.quantity, out int currentQuantity))
                {
                    inventoryItem.quantity = (currentQuantity + 1).ToString(); // cropAttributes.yieldAmount
                }
                else
                {
                    Debug.LogError($"'{inventoryItem.itemName}'의 수량 데이터가 잘못되었습니다.");
                }
            }
            else
            {
                // MyItemList에 없는 경우 AllItemList에서 찾은 cropItem을 MyItemList에 추가
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
            Debug.LogError($"AllItemList에서 작물 ID '{cropAttributes.id}'에 대한 정보를 찾을 수 없습니다.");
            return;
        }

        // 인벤토리 저장 및 UI 갱신
        InventoryManager.Instance.Save();
    }
}
