using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// 엔딩 종류를 구분하는 열거형
public enum EndingType
{
    EndingA,    // 머물기
    EndingB     // 돌아가기
}

// 엔딩 장면에서 선택된 캐릭터와 엔딩 타입에 따라 올바른 타임라인을 재생하는 컨트롤러
public class EndSceneController : MonoBehaviour
{
    [Header("캐릭터별 Playable Director")]
    public PlayableDirector maleDirector;
    public PlayableDirector femaleDirector;

    [Header("공용 엔딩 타임라인")]
    // Ending A에 사용되는 공용 타임라인 
    public PlayableAsset endingA_Timeline;

    // Ending B에 사용되는 공용 타임라인 
    public PlayableAsset endingB_Timeline;

    // 씬 시작 시 자동으로 선택된 엔딩을 재생한다.
    private void Start()
    {
        PlaySelectedEnding();
    }

    /// <summary>
    /// 선택된 캐릭터와 엔딩 타입에 맞춰 해당 타임라인을 재생한다.
    /// PlayerPrefs로 저장된 캐릭터(MaleCharacter/FemaleCharacter)를 확인하고,
    /// EndingChoice.SelectedEnding 값을 기반으로 엔딩을 결정한다.
    /// </summary>
    private void PlaySelectedEnding()
    {
        // PlayerPrefs에서 선택된 캐릭터 불러오기 (기본값: MaleCharacter)
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "MaleCharacter");

        // 외부 클래스에서 저장한 엔딩 선택 값 불러오기
        EndingType selectedEnding = EndingChoice.SelectedEnding;

        // 선택된 캐릭터에 맞는 Director 할당
        PlayableDirector targetDirector = (selectedCharacter == "MaleCharacter") ? maleDirector : femaleDirector;

        // 활성화해야 하는 Director만 켜기
        if (maleDirector != null)
            maleDirector.gameObject.SetActive(selectedCharacter == "MaleCharacter");

        if (femaleDirector != null)
            femaleDirector.gameObject.SetActive(selectedCharacter == "FemaleCharacter");

        // 엔딩 타입에 따른 타임라인 선택
        PlayableAsset targetTimeline = (selectedEnding == EndingType.EndingA) ? endingA_Timeline : endingB_Timeline;

        // 예외 처리: 값이 비어있으면 로그 출력하고 종료
        if (targetDirector == null || targetTimeline == null)
        {
            Debug.LogError("[EndSceneController] Director 또는 Timeline이 연결되어 있지 않습니다.");
            return;
        }

        // 타임라인 재생
        targetDirector.playableAsset = targetTimeline;
        targetDirector.Play();
    }
}
