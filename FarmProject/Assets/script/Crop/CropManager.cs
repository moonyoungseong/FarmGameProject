using UnityEngine;

public class CropManager : MonoBehaviour
{
    private CropAttributes cropAttributes;  // CropAttributes를 연결
    private GameObject currentCropObject;  // 현재 상태의 작물 오브젝트
    private float currentGrowthTime;       // 현재 작물의 성장 시간

    private int waterCount;                // 물을 준 횟수
    private bool isFullyGrown;             // 작물이 완전히 자랐는지 여부
    private bool isDead;                   // 작물이 죽었는지 여부

    private float timeToWait = 10f;        // 시간 멈추는 간격 (초)
    private float timeCounter;             // 타이머 카운터

    // CropFactory에서 넘겨받은 CropAttributes 설정
    public void SetCropAttributes(CropAttributes attributes)
    {
        cropAttributes = attributes;
        currentGrowthTime = 0f;  // 초기화
        waterCount = 0;          // 물 준 횟수 초기화
        isFullyGrown = false;    // 아직 완전히 자라지 않음
        isDead = false;          // 죽지 않음
        currentCropObject = Instantiate(cropAttributes.seedPrefab, transform.position, Quaternion.identity);
        timeCounter = 0f;        // 타이머 초기화
    }

    void Update()
    {
        if (waterCount > 0 && !isDead)
        {
            // 시간이 흐르고 있는 경우
            if (timeCounter < timeToWait)
            {
                timeCounter += Time.deltaTime;  // 타이머 진행
            }
            else
            {
                currentGrowthTime += Time.deltaTime;  // 시간이 흐르면 성장
                CheckGrowthStage();
                timeCounter = 0f;  // 타이머 초기화
            }
        }
    }

    void CheckGrowthStage()
    {
        int growthStage = Mathf.FloorToInt((currentGrowthTime / cropAttributes.growthTime) * cropAttributes.growthStages);
        Debug.Log($"Current Growth Stage: {growthStage}");  // 현재 성장 단계 출력

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
        // 현재 오브젝트가 있으면 삭제
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
        if (!isDead)  // 작물이 죽었을 경우 물을 줄 수 없도록
        {
            waterCount++;  // 물을 준 횟수 증가

            if (waterCount == 1)
            {
                // 씨앗 상태에서 첫 번째 물을 줌
                currentGrowthTime = 0f;  // 첫 물은 시작 시점에서 시간 진행
            }
            else if (waterCount == 2)
            {
                // 두 번째 물은 성장 중
            }
            else if (waterCount == 3)
            {
                // 세 번째 물은 중간 단계에서 주는 마지막 물
            }
            else if (waterCount >= 4)
            {
                // 네 번째 물을 주면 작물이 죽음
                Die();
            }
        }
    }

    // 작물이 죽은 상태로 처리하는 메서드
    void Die()
    {
        isDead = true;
        if (currentCropObject != null)
        {
            Destroy(currentCropObject);  // 죽은 작물 오브젝트 삭제
        }

        // 죽은 상태에 맞는 프리팹을 표시할 수도 있음 (예: 시든 작물 이미지)
        // 현재는 그냥 죽은 상태로 처리
        Debug.Log("작물이 죽었습니다.");
    }
}
