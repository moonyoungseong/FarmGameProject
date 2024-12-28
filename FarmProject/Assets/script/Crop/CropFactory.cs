using UnityEngine;

public class CropFactory : MonoBehaviour
{
    public GameObject CreateCrop(CropAttributes cropAttributes, Vector3 position)
    {
        // �۹��� �ʱ� ���� (���� ���� ������)�� ���� ������Ʈ ����
        GameObject cropObject = Instantiate(cropAttributes.seedPrefab, position, Quaternion.identity);

        // �ʿ��ϴٸ� �߰� �ʱ�ȭ �۾��� ������ �� ����
        cropObject.AddComponent<CropManager>().SetCropAttributes(cropAttributes);

        return cropObject;
    }
}
