using UnityEngine;
using System.Collections.Generic;

public class SprinklerPool : MonoBehaviour
{
    public GameObject sprinklerPrefab; // 스프링클러 프리팹
    private Queue<GameObject> pool = new Queue<GameObject>(); // 오브젝트 풀

    // 스프링클러 풀에서 가져오기
    public GameObject GetSprinkler()
    {
        if (pool.Count > 0)
        {
            GameObject sprinkler = pool.Dequeue();
            sprinkler.SetActive(true); // 활성화
            return sprinkler;
        }
        else
        {
            GameObject sprinkler = Instantiate(sprinklerPrefab);
            sprinkler.SetActive(false); // 비활성화된 상태로 생성
            return sprinkler;
        }
    }

    // 스프링클러 풀로 반환하기
    public void ReturnSprinkler(GameObject sprinkler)
    {
        sprinkler.SetActive(false); // 비활성화
        pool.Enqueue(sprinkler); // 풀에 반환
    }
}
