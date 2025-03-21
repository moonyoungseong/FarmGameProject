using System.Collections.Generic;
using UnityEngine;

public class SprinklerPool : MonoBehaviour
{
    public GameObject sprinklerPrefab; // 스프링클러 프리팹
    public int poolSize = 10; // 스프링클러 개수
    private Queue<GameObject> sprinklerPool = new Queue<GameObject>();

    private void Start()
    {
        // 미리 10개 생성하고 비활성화 상태로 큐에 저장
        for (int i = 0; i < poolSize; i++)
        {
            GameObject sprinkler = Instantiate(sprinklerPrefab);
            sprinkler.SetActive(false);
            sprinklerPool.Enqueue(sprinkler);
        }
    }

    // 풀에서 스프링클러 가져오기
    public GameObject GetSprinkler(Vector3 position)
    {
        if (sprinklerPool.Count > 0)
        {
            GameObject sprinkler = sprinklerPool.Dequeue();
            sprinkler.transform.position = position;
            sprinkler.SetActive(true);
            return sprinkler;
        }
        return null; // 풀에 남아있는 스프링클러가 없으면 null 반환
    }

    // 스프링클러 다시 반환 (비활성화 후 큐에 넣음)
    public void ReturnSprinkler(GameObject sprinkler)
    {
        sprinkler.SetActive(false);
        sprinklerPool.Enqueue(sprinkler);
    }
}
