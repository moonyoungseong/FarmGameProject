using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static string nextSceneName; // 이동할 씬 저장

    public static void LoadSceneWithLoading(string sceneName)
    {
        nextSceneName = sceneName; // 다음 씬 저장
        SceneManager.LoadScene("LoadScene"); // 로딩 씬 실행
    }
}