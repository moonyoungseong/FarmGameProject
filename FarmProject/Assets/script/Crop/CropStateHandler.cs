using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// CropStateHandler.cs 
/// 플레이어 입력에 따라 물 주기(Watering) 기능을 실행하는 컴포넌트.
///
/// - E + R 입력 시 물 주기 수행
/// - 스프링클러 풀링 시스템을 통해 스프링클러 이펙트 재생
/// - 스프링클러 타임라인(PlayableDirector) 연동
///
/// 이 스크립트는 작물 오브젝트의 상태와 외부 이펙트 시스템 간의 연결 역할을 담당
/// </summary>
public class CropStateHandler : MonoBehaviour
{
    private Crop crop;

    /// 스프링클러 타임라인 실행을 위한 PlayableDirector
    public PlayableDirector sprinklerTimeline;

    /// 스프링클러 이펙트 오브젝트 풀링 시스템
    public SprinklerPool sprinklerPool;

    /// <summary>
    /// 외부에서 Crop 객체를 할당하는 메서드.
    /// 작물 생성 후 동적으로 연결할 때 사용한다.
    /// </summary>
    public void AssignCrop(Crop newCrop)
    {
        crop = newCrop;
    }

    private void Start()
    {
        // 풀링 시스템과 타임라인이 정상적으로 연결되어 있는지 확인
        if (sprinklerPool == null)
            Debug.LogWarning("SprinklerPool is not assigned!");

        if (sprinklerTimeline == null)
            Debug.LogWarning("SprinklerTimeline is not assigned!");
    }


    private void Update()
    {
        // crop이 아직 연결되지 않았다면 씬에서 자동으로 하나를 탐색하여 연결
        if (crop == null)
        {
            Crop foundCrop = FindObjectOfType<Crop>();
            if (foundCrop != null)
                AssignCrop(foundCrop);
        }

        // 플레이어 입력: E + R 동시 입력 시 물 주기 동작
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.E) && crop != null)
        {
            ActivateSprinkler();
        }
    }

    // 물 주기 동작을 수행하고 스프링클러 애니메이션 및 이펙트 시스템을 실행
    public void ActivateSprinkler()
    {
        if (crop == null)
        {
            Debug.LogWarning("Crop is not assigned! Cannot water the crop.");
            return;
        }

        // 작물 성장 시스템에 물 공급
        crop.WaterCrop();

        // 타임라인 + 스프링클러 활성화 핸들러 탐색
        SprinklerTimelineHandler sprinklerTimelineHandler = FindObjectOfType<SprinklerTimelineHandler>();

        if (sprinklerTimelineHandler != null)
        {
            sprinklerTimelineHandler.StartSprinklerAnimation(transform.position, 10);
        }
        else
        {
            Debug.LogWarning("SprinklerTimelineHandler is not found in the scene.");
        }
    }
}
