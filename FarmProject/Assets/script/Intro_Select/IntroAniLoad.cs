using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class IntroAniLoad : MonoBehaviour
{
    public PlayableDirector playableDirector;  // Ÿ�Ӷ����� PlayableDirector
    public GameObject maleCharacter;  // ���� ĳ���� ������Ʈ
    public GameObject femaleCharacter; // ���� ĳ���� ������Ʈ

    // Ÿ�Ӷ����� ������ ȣ��� �̺�Ʈ ó�� �Լ�
    void Start()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "MaleCharacter");

        if (selectedCharacter == "MaleCharacter")
        {
            // ��ĳ ���ε�
            playableDirector.SetGenericBinding(playableDirector.playableAsset.outputs.GetEnumerator().Current.sourceObject, maleCharacter);
            maleCharacter.SetActive(true);
            femaleCharacter.SetActive(false);
        }
        else if (selectedCharacter == "FemaleCharacter")
        {
            // ��ĳ�� �ٲٱ� ���� ��ĳ�� ��ġ�� ��ĳ�� ����
            femaleCharacter.transform.position = maleCharacter.transform.position; // ��ġ ����ȭ
            femaleCharacter.transform.rotation = maleCharacter.transform.rotation; // ȸ�� ����ȭ (�ʿ��ϸ�)

            // ��ĳ ���ε�
            playableDirector.SetGenericBinding(playableDirector.playableAsset.outputs.GetEnumerator().Current.sourceObject, femaleCharacter);
            maleCharacter.SetActive(false);
            femaleCharacter.SetActive(true);
        }

        // Ÿ�Ӷ��� ���
        playableDirector.Play();

        // Ÿ�Ӷ����� ���� �� ȣ��� �̺�Ʈ ���
        playableDirector.stopped += OnTimelineFinished;
    }

    // Ÿ�Ӷ����� ������ ���� ��ȯ�ϴ� �Լ�
    void OnTimelineFinished(PlayableDirector director)
    {
        // Ÿ�Ӷ����� ������ �� ���� ������ ��ȯ
        LoadScene("MainScene");  // "NextSceneName"�� ���ϴ� �� �̸����� �ٲ��ּ���
    }

    // �� ��ȯ �Լ�
    void LoadScene(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);  // �� �ε�
        SceneLoader.LoadSceneWithLoading(sceneName);
    }
}
