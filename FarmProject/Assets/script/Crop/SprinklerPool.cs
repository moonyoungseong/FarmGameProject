using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SprinklerPool.cs
/// 스프링클러를 관리하는 오브젝트 풀 시스템.
/// 
/// - 씬에 배치된 스프링클러 오브젝트를 풀에 미리 등록
/// - 새로운 스프링클러를 생성하지 않고, 기존 오브젝트를 재사용
/// - 사용된 스프링클러를 다시 풀로 반환하여 성능 최적화
/// </summary>
public class SprinklerPool : MonoBehaviour
{
    // 재사용 가능한 스프링클러 오브젝트를 저장하는 큐
    private Queue<GameObject> pool = new Queue<GameObject>();

    // 이 목록에 있는 오브젝트들이 풀에 등록된다.
    public List<GameObject> sceneSprinklers;

    /// <summary>
    /// 씬에 미리 배치된 스프링클러들을 풀에 등록하는 초기화 메서드.
    /// SprinklerTimelineHandler에서 호출
    /// </summary>
    public void InitializePoolWithSceneSprinklers()
    {
        foreach (var sprinkler in sceneSprinklers)
        {
            if (!pool.Contains(sprinkler))
            {
                sprinkler.SetActive(false);
                pool.Enqueue(sprinkler);
            }
        }
    }

    /// <summary>
    /// 풀에서 스프링클러 오브젝트를 하나 가져온다.
    /// 새로운 오브젝트를 생성하지 않고, 풀에 있는 경우에만 반환한다.
    /// </summary>
    public GameObject GetSprinkler()
    {
        if (pool.Count > 0)
        {
            GameObject sprinkler = pool.Dequeue();
            sprinkler.SetActive(false);
            return sprinkler;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 사용 완료된 스프링클러 오브젝트를 풀로 되돌린다.
    /// 반환 시 항상 비활성화한다.
    /// </summary>
    /// <param name="sprinkler">풀로 반환할 스프링클러 오브젝트</param>
    public void ReturnSprinkler(GameObject sprinkler)
    {
        sprinkler.SetActive(false);
        pool.Enqueue(sprinkler);
    }
}
