using UnityEngine;
using System.Collections.Generic;

public class SprinklerPool : MonoBehaviour
{
    public GameObject sprinklerPrefab; // ������Ŭ�� ������
    private Queue<GameObject> pool = new Queue<GameObject>(); // ������Ʈ Ǯ

    // ������Ŭ�� Ǯ���� ��������
    public GameObject GetSprinkler()
    {
        if (pool.Count > 0)
        {
            GameObject sprinkler = pool.Dequeue();
            sprinkler.SetActive(true); // Ȱ��ȭ
            return sprinkler;
        }
        else
        {
            GameObject sprinkler = Instantiate(sprinklerPrefab);
            sprinkler.SetActive(false); // ��Ȱ��ȭ�� ���·� ����
            return sprinkler;
        }
    }

    // ������Ŭ�� Ǯ�� ��ȯ�ϱ�
    public void ReturnSprinkler(GameObject sprinkler)
    {
        sprinkler.SetActive(false); // ��Ȱ��ȭ
        pool.Enqueue(sprinkler); // Ǯ�� ��ȯ
    }
}
