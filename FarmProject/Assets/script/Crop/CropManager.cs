using UnityEngine;

public class CropManager : MonoBehaviour
{
    private CropAttributes cropAttributes;  // CropAttributes를 연결
    private GameObject currentCropObject;  // 현재 상태의 작물 오브젝트
    private float currentGrowthTime;       // 현재 작물의 성장 시간

    private bool hasWatered;               // 물을 주었는지 확인하는 변수
    private bool isFullyGrown;             // 작물이 완전히 자랐는지 여부

    // CropFactory에서 넘겨받은 CropAttributes 설정
    public void SetCropAttributes(CropAttributes attributes)
    {
        cropAttributes = attributes;
        currentGrowthTime = 0f;  // 초기화
        hasWatered = false;      // 물을 아직 주지 않음
        isFullyGrown = false;    // 아직 완전히 자라지 않음
        currentCropObject = Instantiate(cropAttributes.seedPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        // 물을 주었을 때만 시간이 흐르도록 처리
        if (hasWatered && !isFullyGrown)
        {
            currentGrowthTime += Time.deltaTime;  // 시간이 흐르면 성장
            CheckGrowthStage();
        }
    }

    void CheckGrowthStage()
    {
        int growthStage = Mathf.FloorToInt((currentGrowthTime / cropAttributes.growthTime) * cropAttributes.growthStages);

        // 성장 단계에 맞춰 프리팹을 교체
        switch (growthStage)
        {
            case 0:
                SetCropPrefab(cropAttributes.seedPrefab);
                break;
            case 1:
                SetCropPrefab(cropAttributes.growthPrefab);
                break;
            case 2:
                SetCropPrefab(cropAttributes.fullyGrownPrefab);
                isFullyGrown = true; // 완전히 자란 상태로 설정
                break;
        }
    }

    void SetCropPrefab(GameObject prefab)
    {
        if (currentCropObject != null)
        {
            Destroy(currentCropObject);  // 기존 오브젝트 삭제
        }

        // 새로운 상태로 프리팹을 생성
        currentCropObject = Instantiate(prefab, transform.position, Quaternion.identity);
    }

    // 물을 주는 메서드
    public void WaterCrop()
    {
        if (!isFullyGrown && !hasWatered)  // 물을 한 번만 줄 수 있도록 설정, 이미 자란 작물에는 물을 주지 않음
        {
            hasWatered = true;  // 물을 주었다고 표시
        }
    }
}
