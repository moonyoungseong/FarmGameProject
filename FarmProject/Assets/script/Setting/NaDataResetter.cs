using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaDataResetter : MonoBehaviour
{
    //[SerializeField] private CharacterSelection characterSelection; // 이름 짓는 함수 있는 스크립트

    public void OnChangeNameButtonClicked()
    {
        // 싱글톤 인스턴스를 통해 접근
        //CharacterSelection.Instance.SaveCharacterName();
    }

    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("모든 데이터가 초기화되었습니다.");
    }
}
