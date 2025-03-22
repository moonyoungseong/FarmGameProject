using UnityEngine;
using System.Collections.Generic;

public class SprinklerPool : MonoBehaviour
{
    public GameObject sprinklerPrefab; // 스프링클러 프리팹
    private Queue<GameObject> pool = new Queue<GameObject>(); // 오브젝트 풀

    public List<GameObject> sceneSprinklers; // 씬에 배치된 스프링클러 목록

    // 씬에 배치된 스프링클러들을 풀에 등록
    public void InitializePoolWithSceneSprinklers()
    {
        foreach (var sprinkler in sceneSprinklers)
        {
            if (!pool.Contains(sprinkler)) // 중복 등록 방지
            {
                sprinkler.SetActive(false); // 풀에 등록하기 전에 비활성화
                pool.Enqueue(sprinkler); // 풀에 추가
            }
        }
    }

    // 풀에서 스프링클러 가져오기 (새로 생성하지 않음)
    public GameObject GetSprinkler()
    {
        // 풀에서 스프링클러를 가져올 때, 이미 존재하는 오브젝트만 반환
        if (pool.Count > 0)
        {
            GameObject sprinkler = pool.Dequeue(); // 풀에서 가져오기
            sprinkler.SetActive(false); // 가져온 스프링클러는 비활성화
            return sprinkler;
        }
        else
        {
            return null; // 풀에 더 이상 오브젝트가 없으면 null 반환
        }
    }

    // 스프링클러를 풀로 반환하기
    public void ReturnSprinkler(GameObject sprinkler)
    {
        sprinkler.SetActive(false); // 비활성화
        pool.Enqueue(sprinkler); // 풀에 반환
    }
}






