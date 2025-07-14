using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeySceneLoader : MonoBehaviour
{
    public string nextSceneName;  // Inspector에서 이동할 씬 이름 지정

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}