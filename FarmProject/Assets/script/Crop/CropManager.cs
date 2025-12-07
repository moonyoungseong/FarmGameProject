using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CropManager.cs
/// 개별 작물의 성장 로직을 관리하는 스크립트.
/// - CropAttributes 데이터를 받아 초기 상태 생성
/// - 물을 준 이후부터 성장 시간이 흐름
/// - 성장 단계에 따라 프리팹 교체
/// - 최종적으로 fully grown 상태 여부 관리
/// </summary>
public class CropManager : MonoBehaviour
{
    private CropAttributes cropAttributes;

    // 현재 보여지고 있는 작물 오브젝트(씨앗/중간/완성)
    private GameObject currentCropObject;

    // 현재까지 누적된 성장 시간
    private float currentGrowthTime;

    // 물을 주었는지 여부
    private bool hasWatered;

    // 작물이 완전히 성장했는지 여부
    private bool isFullyGrown;

    /// <summary>
    /// CropFactory에서 호출하는 초기 설정 메서드.
    /// 작물 속성(Attribute)을 받아 씨앗 프리팹을 생성
    /// </summary>
    public void SetCropAttributes(CropAttributes attributes)
    {
        cropAttributes = attributes;
        currentGrowthTime = 0f;
        hasWatered = false;
        isFullyGrown = false;

        // 첫 상태(씨앗) 생성
        currentCropObject = Instantiate(
            cropAttributes.seedPrefab,
            transform.position,
            Quaternion.identity
        );
    }

    private void Update()
    {
        // 물을 준 이후에만 성장 시간이 흐름
        if (hasWatered && !isFullyGrown)
        {
            currentGrowthTime += Time.deltaTime;   // 실시간 성장
            CheckGrowthStage();
        }
    }


    // 현재 성장 시간에 따라 어느 성장 단계인지 계산하고 적절한 프리팹으로 교체한다.
    private void CheckGrowthStage()
    {
        // 현재 성장 진행 비율로 단계 계산 (0, 1, 2...)
        int growthStage = Mathf.FloorToInt(
            (currentGrowthTime / cropAttributes.growthTime) * cropAttributes.growthStages
        );

        // 성장 단계별 프리팹 교체
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
                isFullyGrown = true;   // 최종 성장 완료 표시
                break;
        }
    }

    /// <summary>
    /// 현재 작물 오브젝트를 삭제하고 새 프리팹으로 교체한다.
    /// </summary>
    private void SetCropPrefab(GameObject prefab)
    {
        if (currentCropObject != null)
            Destroy(currentCropObject);

        currentCropObject = Instantiate(
            prefab,
            transform.position,
            Quaternion.identity
        );
    }

    // Crop.cs 등 다른 스크립트에서 호출하는 물주기 메서드.
    public void WaterCrop()
    {
        if (!isFullyGrown && !hasWatered)
        {
            hasWatered = true;
        }
    }
}
