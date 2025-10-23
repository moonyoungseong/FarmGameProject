using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public enum EndingType
{
    EndingA,
    EndingB
}

public class EndSceneController : MonoBehaviour
{
    [Header("캐릭터별 Playable Director")]
    public PlayableDirector maleDirector;
    public PlayableDirector femaleDirector;

    [Header("공용 엔딩 타임라인")]
    public PlayableAsset endingA_Timeline;
    public PlayableAsset endingB_Timeline;

    private void Start()
    {
        PlaySelectedEnding();
    }

    private void PlaySelectedEnding()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "MaleCharacter");
        EndingType selectedEnding = EndingChoice.SelectedEnding;

        PlayableDirector targetDirector = (selectedCharacter == "MaleCharacter") ? maleDirector : femaleDirector;

        if (maleDirector != null) maleDirector.gameObject.SetActive(selectedCharacter == "MaleCharacter");
        if (femaleDirector != null) femaleDirector.gameObject.SetActive(selectedCharacter == "FemaleCharacter");

        PlayableAsset targetTimeline = (selectedEnding == EndingType.EndingA) ? endingA_Timeline : endingB_Timeline;

        if (targetDirector == null || targetTimeline == null)
        {
            Debug.LogError("[EndSceneController] Director 또는 Timeline이 연결되어 있지 않습니다.");
            return;
        }

        targetDirector.playableAsset = targetTimeline;
        targetDirector.Play();

        Debug.Log($"[EndSceneController] {selectedCharacter} 엔딩 {selectedEnding} 재생 시작");
    }
}
