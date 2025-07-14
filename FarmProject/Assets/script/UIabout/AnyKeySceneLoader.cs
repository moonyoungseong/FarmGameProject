using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeySceneLoader : MonoBehaviour
{
    public string nextSceneName;  // Inspector���� �̵��� �� �̸� ����

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}