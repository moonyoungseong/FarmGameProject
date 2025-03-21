using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CropStateHandler : MonoBehaviour
{
    private Crop crop;  // crop ����
    public PlayableDirector sprinklerTimeline; // Ÿ�Ӷ��� �߰�
    public SprinklerPool sprinklerPool; // ������Ʈ Ǯ�� �ý��� �߰�

    // crop �Ҵ� �޼���
    public void AssignCrop(Crop newCrop)
    {
        crop = newCrop; // ���ο� crop ��ü�� �Ҵ�
        Debug.Log("Crop assigned: " + crop.name); // crop �Ҵ� Ȯ��
    }

    private void Start()
    {
        // null �˻� �� ��� �޽��� ���
        if (sprinklerPool == null)
        {
            Debug.LogWarning("SprinklerPool is not assigned!");
        }
        else
        {
            Debug.Log("SprinklerPool is assigned."); // sprinklerPool�� �Ҵ�� ���
        }

        if (sprinklerTimeline == null)
        {
            Debug.LogWarning("SprinklerTimeline is not assigned!");
        }
        else
        {
            Debug.Log("SprinklerTimeline is assigned."); // sprinklerTimeline�� �Ҵ�� ���
        }
    }

    private void Update()
    {
        if (crop == null)
        {
            Crop foundCrop = FindObjectOfType<Crop>(); // ������ �ڵ� �˻�
            if (foundCrop != null)
            {
                AssignCrop(foundCrop);
                Debug.Log("Crop dynamically assigned: " + foundCrop.name);
            }
        }

        //// E + R Ű�� ���ÿ� ������ �� �� �ֱ� �� ������Ŭ�� Ȱ��ȭ
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.E) && crop != null)
        {
            Debug.Log("E + R keys pressed. Watering crop..."); // E + R Ű�� ������ �� �α� ���
            //crop.WaterCrop(); // ���� �� �ֱ� ��� ����
            ActivateSprinkler(); // ������Ŭ�� Ȱ��ȭ �� Ÿ�Ӷ��� ����
        }

        // ActivateSprinkler(); // ������Ŭ�� Ȱ��ȭ �� Ÿ�Ӷ��� ����
    }

    public void ActivateSprinkler()
    {
        if (crop == null)
        {
            Debug.LogWarning("Crop is not assigned! Cannot water the crop.");
            return;
        }

        crop.WaterCrop(); // ���� �� �ֱ� ��� ����

        if (sprinklerPool == null) return; // Ǯ�� �ý����� null�̸� �Լ��� ����

        // ������Ʈ Ǯ���� ������Ŭ���� �����ɴϴ�.
        GameObject sprinkler = sprinklerPool.GetSprinkler();

        if (sprinkler != null)
        {
            Debug.Log("Sprinkler activated: " + sprinkler.name); // ������Ŭ�� Ȱ��ȭ �α� ���
            sprinkler.SetActive(true); // ������ ������Ŭ���� Ȱ��ȭ

            // ������Ŭ���� ��ġ�� ������� Ÿ�Ӷ��� ����
            SprinklerTimelineHandler sprinklerTimelineHandler = FindObjectOfType<SprinklerTimelineHandler>();
            if (sprinklerTimelineHandler != null)
            {
                Debug.Log("Starting sprinkler animation at position: " + sprinkler.transform.position); // Ÿ�Ӷ��� ���� ��ġ Ȯ��
                sprinklerTimelineHandler.StartSprinklerAnimation(sprinkler.transform.position);
            }
            else
            {
                Debug.LogWarning("SprinklerTimelineHandler is not found in the scene."); // SprinklerTimelineHandler�� ���� ������ ���
            }
        }
        else
        {
            Debug.LogWarning("Failed to get sprinkler from pool."); // Ǯ���� ������Ŭ���� �������� ������ �� ���
        }
    }
}
