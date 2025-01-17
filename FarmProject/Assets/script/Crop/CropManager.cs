using UnityEngine;

public class CropManager : MonoBehaviour
{
    private CropAttributes cropAttributes;  // CropAttributes를 연결
    private GameObject currentCropObject;  // 현재 상태의 작물 오브젝트
    private int currentGrowthStage = 0;    // 현재 성장 단계 (0: 씨앗, 1: 성장 중, 2: 완전 성장)

    // CropFactory에서 넘겨받은 CropAttributes 설정
    public void SetCropAttributes(CropAttributes attributes)
    {
        cropAttributes = attributes;
        currentGrowthStage = 0;  // 초기 성장 단계
        currentCropObject = Instantiate(cropAttributes.seedPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        // 물을 주었을 때 성장 단계가 진행되도록 함
        if (Input.GetKeyDown(KeyCode.R))
        {
            WaterCrop();
        }
    }

    void WaterCrop()
    {
        // 현재 단계가 최대 단계보다 작으면, 성장 진행
        if (currentGrowthStage < cropAttributes.growthStages)
        {
            currentGrowthStage++;  // 단계 증가
            CheckGrowthStage();  // 성장 단계 확인 및 프리팹 교체
        }
    }

    void CheckGrowthStage()
    {
        // 성장 단계에 맞는 프리팹을 설정
        switch (currentGrowthStage)
        {
            case 0:
                SetCropPrefab(cropAttributes.seedPrefab);
                break;
            case 1:
                SetCropPrefab(cropAttributes.growthPrefab);
                break;
            case 2:
                SetCropPrefab(cropAttributes.fullyGrownPrefab);
                break;
        }
    }

    void SetCropPrefab(GameObject prefab)
    {
        // 기존 오브젝트를 삭제하고 새로운 상태로 교체
        if (currentCropObject != null)
        {
            Destroy(currentCropObject);
        }

        // 새로운 프리팹을 인스턴스화
        currentCropObject = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}

