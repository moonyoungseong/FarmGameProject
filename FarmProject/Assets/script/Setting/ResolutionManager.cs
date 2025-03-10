using UnityEngine;

/// <summary>
/// �ػ� ���� �ڵ尡 ���� �۵��� ���ϰ� �ִ���
/// ���߿� �׽�Ʈ �غ��� ���濹��
/// </summary>

public class ResolutionManager : MonoBehaviour
{
    public void SetFullHD()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        Debug.Log("�ػ󵵸� Full HD(1920x1080)�� ����");
    }

    public void SetHD()
    {
        //Screen.SetResolution(1366, 768, FullScreenMode.FullScreenWindow);
        Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
        Debug.Log("�ػ󵵸� HD(1280x720)�� ����");
    }

    public void sethdplus()
    {
        Screen.SetResolution(1600, 900, FullScreenMode.FullScreenWindow);
        Debug.Log("�ػ󵵸� hd+ (1600x900)�� ����");
    }
}
