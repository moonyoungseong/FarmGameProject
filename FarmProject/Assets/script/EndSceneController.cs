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
    [Header("ĳ���ͺ� Playable Director")]
    public PlayableDirector maleDirector;
    public PlayableDirector femaleDirector;

    [Header("���� ���� Ÿ�Ӷ���")]
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
            Debug.LogError("[EndSceneController] Director �Ǵ� Timeline�� ����Ǿ� ���� �ʽ��ϴ�.");
            return;
        }

        targetDirector.playableAsset = targetTimeline;
        targetDirector.Play();

        Debug.Log($"[EndSceneController] {selectedCharacter} ���� {selectedEnding} ��� ����");
    }
}
