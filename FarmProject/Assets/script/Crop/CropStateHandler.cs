using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropStateHandler : MonoBehaviour
{
    private Crop crop;  // crop 변수

    // crop 할당 메서드
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

    //public void WaterCrop()     // 이 함수 실행이 잘 안된다. 일단 임시로 해놓고 나중에 지우든지 하자.
    //{
    //    if( crop != null)
    //    {
    //        crop.WaterCrop();
    //    }
    //}
}