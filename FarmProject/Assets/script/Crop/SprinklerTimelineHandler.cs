using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class SprinklerTimelineHandler : MonoBehaviour
{
    public PlayableDirector timeline; // 타임라인 참조
    private List<GameObject> activeSprinklers = new List<GameObject>(); // 현재 활성화된 스프링클러들
    private SprinklerPool sprinklerPool; // 오브젝트 풀링 시스템

    private void Start()
    {
        sprinklerPool = FindObjectOfType<SprinklerPool>(); // 오브젝트 풀링 찾기
        sprinklerPool.InitializePoolWithSceneSprinklers(); // 씬에 배치된 스프링클러를 풀에 등록
        timeline.stopped += OnTimelineEnd; // 타임라인 종료 이벤트 연결
    }

    // 타임라인 시작 시 10개의 스프링클러를 활성화
    public void StartSprinklerAnimation(Vector3 position, int count = 10)
    {
        activeSprinklers.Clear(); // 기존 활성화된 스프링클러 초기화

        // 씬에 배치된 스프링클러 중에서 풀에 이미 있는 오브젝트만 가져와서 활성화
        for (int i = 0; i < count; i++)
        {
            GameObject sprinkler = sprinklerPool.GetSprinkler(); // 풀에서 비활성화된 스프링클러 가져오기
            if (sprinkler != null)
            {
                sprinkler.transform.position = position; // 위치 설정
                activeSprinklers.Add(sprinkler); // 리스트에 추가
            }
        }

        // 타임라인에서 활성화된 스프링클러들을 시작
        foreach (var sprinkler in activeSprinklers)
        {
            sprinkler.SetActive(true); // 타임라인에서만 활성화
        }

        timeline.Play(); // 타임라인 실행
        AudioManager.Instance.PlaySFX(11);   // 물 뿌릴때 효과음

        AudioManager.Instance.StopAllSFXAfterTime(9f);     // 8초뒤 효과음 끄기
    }

    private void OnTimelineEnd(PlayableDirector pd)
    {
        // 타임라인 종료 후, 스프링클러 풀로 반환
        foreach (var sprinkler in activeSprinklers)
        {
            sprinklerPool.ReturnSprinkler(sprinkler); // 풀에 반환
        }

        activeSprinklers.Clear(); // 리스트 초기화
    }
}






