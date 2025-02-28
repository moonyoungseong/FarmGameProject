using UnityEngine;

public class CropManager : MonoBehaviour
{
    private CropAttributes cropAttributes;  // CropAttributes�� ����
    private GameObject currentCropObject;  // ���� ������ �۹� ������Ʈ
    private float currentGrowthTime;       // ���� �۹��� ���� �ð�

    private bool hasWatered;               // ���� �־����� Ȯ���ϴ� ����
    private bool isFullyGrown;             // �۹��� ������ �ڶ����� ����

    // CropFactory���� �Ѱܹ��� CropAttributes ����
    public void SetCropAttributes(CropAttributes attributes)
    {
        cropAttributes = attributes;
        currentGrowthTime = 0f;  // �ʱ�ȭ
        hasWatered = false;      // ���� ���� ���� ����
        isFullyGrown = false;    // ���� ������ �ڶ��� ����
        currentCropObject = Instantiate(cropAttributes.seedPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        // ���� �־��� ���� �ð��� �帣���� ó��
        if (hasWatered && !isFullyGrown)
        {
            currentGrowthTime += Time.deltaTime;  // �ð��� �帣�� ����
            CheckGrowthStage();
        }
    }

    void CheckGrowthStage()
    {
        int growthStage = Mathf.FloorToInt((currentGrowthTime / cropAttributes.growthTime) * cropAttributes.growthStages);

        // ���� �ܰ迡 ���� �������� ��ü
        switch (growthStage)
        {
            case 0:
                SetCropPrefab(cropAttributes.seedPrefab);
                break;
            case 1:
                SetCropPrefab(cropAttributes.growthPrefab);
                break;
            case 2:
                SetCropPrefab(cropAttributes.fullyGrownPrefab);
                isFullyGrown = true; // ������ �ڶ� ���·� ����
                break;
        }
    }

    void SetCropPrefab(GameObject prefab)
    {
        if (currentCropObject != null)
        {
            Destroy(currentCropObject);  // ���� ������Ʈ ����
        }

        // ���ο� ���·� �������� ����
        currentCropObject = Instantiate(prefab, transform.position, Quaternion.identity);
    }

    // ���� �ִ� �޼���
    public void WaterCrop()
    {
        if (!isFullyGrown && !hasWatered)  // ���� �� ���� �� �� �ֵ��� ����, �̹� �ڶ� �۹����� ���� ���� ����
        {
            hasWatered = true;  // ���� �־��ٰ� ǥ��
        }
    }
}
