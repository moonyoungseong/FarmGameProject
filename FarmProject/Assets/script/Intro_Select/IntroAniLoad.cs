using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class IntroAniLoad : MonoBehaviour
{
    public PlayableDirector playableDirector;  // 타임라인의 PlayableDirector
    public GameObject maleCharacter;  // 남자 캐릭터 오브젝트
    public GameObject femaleCharacter; // 여자 캐릭터 오브젝트

    // 타임라인이 끝나면 호출될 이벤트 처리 함수
    void Start()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "MaleCharacter");

        if (selectedCharacter == "MaleCharacter")
        {
            // 남캐 바인딩
            playableDirector.SetGenericBinding(playableDirector.playableAsset.outputs.GetEnumerator().Current.sourceObject, maleCharacter);
            maleCharacter.SetActive(true);
            femaleCharacter.SetActive(false);
        }
        else if (selectedCharacter == "FemaleCharacter")
        {
            // 여캐로 바꾸기 전에 남캐의 위치를 여캐에 복사
            femaleCharacter.transform.position = maleCharacter.transform.position; // 위치 동기화
            femaleCharacter.transform.rotation = maleCharacter.transform.rotation; // 회전 동기화 (필요하면)

            // 여캐 바인딩
            playableDirector.SetGenericBinding(playableDirector.playableAsset.outputs.GetEnumerator().Current.sourceObject, femaleCharacter);
            maleCharacter.SetActive(false);
            femaleCharacter.SetActive(true);
        }

        // 타임라인 재생
        playableDirector.Play();

        // 타임라인이 끝날 때 호출될 이벤트 등록
        playableDirector.stopped += OnTimelineFinished;
    }

    // 타임라인이 끝나면 씬을 전환하는 함수
    void OnTimelineFinished(PlayableDirector director)
    {
        // 타임라인이 끝났을 때 다음 씬으로 전환
        LoadScene("MainScene");  // "NextSceneName"을 원하는 씬 이름으로 바꿔주세요
    }

    // 씬 전환 함수
    void LoadScene(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);  // 씬 로드
        SceneLoader.LoadSceneWithLoading(sceneName);
    }
}
