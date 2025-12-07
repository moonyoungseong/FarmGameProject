using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CropFactory.cs
/// ScriptableObject로 정의된 CropAttributes를 기반으로
/// 씬에 Crop 오브젝트를 생성하고 초기화하는 팩토리 클래스
/// </summary>
public class CropFactory : MonoBehaviour
{
    /// <summary>
    /// Crop 생성 메서드
    /// </summary>
    /// <param name="cropAttributes">생성할 작물 속성 데이터</param>
    /// <param name="position">씬에서 생성 위치</param>
    /// <returns>생성된 Crop GameObject</returns>
    public GameObject CreateCrop(CropAttributes cropAttributes, Vector3 position)
    {
        // 초기 상태(씨앗 상태) 프리팹으로 Crop 오브젝트 생성
        GameObject cropObject = Instantiate(cropAttributes.seedPrefab, position, Quaternion.identity);

        // Crop 컴포넌트를 가져오거나 없으면 추가
        Crop crop = cropObject.GetComponent<Crop>();
        if (crop == null)
        {
            crop = cropObject.AddComponent<Crop>();
        }

        // CropStateHandler 컴포넌트를 가져오거나 없으면 추가
        CropStateHandler cropStateHandler = cropObject.GetComponent<CropStateHandler>();
        if (cropStateHandler == null)
        {
            cropStateHandler = cropObject.AddComponent<CropStateHandler>();
        }

        // CropStateHandler에 Crop 참조 할당
        cropStateHandler.AssignCrop(crop);

        // CropManager 추가 및 속성 설정
        cropObject.AddComponent<CropManager>().SetCropAttributes(cropAttributes);

        // 초기화 완료된 Crop 오브젝트 반환
        return cropObject;
    }
}
