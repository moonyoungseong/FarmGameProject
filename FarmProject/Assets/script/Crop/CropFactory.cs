using UnityEngine;

public class CropFactory : MonoBehaviour
{
    public GameObject CreateCrop(CropAttributes cropAttributes, Vector3 position)
    {
        // 작물의 초기 상태 (씨앗 상태 프리팹)로 게임 오브젝트 생성
        GameObject cropObject = Instantiate(cropAttributes.seedPrefab, position, Quaternion.identity);

        // Crop 컴포넌트를 가져오거나 추가
        Crop crop = cropObject.GetComponent<Crop>();
        if (crop == null)
        {
            crop = cropObject.AddComponent<Crop>();
        }

        // CropStateHandler 컴포넌트를 가져오거나 추가
        CropStateHandler cropStateHandler = cropObject.GetComponent<CropStateHandler>();
        if (cropStateHandler == null)
        {
            cropStateHandler = cropObject.AddComponent<CropStateHandler>();
        }

        // cropStateHandler에 crop을 할당
        cropStateHandler.AssignCrop(crop);

        // 필요하다면 추가 초기화 작업을 진행할 수 있음
        cropObject.AddComponent<CropManager>().SetCropAttributes(cropAttributes);

        return cropObject;
    }
}
