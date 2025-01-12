using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropStateHandler : MonoBehaviour
{
    public Crop crop;

    private void Update()
    {
        // 'W' 키를 눌러서 물을 주는 테스트
        if (Input.GetKeyDown(KeyCode.W))
        {
            crop.WaterCrop();
        }
    }
}
