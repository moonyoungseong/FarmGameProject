using UnityEngine;

public class CropManager : MonoBehaviour
{
    private CropAttributes cropAttributes;  // CropAttributes�� ����
    private GameObject currentCropObject;  // ���� ������ �۹� ������Ʈ
    private float currentGrowthTime;       // ���� �۹��� ���� �ð�

    private int waterCount;                // ���� �� Ƚ��
    private bool isFullyGrown;             // �۹��� ������ �ڶ����� ����
    private bool isDead;                   // �۹��� �׾����� ����

    private float timeToWait = 10f;        // �ð� ���ߴ� ���� (��)
    private float timeCounter;             // Ÿ�̸� ī����

    // CropFactory���� �Ѱܹ��� CropAttributes ����
    public void SetCropAttributes(CropAttributes attributes)
    {
        cropAttributes = attributes;
        currentGrowthTime = 0f;  // �ʱ�ȭ
        waterCount = 0;          // �� �� Ƚ�� �ʱ�ȭ
        isFullyGrown = false;    // ���� ������ �ڶ��� ����
        isDead = false;          // ���� ����
        currentCropObject = Instantiate(cropAttributes.seedPrefab, transform.position, Quaternion.identity);
        timeCounter = 0f;        // Ÿ�̸� �ʱ�ȭ
    }

    void Update()
    {
        if (waterCount > 0 && !isDead)
        {
            // �ð��� �帣�� �ִ� ���
            if (timeCounter < timeToWait)
            {
                timeCounter += Time.deltaTime;  // Ÿ�̸� ����
            }
            else
            {
                currentGrowthTime += Time.deltaTime;  // �ð��� �帣�� ����
                CheckGrowthStage();
                timeCounter = 0f;  // Ÿ�̸� �ʱ�ȭ
            }
        }
    }

    void CheckGrowthStage()
    {
        int growthStage = Mathf.FloorToInt((currentGrowthTime / cropAttributes.growthTime) * cropAttributes.growthStages);
        Debug.Log($"Current Growth Stage: {growthStage}");  // ���� ���� �ܰ� ���

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
        // ���� ������Ʈ�� ������ ����
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
        if (!isDead)  // �۹��� �׾��� ��� ���� �� �� ������
        {
            waterCount++;  // ���� �� Ƚ�� ����

            if (waterCount == 1)
            {
                // ���� ���¿��� ù ��° ���� ��
                currentGrowthTime = 0f;  // ù ���� ���� �������� �ð� ����
            }
            else if (waterCount == 2)
            {
                // �� ��° ���� ���� ��
            }
            else if (waterCount == 3)
            {
                // �� ��° ���� �߰� �ܰ迡�� �ִ� ������ ��
            }
            else if (waterCount >= 4)
            {
                // �� ��° ���� �ָ� �۹��� ����
                Die();
            }
        }
    }

    // �۹��� ���� ���·� ó���ϴ� �޼���
    void Die()
    {
        isDead = true;
        if (currentCropObject != null)
        {
            Destroy(currentCropObject);  // ���� �۹� ������Ʈ ����
        }

        // ���� ���¿� �´� �������� ǥ���� ���� ���� (��: �õ� �۹� �̹���)
        // ����� �׳� ���� ���·� ó��
        Debug.Log("�۹��� �׾����ϴ�.");
    }
}
