using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// SceneLoad.cs
/// 로딩 씬에서 랜덤 배경 이미지와 팁 문구를 보여주고  
/// 다음 씬을 비동기로 로드하는 기능을 담당.
/// 
/// - SceneLoader.nextSceneName 값을 읽어 다음 씬 결정
/// - 로딩 진행도(0~100%) UI로 표시
/// - 로딩 완료 후 스페이스 키 입력 시 실제 씬 전환
/// </summary>
public class SceneLoad : MonoBehaviour
{
    public Sprite[] backgroundImages;      // 로딩 화면에서 랜덤으로 보여줄 배경 이미지들
    public string[] tipMessages;           // 각 이미지에 대응하는 팁 문구 리스트
    public Image loadingBackground;        // 로딩 화면 배경 이미지 UI
    public TextMeshProUGUI tipText;        // 배경 이미지에 대응하는 팁 텍스트 UI

    public Image loadingBar;               // 로딩 진행 상태를 시각적으로 표시하는 바
    public TextMeshProUGUI loadText;       // 로딩 퍼센트 숫자 표시
    public TextMeshProUGUI anyKeyText;     // "스페이스 키 누르세요" 안내 텍스트

    /// 시작 시 랜덤 이미지/텍스트를 보여주고 로딩 코루틴 시작
    private void Start()
    {
        ShowRandomTipAndImage();
        StartCoroutine(LoadScene());
    }

    /// <summary>
    /// 배경 이미지 배열에서 랜덤으로 하나 선택하고  
    /// 그 이미지에 해당하는 팁 메시지 출력.
    /// </summary>
    void ShowRandomTipAndImage()
    {
        int randomIndex = Random.Range(0, backgroundImages.Length);
        loadingBackground.sprite = backgroundImages[randomIndex];
        tipText.text = tipMessages[randomIndex];
    }

    /// <summary>
    /// 다음 씬을 비동기로 로드하며 로딩 바 UI를 갱신한다.
    /// 로딩이 100%에 도달하면 스페이스 입력 시 씬 활성화
    /// </summary>
    IEnumerator LoadScene()
    {
        yield return null;

        // SceneLoader에 저장되어 있는 다음 씬 이름 로드
        string targetScene = SceneLoader.nextSceneName;

        // 비동기 씬 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            // 로딩 바 진행 업데이트
            if (loadingBar.fillAmount < 0.9f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 1f, Time.deltaTime);
            }

            // UI 퍼센트 표시
            int progressPercentage = Mathf.FloorToInt(loadingBar.fillAmount * 100);
            loadText.text = progressPercentage + "%";

            // 로딩 완료 시 안내 문구 출력
            if (loadingBar.fillAmount >= 1f)
            {
                anyKeyText.text = "스페이스 키 누르세요";
                loadText.text = "100%";
            }

            // 스페이스 키 입력 시 씬 활성화
            if (Input.GetKeyDown(KeyCode.Space) && loadingBar.fillAmount >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}
