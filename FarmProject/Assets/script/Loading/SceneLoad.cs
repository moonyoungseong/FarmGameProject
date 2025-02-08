using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoad : MonoBehaviour
{
    public Image loadingBar;
    public TextMeshProUGUI loadText;
    public TextMeshProUGUI anyKeyText; // 키 누르라는 글자

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        // 전역 변수에서 이동할 씬 가져오기
        string targetScene = SceneLoader.nextSceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            // 로딩 바 진행 상태 업데이트
            if (loadingBar.fillAmount < 0.9f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 1f, Time.deltaTime);
            }

            // 진행 상태를 숫자로 표시
            int progressPercentage = Mathf.FloorToInt(loadingBar.fillAmount * 100); // 퍼센트로 변환
            loadText.text = progressPercentage + "%"; // 숫자 형태로 표시

            // 로딩 바가 100%에 도달하면 텍스트 변경
            if (loadingBar.fillAmount >= 1f)
            {
                anyKeyText.text = "아무키나 누르세요";
                loadText.text = "100%";
            }

            // 스페이스 키를 누르면 씬을 활성화
            if (Input.GetKeyDown(KeyCode.Space) && loadingBar.fillAmount >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}
