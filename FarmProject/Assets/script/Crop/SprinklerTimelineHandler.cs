using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// SprinklerTimelineHandler.cs
/// 스프링클러 타임라인 재생 시 필요한 스프링클러 오브젝트들을
/// SprinklerPool 에서 가져와 활성화하고,
/// 타임라인 종료 시 다시 반환하는 역할을 담당.
/// 
/// - 타임라인에 맞춰 스프링클러 여러 개를 동시에 활성화
/// - 오브젝트 풀링 시스템(SprinklerPool)과 연동
/// - 타임라인 종료 후 스프링클러 자동 반환
/// </summary>
public class SprinklerTimelineHandler : MonoBehaviour
{
    // 스프링클러 실행에 사용되는 Timeline 컴포넌트
    public PlayableDirector timeline;

    // 현재 활성화되어 타임라인에서 사용 중인 스프링클러 목록
    private List<GameObject> activeSprinklers = new List<GameObject>();

    private SprinklerPool sprinklerPool;

    private void Start()
    {
        sprinklerPool = FindObjectOfType<SprinklerPool>(); // 오브젝트 풀 검색
        sprinklerPool.InitializePoolWithSceneSprinklers(); // 씬에 배치된 스프링클러 초기 등록
        timeline.stopped += OnTimelineEnd; // 타임라인 종료 시 콜백 연결
    }

    /// <summary>
    /// 스프링클러 타임라인을 실행하는 메서드.
    /// 지정된 위치에 원하는 개수만큼 스프링클러를 가져와 활성화한다.
    /// </summary>
    public void StartSprinklerAnimation(Vector3 position, int count = 10)
    {
        activeSprinklers.Clear(); // 이전 스프링클러 목록 초기화

        // 풀에서 스프링클러 가져오기
        for (int i = 0; i < count; i++)
        {
            GameObject sprinkler = sprinklerPool.GetSprinkler();
            if (sprinkler != null)
            {
                sprinkler.transform.position = position; // 위치 설정
                activeSprinklers.Add(sprinkler);
            }
        }

        // 가져온 스프링클러 활성화
        foreach (var sprinkler in activeSprinklers)
        {
            sprinkler.SetActive(true);
        }

        timeline.Play(); // 타임라인 재생

        // 효과음 처리
        AudioManager.Instance.PlaySFX(11);       // 물 뿌리는 소리
        AudioManager.Instance.StopAllSFXAfterTime(9f); // 9초 후 모든 효과음 중지
    }

    // 사용했던 스프링클러들을 모두 풀에 반환한다.
    private void OnTimelineEnd(PlayableDirector pd)
    {
        foreach (var sprinkler in activeSprinklers)
        {
            sprinklerPool.ReturnSprinkler(sprinkler);
        }

        activeSprinklers.Clear(); // 목록 초기화
    }
}
