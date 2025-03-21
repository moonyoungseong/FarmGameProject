using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class SprinklerTimelineHandler : MonoBehaviour
{
    public PlayableDirector timeline; // 타임라인 참조
    private List<GameObject> activeSprinklers = new List<GameObject>(); // 현재 사용 중인 스프링클러들
    private SprinklerPool sprinklerPool; // 오브젝트 풀링 시스템

    private void Start()
    {
        sprinklerPool = FindObjectOfType<SprinklerPool>(); // 오브젝트 풀링 찾기
        timeline.stopped += OnTimelineEnd; // 타임라인 종료 이벤트 연결
    }

    // 타임라인 시작 시 스프링클러 활성화 (여러 개 활성화)
    public void StartSprinklerAnimation(Vector3 position, int count = 10)
    {
        activeSprinklers.Clear(); // 기존 활성화된 스프링클러들을 초기화

        for (int i = 0; i < count; i++)
        {
            GameObject sprinkler = sprinklerPool.GetSprinkler(); // 풀에서 스프링클러 가져오기

            if (sprinkler != null)
            {
                // 스프링클러의 위치는 타임라인에서 처리하고, 여기서는 활성화만
                sprinkler.SetActive(true); // 오브젝트 활성화
                activeSprinklers.Add(sprinkler); // 활성화된 스프링클러 목록에 추가
            }
        }

        // 타임라인 실행
        timeline.Play();
    }

    // 타임라인 종료 시 스프링클러 풀로 반환
    private void OnTimelineEnd(PlayableDirector pd)
    {
        foreach (GameObject sprinkler in activeSprinklers)
        {
            if (sprinkler != null)
            {
                // 풀링을 사용할 때는 반환하고, 씬에 있는 오브젝트는 비활성화
                sprinklerPool.ReturnSprinkler(sprinkler);
            }
        }

        activeSprinklers.Clear(); // 사용된 스프링클러 리스트 초기화
    }
}
