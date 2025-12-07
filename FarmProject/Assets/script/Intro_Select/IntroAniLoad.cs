using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

/// <summary>
/// IntroAniLoad.cs
/// 
/// 인트로 타임라인을 실행하고,
/// 저장된 캐릭터 선택값에 따라 알맞은 캐릭터를 타임라인에 바인딩한 뒤
/// 타임라인 종료 시 메인 씬으로 이동하는 클래스.
/// </summary>
public class IntroAniLoad : MonoBehaviour
{
    // 인트로 타임라인의 PlayableDirector
    public PlayableDirector playableDirector;

    public GameObject maleCharacter;
    public GameObject femaleCharacter;

    // PlayerPrefs에서 선택된 캐릭터 불러오기, 캐릭터 바인딩, 재생
    void Start()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "MaleCharacter");

        if (selectedCharacter == "MaleCharacter")
        {
            // 남자 캐릭터 바인딩
            playableDirector.SetGenericBinding(
                playableDirector.playableAsset.outputs.GetEnumerator().Current.sourceObject,
                maleCharacter
            );

            maleCharacter.SetActive(true);
            femaleCharacter.SetActive(false);
        }
        else if (selectedCharacter == "FemaleCharacter")
        {
            // 여캐 활성화 전 위치 동기화
            femaleCharacter.transform.position = maleCharacter.transform.position;
            femaleCharacter.transform.rotation = maleCharacter.transform.rotation;

            // 여자 캐릭터 바인딩
            playableDirector.SetGenericBinding(
                playableDirector.playableAsset.outputs.GetEnumerator().Current.sourceObject,
                femaleCharacter
            );

            maleCharacter.SetActive(false);
            femaleCharacter.SetActive(true);
        }

        playableDirector.Play();
        playableDirector.stopped += OnTimelineFinished;
    }


    /// <summary>
    /// 타임라인 재생이 종료되면 자동 호출되는 함수.
    /// 다음 씬(MainScene)으로 이동한다.
    /// </summary>
    void OnTimelineFinished(PlayableDirector director)
    {
        LoadScene("MainScene");
    }

    // 로딩 화면을 포함한 씬 전환 처리.
    void LoadScene(string sceneName)
    {
        SceneLoader.LoadSceneWithLoading(sceneName);
    }
}
