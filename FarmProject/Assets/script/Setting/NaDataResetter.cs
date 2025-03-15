using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NaDataResetter : MonoBehaviour
{
    //[SerializeField] private CharacterSelection characterSelection; // 이름 짓는 함수 있는 스크립트
    public TMP_InputField ChangeNameInputField;  // TMP_InputField
    public TMP_Text MainNameText;  // 메인 화면에 있는 이름 Text


    public void OnChangeNameButtonClicked()
    {
        // 싱글톤 인스턴스를 통해 접근
        //CharacterSelection.Instance.SaveCharacterName();

        string changeName = ChangeNameInputField.text;  // 입력된 이름 가져오기
        if (!string.IsNullOrEmpty(changeName))
        {
            PlayerPrefs.SetString("CharacterName", changeName);  // PlayerPrefs에 저장
            PlayerPrefs.Save();  // 저장 강제 실행

            // 입력된 이름을 UI에 바로 반영
            MainNameText.text = changeName;
        }
        else
        {
            Debug.LogWarning("Character name is empty!");
        }
    }

    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("모든 데이터가 초기화되었습니다.");
    }
}
