using UnityEngine;
using System.Collections.Generic;

public class SprinklerPool : MonoBehaviour
{
    public GameObject sprinklerPrefab; // ������Ŭ�� ������
    private Queue<GameObject> pool = new Queue<GameObject>(); // ������Ʈ Ǯ

    public List<GameObject> sceneSprinklers; // ���� ��ġ�� ������Ŭ�� ���

    // ���� ��ġ�� ������Ŭ������ Ǯ�� ���
    public void InitializePoolWithSceneSprinklers()
    {
        foreach (var sprinkler in sceneSprinklers)
        {
            if (!pool.Contains(sprinkler)) // �ߺ� ��� ����
            {
                sprinkler.SetActive(false); // Ǯ�� ����ϱ� ���� ��Ȱ��ȭ
                pool.Enqueue(sprinkler); // Ǯ�� �߰�
            }
        }
    }

    // Ǯ���� ������Ŭ�� �������� (���� �������� ����)
    public GameObject GetSprinkler()
    {
        // Ǯ���� ������Ŭ���� ������ ��, �̹� �����ϴ� ������Ʈ�� ��ȯ
        if (pool.Count > 0)
        {
            GameObject sprinkler = pool.Dequeue(); // Ǯ���� ��������
            sprinkler.SetActive(false); // ������ ������Ŭ���� ��Ȱ��ȭ
            return sprinkler;
        }
        else
        {
            return null; // Ǯ�� �� �̻� ������Ʈ�� ������ null ��ȯ
        }
    }

    // ������Ŭ���� Ǯ�� ��ȯ�ϱ�
    public void ReturnSprinkler(GameObject sprinkler)
    {
        sprinkler.SetActive(false); // ��Ȱ��ȭ
        pool.Enqueue(sprinkler); // Ǯ�� ��ȯ
    }
}






