using UnityEngine;

public class CropFactory : MonoBehaviour
{
    public GameObject CreateCrop(CropAttributes cropAttributes, Vector3 position)
    {
        // �۹��� �ʱ� ���� (���� ���� ������)�� ���� ������Ʈ ����
        GameObject cropObject = Instantiate(cropAttributes.seedPrefab, position, Quaternion.identity);

        // Crop ������Ʈ�� �������ų� �߰�
        Crop crop = cropObject.GetComponent<Crop>();
        if (crop == null)
        {
            crop = cropObject.AddComponent<Crop>();
        }

        // CropStateHandler ������Ʈ�� �������ų� �߰�
        CropStateHandler cropStateHandler = cropObject.GetComponent<CropStateHandler>();
        if (cropStateHandler == null)
        {
            cropStateHandler = cropObject.AddComponent<CropStateHandler>();
        }

        // cropStateHandler�� crop�� �Ҵ�
        cropStateHandler.AssignCrop(crop);

        // �ʿ��ϴٸ� �߰� �ʱ�ȭ �۾��� ������ �� ����
        cropObject.AddComponent<CropManager>().SetCropAttributes(cropAttributes);

        return cropObject;
    }
}
