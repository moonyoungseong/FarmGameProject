using UnityEngine;

public class CropManager : MonoBehaviour
{
    private CropAttributes cropAttributes;  // CropAttributes를 연결
    private GameObject currentCropObject;  // 현재 상태의 작물 오브젝트
    private float currentGrowthTime;       // 현재 작물의 성장 시간

    // CropFactory에서 넘겨받은 CropAttributes 설정
    public void SetCropAttributes(CropAttributes attributes)
    {
        cropAttributes = attributes;
        currentGrowthTime = 0f;  // 초기화
        currentCropObject = Instantiate(cropAttributes.seedPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        // 작물 성장 관리
        if (currentGrowthTime < cropAttributes.growthTime)
        {
            currentGrowthTime += Time.deltaTime;
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
}
