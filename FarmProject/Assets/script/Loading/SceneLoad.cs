using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoad : MonoBehaviour
{
    public Image loadingBar;
    public TextMeshProUGUI loadText;
    public TextMeshProUGUI anyKeyText; // Ű ������� ����

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        // ���� �������� �̵��� �� ��������
        string targetScene = SceneLoader.nextSceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            // �ε� �� ���� ���� ������Ʈ
            if (loadingBar.fillAmount < 0.9f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, 1f, Time.deltaTime);
            }

            // ���� ���¸� ���ڷ� ǥ��
            int progressPercentage = Mathf.FloorToInt(loadingBar.fillAmount * 100); // �ۼ�Ʈ�� ��ȯ
            loadText.text = progressPercentage + "%"; // ���� ���·� ǥ��

            // �ε� �ٰ� 100%�� �����ϸ� �ؽ�Ʈ ����
            if (loadingBar.fillAmount >= 1f)
            {
                anyKeyText.text = "�ƹ�Ű�� ��������";
                loadText.text = "100%";
            }

            // �����̽� Ű�� ������ ���� Ȱ��ȭ
            if (Input.GetKeyDown(KeyCode.Space) && loadingBar.fillAmount >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}
