using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);  // �� �ε�
        SceneLoader.LoadSceneWithLoading(sceneName);
    }
}
