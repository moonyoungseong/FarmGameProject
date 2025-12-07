using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneLoader.cs
/// 씬 전환을 전담하는 클래스
/// 
/// - 게임 어디에서든 로딩 씬을 거쳐 다른 씬으로 이동할 수 있게 함
/// - LoadSceneWithLoading() 호출 시 nextSceneName 저장 후
///   "LoadScene" 씬을 먼저 호출
/// - 로딩 씬(SceneLoad.cs)이 nextSceneName 을 읽어 실제 타겟 씬을 로드함
/// "중간 로딩 화면을 거쳐 씬 이동"을 구현하는 핵심 유틸리티.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static string nextSceneName; 

    /// <summary>
    /// 로딩 씬을 통해 특정 씬으로 이동
    /// - 실질적인 로딩은 LoadScene 씬에서 처리됨(SceneLoad.cs)
    /// </summary>
    public static void LoadSceneWithLoading(string sceneName)
    {
        nextSceneName = sceneName; // 이동할 씬 저장
        SceneManager.LoadScene("LoadScene"); // 로딩 씬 실행
    }
}
