using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class SprinklerTimelineHandler : MonoBehaviour
{
    public PlayableDirector timeline; // Ÿ�Ӷ��� ����
    private List<GameObject> activeSprinklers = new List<GameObject>(); // ���� Ȱ��ȭ�� ������Ŭ����
    private SprinklerPool sprinklerPool; // ������Ʈ Ǯ�� �ý���

    private void Start()
    {
        sprinklerPool = FindObjectOfType<SprinklerPool>(); // ������Ʈ Ǯ�� ã��
        sprinklerPool.InitializePoolWithSceneSprinklers(); // ���� ��ġ�� ������Ŭ���� Ǯ�� ���
        timeline.stopped += OnTimelineEnd; // Ÿ�Ӷ��� ���� �̺�Ʈ ����
    }

    // Ÿ�Ӷ��� ���� �� 10���� ������Ŭ���� Ȱ��ȭ
    public void StartSprinklerAnimation(Vector3 position, int count = 10)
    {
        activeSprinklers.Clear(); // ���� Ȱ��ȭ�� ������Ŭ�� �ʱ�ȭ

        // ���� ��ġ�� ������Ŭ�� �߿��� Ǯ�� �̹� �ִ� ������Ʈ�� �����ͼ� Ȱ��ȭ
        for (int i = 0; i < count; i++)
        {
            GameObject sprinkler = sprinklerPool.GetSprinkler(); // Ǯ���� ��Ȱ��ȭ�� ������Ŭ�� ��������
            if (sprinkler != null)
            {
                sprinkler.transform.position = position; // ��ġ ����
                activeSprinklers.Add(sprinkler); // ����Ʈ�� �߰�
            }
        }

        // Ÿ�Ӷ��ο��� Ȱ��ȭ�� ������Ŭ������ ����
        foreach (var sprinkler in activeSprinklers)
        {
            sprinkler.SetActive(true); // Ÿ�Ӷ��ο����� Ȱ��ȭ
        }

        timeline.Play(); // Ÿ�Ӷ��� ����
        AudioManager.Instance.PlaySFX(11);   // �� �Ѹ��� ȿ����

        AudioManager.Instance.StopAllSFXAfterTime(9f);     // 8�ʵ� ȿ���� ����
    }

    private void OnTimelineEnd(PlayableDirector pd)
    {
        // Ÿ�Ӷ��� ���� ��, ������Ŭ�� Ǯ�� ��ȯ
        foreach (var sprinkler in activeSprinklers)
        {
            sprinklerPool.ReturnSprinkler(sprinkler); // Ǯ�� ��ȯ
        }

        activeSprinklers.Clear(); // ����Ʈ �ʱ�ȭ
    }
}






