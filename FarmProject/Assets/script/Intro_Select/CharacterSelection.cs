using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// CharacterSelection.cs
/// 
/// 캐릭터 선택 화면을 관리하는 클래스
/// 캐릭터(남/여) 활성화, 이름 저장, 애니메이션 트리거 등을 처리한다.
/// </summary>
public class CharacterSelection : MonoBehaviour
{
    [Header("Character Objects")]
    public GameObject maleCharacter;
    public GameObject femaleCharacter;

    [Header("Animator")]
    public Animator animator;

    [Header("UI Input")]
    // 캐릭터 이름 입력용
    public TMP_InputField characterNameInputField;

    // 남자 캐릭터를 선택하는 함수.
    public void ShowMaleCharacter()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("CharacterName", "")))
        {
            SaveCharacterName();    // 이름 저장
        }

        animator.SetTrigger("SwitchAnimation");
        maleCharacter.SetActive(true);
        femaleCharacter.SetActive(false);

        PlayerPrefs.SetString("SelectedCharacter", "MaleCharacter");
        PlayerPrefs.Save();
    }

    // 여자 캐릭터를 선택하는 함수.
    public void ShowFemaleCharacter()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("CharacterName", "")))
        {
            SaveCharacterName();    // 이름 저장
        }

        animator.SetTrigger("SwitchAnimation");
        maleCharacter.SetActive(false);
        femaleCharacter.SetActive(true);

        PlayerPrefs.SetString("SelectedCharacter", "FemaleCharacter");
        PlayerPrefs.Save();
    }


    /// <summary>
    /// 캐릭터 이름을 PlayerPrefs에 저장한다.
    /// 빈 문자열이면 저장하지 않고 Warning 출력.
    /// </summary>
    public void SaveCharacterName()
    {
        string characterName = characterNameInputField.text;

        if (!string.IsNullOrEmpty(characterName))
        {
            PlayerPrefs.SetString("CharacterName", characterName);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("Character name is empty!");
        }
    }
}
