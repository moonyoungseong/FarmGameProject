using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoEnding : MonoBehaviour
{
    [Header("���� �г�")]
    public GameObject endingPanel;

    [Header("���� ����")]
    public int goldThreshold = 100;

    private void Start()
    {
        if (endingPanel != null)
            endingPanel.SetActive(false);

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += CheckGold;
            CheckGold(GoldManager.Instance.GetGold());
        }
    }

    private void CheckGold(int currentGold)
    {
        if (currentGold >= goldThreshold && endingPanel != null)
        {
            endingPanel.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= CheckGold;
    }

    public void OnClickEndingAButton1()
    {
        EndingChoice.SelectedEnding = EndingType.EndingA;
        //LoadScene("EndScene");
    }

    public void OnClickEndingBButton2()
    {
        EndingChoice.SelectedEnding = EndingType.EndingB;
        //LoadScene("EndScene");
    }

    // �� �̵��� ���� �Լ� (Coroutine)
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
        else
        {
            Debug.LogWarning("[GoEnding] LoadScene ȣ�� �� �� �̸��� ����ֽ��ϴ�.");
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // �� �ε�
        SceneManager.LoadScene(sceneName);

        // 1������ ��� �� ī�޶�/���� �ʱ�ȭ
        yield return null;

        // ������ Brightness�� Post Processing�� �ٷ� ����ǵ��� ī�޶� ���� Ȱ��ȭ
        var cam = FindObjectOfType<Camera>();
        if (cam != null)
        {
            cam.enabled = false;
            cam.enabled = true;
        }

        // �ʿ��ϸ� Cinemachine Virtual Camera�� ������ Ȱ��ȭ
        var vCam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (vCam != null)
            vCam.gameObject.SetActive(false);
        if (vCam != null)
            vCam.gameObject.SetActive(true);
    }
}
