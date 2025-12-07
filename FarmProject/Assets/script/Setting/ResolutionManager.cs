using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ResolutionManager.cs
/// 해상도 변경을 관리하는 스크립트.
/// </summary>
public class ResolutionManager : MonoBehaviour
{
    /// <summary>
    /// 해상도를 Full HD로 변경하는 함수.
    /// FullScreenWindow 모드로 적용된다.
    /// </summary>
    public void SetFullHD()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }

    /// <summary>
    /// 해상도를 HD로 변경하는 함수.
    /// 낮은 사양 또는 창 모드 테스트할 때 사용.
    /// </summary>
    public void SetHD()
    {
        Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
    }

    /// <summary>
    /// 해상도를 HD+로 변경하는 함수.
    /// </summary>
    public void sethdplus()
    {
        Screen.SetResolution(1600, 900, FullScreenMode.FullScreenWindow);
    }
}
