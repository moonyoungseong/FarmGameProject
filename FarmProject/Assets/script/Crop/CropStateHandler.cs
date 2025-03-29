using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CropStateHandler : MonoBehaviour
{
    private Crop crop;  // crop 변수
    public PlayableDirector sprinklerTimeline; // 타임라인 추가
    public SprinklerPool sprinklerPool; // 오브젝트 풀링 시스템 추가

    // crop 할당 메서드
    public void AssignCrop(Crop newCrop)
    {
        crop = newCrop; // 새로운 crop 객체를 할당
        //Debug.Log("Crop assigned: " + crop.name); // crop 할당 확인
    }

    private void Start()
    {
        // null 검사 및 경고 메시지 출력
        if (sprinklerPool == null)
        {
            Debug.LogWarning("SprinklerPool is not assigned!");
        }
        else
        {
            //Debug.Log("SprinklerPool is assigned."); // sprinklerPool이 할당된 경우 ------------------- 잠시 주석 처리 
        }

        if (sprinklerTimeline == null)
        {
            Debug.LogWarning("SprinklerTimeline is not assigned!");
        }
        else
        {
            //Debug.Log("SprinklerTimeline is assigned."); // sprinklerTimeline이 할당된 경우 ------------------- 잠시 주석 처리 
        }
    }

    private void Update()
    {
        if (crop == null)
        {
            Crop foundCrop = FindObjectOfType<Crop>(); // 씬에서 자동 검색
            if (foundCrop != null)
            {
                AssignCrop(foundCrop);
                //Debug.Log("Crop dynamically assigned: " + foundCrop.name);
            }
        }

        //// E + R 키가 동시에 눌렸을 때 물 주기 및 스프링클러 활성화
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.E) && crop != null)
        {
            Debug.Log("E + R keys pressed. Watering crop..."); // E + R 키가 눌렸을 때 로그 출력
            //crop.WaterCrop(); // 기존 물 주기 기능 실행
            ActivateSprinkler(); // 스프링클러 활성화 및 타임라인 실행
        }
    }

    public void ActivateSprinkler()
    {
        if (crop == null)
        {
            Debug.LogWarning("Crop is not assigned! Cannot water the crop.");
            return;
        }

        crop.WaterCrop(); // 작물에 물 주기

        SprinklerTimelineHandler sprinklerTimelineHandler = FindObjectOfType<SprinklerTimelineHandler>();
        if (sprinklerTimelineHandler != null)
        {
            Debug.Log("Starting sprinkler animation.");
            sprinklerTimelineHandler.StartSprinklerAnimation(transform.position, 10); // 10개 스프링클러 활성화
        }
        else
        {
            Debug.LogWarning("SprinklerTimelineHandler is not found in the scene.");
        }
    }
}






