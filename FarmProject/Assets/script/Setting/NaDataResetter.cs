using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// NaDataResetter.cs
/// 플레이어 이름 변경 및 저장 / 전체 데이터 초기화 기능을 담당하는 스크립트.
/// PlayerPrefs를 사용하여 이름을 저장하고, 입력된 이름을 UI에 반영한다.
/// </summary>
public class NaDataResetter : MonoBehaviour
{
    // 변경 이름 입력란
    public TMP_InputField ChangeNameInputField;

    // 표시되는 플레이어 이름 UI
    public TMP_Text MainNameText;

    /// <summary>
    /// 이름 변경 버튼을 눌렀을 때 실행되는 함수.
    /// - InputField에 입력된 이름을 받아 PlayerPrefs에 저장
    /// - 저장된 이름을 메인 UI 텍스트에 반영
    /// </summary>
    public void OnChangeNameButtonClicked()
    {
        string changeName = ChangeNameInputField.text;  // 입력된 이름 가져오기

        if (!string.IsNullOrEmpty(changeName))
        {
            PlayerPrefs.SetString("CharacterName", changeName);  // 이름 저장
            PlayerPrefs.Save(); // 즉시 저장

            MainNameText.text = changeName;   // UI에 반영
        }
        else
        {
            Debug.LogWarning("Character name is empty!");
        }
    }

    // 모든 PlayerPrefs 데이터를 초기화하는 함수.
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();  // 전체 데이터 삭제
        PlayerPrefs.Save();        // 즉시 저장
    }
}
