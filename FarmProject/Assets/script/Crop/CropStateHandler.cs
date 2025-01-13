using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropStateHandler : MonoBehaviour
{
    private Crop crop;  // crop ����

    // crop �Ҵ� �޼���
    public void AssignCrop(Crop newCrop)
    {
        crop = newCrop;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && crop != null)
        {
            crop.WaterCrop();
        }
    }
}

