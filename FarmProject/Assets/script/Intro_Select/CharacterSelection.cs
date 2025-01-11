using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public GameObject maleCharacter;  // 남자 캐릭터 오브젝트
    public GameObject femaleCharacter; // 여자 캐릭터 오브젝트
    public Animator animator;         // 캐릭터의 애니메이터
    public TMP_InputField characterNameInputField;  // TMP_InputField

    // 남자 캐릭터 선택
    public void ShowMaleCharacter()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("CharacterName", "")))
        {
            SaveCharacterName(); // 이름을 먼저 저장
        }

        animator.SetTrigger("SwitchAnimation");  // 애니메이션 트리거 실행
        maleCharacter.SetActive(true);  // 남자 캐릭터 활성화
        femaleCharacter.SetActive(false);  // 여자 캐릭터 비활성화
        PlayerPrefs.SetString("SelectedCharacter", "MaleCharacter");  // 선택한 캐릭터 저장
        PlayerPrefs.Save();
    }

    // 여자 캐릭터 선택
    public void ShowFemaleCharacter()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("CharacterName", "")))
        {
            SaveCharacterName(); // 이름을 먼저 저장
        }

        animator.SetTrigger("SwitchAnimation");  // 애니메이션 트리거 실행
        maleCharacter.SetActive(false);  // 남자 캐릭터 비활성화
        femaleCharacter.SetActive(true);  // 여자 캐릭터 활성화
        PlayerPrefs.SetString("SelectedCharacter", "FemaleCharacter");  // 선택한 캐릭터 저장
        PlayerPrefs.Save();
    }

    // 캐릭터 이름 저장
    public void SaveCharacterName()
    {
        string characterName = characterNameInputField.text;  // 입력된 이름 가져오기
        if (!string.IsNullOrEmpty(characterName))
        {
            PlayerPrefs.SetString("CharacterName", characterName);  // PlayerPrefs에 저장
            PlayerPrefs.Save();  // 저장 강제 실행
        }
        else
        {
            Debug.LogWarning("Character name is empty!");
        }
    }

    // 씬 전환 함수
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);  // 씬 로드
    }
}
