using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoad : MonoBehaviour
{
    public Image loadingBar;
    public TextMeshProUGUI loadText;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //  ���� �������� �̵��� �� ��������
        string targetScene = SceneLoader.nextSceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            if (loadingBar.fillAmount < 0.9f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 1f, Time.deltaTime);
            }

            if (loadingBar.fillAmount >= 1f)
            {
                loadText.text = "�����̽� ������";
            }

            if (Input.GetKeyDown(KeyCode.Space) && loadingBar.fillAmount >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}
