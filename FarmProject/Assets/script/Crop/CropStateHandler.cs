using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropStateHandler : MonoBehaviour
{
    public Crop crop;

    private void Update()
    {
        // 'W' Ű�� ������ ���� �ִ� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.W))
        {
            crop.WaterCrop();
        }
    }
}
