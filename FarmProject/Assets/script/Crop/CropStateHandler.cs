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
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.E) && crop != null) // Ư��Ű�� ������ �� �� �ֱ�
        {
            crop.WaterCrop();
        }
    }

    //public void WaterCrop()     // �� �Լ� ������ �� �ȵȴ�. �ϴ� �ӽ÷� �س��� ���߿� ������� ����.
    //{
    //    if( crop != null)
    //    {
    //        crop.WaterCrop();
    //    }
    //}
}