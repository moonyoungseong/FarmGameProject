using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static string nextSceneName; // �̵��� �� ����

    public static void LoadSceneWithLoading(string sceneName)
    {
        nextSceneName = sceneName; // ���� �� ����
        SceneManager.LoadScene("LoadScene"); // �ε� �� ����
    }
}