using UnityEngine;
using UnityEngine.UI;

public class HarvestableCrop : MonoBehaviour
{
    public CropAttributes cropAttributes; // 작물 속성 데이터
    public Button harvestButton;          // 수확 버튼

    private void Start()
    {
        // 버튼에 클릭 이벤트 연결
        if (harvestButton != null)
        {
            harvestButton.onClick.AddListener(HarvestCrop);
        }
        else
        {
            Debug.LogError("수확 버튼이 설정되지 않았습니다.");
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
