using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crop.cs
/// 플레이어가 심은 작물의 확인 및 물 주기
/// </summary>
public class Crop : MonoBehaviour
{
    // 이 Crop 오브젝트와 연결된 CropManager 컴포넌트 참조
    private CropManager cropManager;

    private void Start()
    {
        // 같은 게임 오브젝트에 있는 CropManager 컴포넌트를 찾아서 연결
        cropManager = GetComponent<CropManager>();
    }

    // 작물에 물을 주는 메서드
    public void WaterCrop()
    {
        if (cropManager != null)
        {
            cropManager.WaterCrop(); // 실제 CropManager 쪽에서 물 주는 처리 수행
        }
    }
}
