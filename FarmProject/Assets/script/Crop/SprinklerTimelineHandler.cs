using UnityEngine;
using UnityEngine.Playables;

public class SprinklerTimelineHandler : MonoBehaviour
{
    public PlayableDirector timeline; // Ÿ�Ӷ��� ����
    private GameObject activeSprinkler; // ���� ��� ���� ������Ŭ��
    private SprinklerPool sprinklerPool; // ������Ʈ Ǯ�� �ý���

    private void Start()
    {
        sprinklerPool = FindObjectOfType<SprinklerPool>(); // ������Ʈ Ǯ�� ã��
        timeline.stopped += OnTimelineEnd;
    }

    // Ÿ�Ӷ��� ���� �� ������Ŭ�� Ȱ��ȭ
    public void StartSprinklerAnimation(Vector3 position)
    {
        activeSprinkler = sprinklerPool.GetSprinkler(position); // ������Ʈ Ǯ���� ��������
        if (activeSprinkler != null)
        {
            timeline.Play(); // Ÿ�Ӷ��� ����
        }
    }

    // Ÿ�Ӷ��� ���� �� ������Ŭ�� Ǯ�� ��ȯ
    private void OnTimelineEnd(PlayableDirector pd)
    {
        if (activeSprinkler != null)
        {
            sprinklerPool.ReturnSprinkler(activeSprinkler); // �ٽ� Ǯ�� ��ȯ
        }
    }
}
