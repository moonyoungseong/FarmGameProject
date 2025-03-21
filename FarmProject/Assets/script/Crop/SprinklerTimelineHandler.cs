using UnityEngine;
using UnityEngine.Playables;

public class SprinklerTimelineHandler : MonoBehaviour
{
    public PlayableDirector timeline; // 타임라인 참조
    private GameObject activeSprinkler; // 현재 사용 중인 스프링클러
    private SprinklerPool sprinklerPool; // 오브젝트 풀링 시스템

    private void Start()
    {
        sprinklerPool = FindObjectOfType<SprinklerPool>(); // 오브젝트 풀링 찾기
        timeline.stopped += OnTimelineEnd;
    }

    // 타임라인 시작 시 스프링클러 활성화
    public void StartSprinklerAnimation(Vector3 position)
    {
        activeSprinkler = sprinklerPool.GetSprinkler(position); // 오브젝트 풀에서 가져오기
        if (activeSprinkler != null)
        {
            timeline.Play(); // 타임라인 실행
        }
    }

    // 타임라인 종료 시 스프링클러 풀로 반환
    private void OnTimelineEnd(PlayableDirector pd)
    {
        if (activeSprinkler != null)
        {
            sprinklerPool.ReturnSprinkler(activeSprinkler); // 다시 풀로 반환
        }
    }
}
