using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneMove.cs
/// 버튼 클릭 등 UI 이벤트에서 간단하게 씬 이동을 호출하기 위한 스크립트
/// 
/// - 실제 로딩 처리 및 전환은 SceneLoader.cs 에서 담당
/// - 이 스크립트는 "씬 이동 버튼"에 붙여서 사용
/// </summary>
public class SceneMove : MonoBehaviour
{
    // 외부에서 호출되는 씬 이동 함수
    public void LoadScene(string sceneName)
    {
        SceneLoader.LoadSceneWithLoading(sceneName);
    }
}
