using UnityEngine;

/// <summary>
/// 해상도 변경 코드가 현재 작동은 안하고 있는중
/// 나중에 테스트 해보고 변경예정
/// </summary>

public class ResolutionManager : MonoBehaviour
{
    public void SetFullHD()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        Debug.Log("해상도를 Full HD(1920x1080)로 변경");
    }

    public void SetHD()
    {
        //Screen.SetResolution(1366, 768, FullScreenMode.FullScreenWindow);
        Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
        Debug.Log("해상도를 HD(1280x720)로 변경");
    }

    public void sethdplus()
    {
        Screen.SetResolution(1600, 900, FullScreenMode.FullScreenWindow);
        Debug.Log("해상도를 hd+ (1600x900)로 변경");
    }
}
