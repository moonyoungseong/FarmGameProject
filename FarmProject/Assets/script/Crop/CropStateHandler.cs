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
        //Debug.Log("Crop assigned: " + crop.name); // crop �Ҵ� Ȯ��
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
            //Debug.Log("SprinklerPool is assigned."); // sprinklerPool�� �Ҵ�� ��� ------------------- ��� �ּ� ó�� 
        }

        if (sprinklerTimeline == null)
        {
            Debug.LogWarning("SprinklerTimeline is not assigned!");
        }
        else
        {
            //Debug.Log("SprinklerTimeline is assigned."); // sprinklerTimeline�� �Ҵ�� ��� ------------------- ��� �ּ� ó�� 
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
                //Debug.Log("Crop dynamically assigned: " + foundCrop.name);
            }
        }

        //// E + R Ű�� ���ÿ� ������ �� �� �ֱ� �� ������Ŭ�� Ȱ��ȭ
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.E) && crop != null)
        {
            Debug.Log("E + R keys pressed. Watering crop..."); // E + R Ű�� ������ �� �α� ���
            //crop.WaterCrop(); // ���� �� �ֱ� ��� ����
            ActivateSprinkler(); // ������Ŭ�� Ȱ��ȭ �� Ÿ�Ӷ��� ����
        }
    }

    public void ActivateSprinkler()
    {
        if (crop == null)
        {
            Debug.LogWarning("Crop is not assigned! Cannot water the crop.");
            return;
        }

        crop.WaterCrop(); // �۹��� �� �ֱ�

        SprinklerTimelineHandler sprinklerTimelineHandler = FindObjectOfType<SprinklerTimelineHandler>();
        if (sprinklerTimelineHandler != null)
        {
            Debug.Log("Starting sprinkler animation.");
            sprinklerTimelineHandler.StartSprinklerAnimation(transform.position, 10); // 10�� ������Ŭ�� Ȱ��ȭ
        }
        else
        {
            Debug.LogWarning("SprinklerTimelineHandler is not found in the scene.");
        }
    }
}






