using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class SprinklerTimelineHandler : MonoBehaviour
{
    public PlayableDirector timeline; // Ÿ�Ӷ��� ����
    private List<GameObject> activeSprinklers = new List<GameObject>(); // ���� ��� ���� ������Ŭ����
    private SprinklerPool sprinklerPool; // ������Ʈ Ǯ�� �ý���

    private void Start()
    {
        sprinklerPool = FindObjectOfType<SprinklerPool>(); // ������Ʈ Ǯ�� ã��
        timeline.stopped += OnTimelineEnd; // Ÿ�Ӷ��� ���� �̺�Ʈ ����
    }

    // Ÿ�Ӷ��� ���� �� ������Ŭ�� Ȱ��ȭ (���� �� Ȱ��ȭ)
    public void StartSprinklerAnimation(Vector3 position, int count = 10)
    {
        activeSprinklers.Clear(); // ���� Ȱ��ȭ�� ������Ŭ������ �ʱ�ȭ

        for (int i = 0; i < count; i++)
        {
            GameObject sprinkler = sprinklerPool.GetSprinkler(); // Ǯ���� ������Ŭ�� ��������

            if (sprinkler != null)
            {
                // ������Ŭ���� ��ġ�� Ÿ�Ӷ��ο��� ó���ϰ�, ���⼭�� Ȱ��ȭ��
                sprinkler.SetActive(true); // ������Ʈ Ȱ��ȭ
                activeSprinklers.Add(sprinkler); // Ȱ��ȭ�� ������Ŭ�� ��Ͽ� �߰�
            }
        }

        // Ÿ�Ӷ��� ����
        timeline.Play();
    }

    // Ÿ�Ӷ��� ���� �� ������Ŭ�� Ǯ�� ��ȯ
    private void OnTimelineEnd(PlayableDirector pd)
    {
        foreach (GameObject sprinkler in activeSprinklers)
        {
            if (sprinkler != null)
            {
                // Ǯ���� ����� ���� ��ȯ�ϰ�, ���� �ִ� ������Ʈ�� ��Ȱ��ȭ
                sprinklerPool.ReturnSprinkler(sprinkler);
            }
        }

        activeSprinklers.Clear(); // ���� ������Ŭ�� ����Ʈ �ʱ�ȭ
    }
}
