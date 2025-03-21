using System.Collections.Generic;
using UnityEngine;

public class SprinklerPool : MonoBehaviour
{
    public GameObject sprinklerPrefab; // ������Ŭ�� ������
    public int poolSize = 10; // ������Ŭ�� ����
    private Queue<GameObject> sprinklerPool = new Queue<GameObject>();

    private void Start()
    {
        // �̸� 10�� �����ϰ� ��Ȱ��ȭ ���·� ť�� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject sprinkler = Instantiate(sprinklerPrefab);
            sprinkler.SetActive(false);
            sprinklerPool.Enqueue(sprinkler);
        }
    }

    // Ǯ���� ������Ŭ�� ��������
    public GameObject GetSprinkler(Vector3 position)
    {
        if (sprinklerPool.Count > 0)
        {
            GameObject sprinkler = sprinklerPool.Dequeue();
            sprinkler.transform.position = position;
            sprinkler.SetActive(true);
            return sprinkler;
        }
        return null; // Ǯ�� �����ִ� ������Ŭ���� ������ null ��ȯ
    }

    // ������Ŭ�� �ٽ� ��ȯ (��Ȱ��ȭ �� ť�� ����)
    public void ReturnSprinkler(GameObject sprinkler)
    {
        sprinkler.SetActive(false);
        sprinklerPool.Enqueue(sprinkler);
    }
}
