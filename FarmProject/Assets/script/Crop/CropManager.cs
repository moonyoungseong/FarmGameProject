using UnityEngine;

public class CropManager : MonoBehaviour
{
    private CropAttributes cropAttributes;  // CropAttributes�� ����
    private GameObject currentCropObject;  // ���� ������ �۹� ������Ʈ
    private int currentGrowthStage = 0;    // ���� ���� �ܰ� (0: ����, 1: ���� ��, 2: ���� ����)

    // CropFactory���� �Ѱܹ��� CropAttributes ����
    public void SetCropAttributes(CropAttributes attributes)
    {
        cropAttributes = attributes;
        currentGrowthStage = 0;  // �ʱ� ���� �ܰ�
        currentCropObject = Instantiate(cropAttributes.seedPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        // ���� �־��� �� ���� �ܰ谡 ����ǵ��� ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            WaterCrop();
        }
    }

    void WaterCrop()
    {
        // ���� �ܰ谡 �ִ� �ܰ躸�� ������, ���� ����
        if (currentGrowthStage < cropAttributes.growthStages)
        {
            currentGrowthStage++;  // �ܰ� ����
            CheckGrowthStage();  // ���� �ܰ� Ȯ�� �� ������ ��ü
        }
    }

    void CheckGrowthStage()
    {
        // ���� �ܰ迡 �´� �������� ����
        switch (currentGrowthStage)
        {
            case 0:
                SetCropPrefab(cropAttributes.seedPrefab);
                break;
            case 1:
                SetCropPrefab(cropAttributes.growthPrefab);
                break;
            case 2:
                SetCropPrefab(cropAttributes.fullyGrownPrefab);
                break;
        }
    }

    void SetCropPrefab(GameObject prefab)
    {
        // ���� ������Ʈ�� �����ϰ� ���ο� ���·� ��ü
        if (currentCropObject != null)
        {
            Destroy(currentCropObject);
        }

        // ���ο� �������� �ν��Ͻ�ȭ
        currentCropObject = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}

