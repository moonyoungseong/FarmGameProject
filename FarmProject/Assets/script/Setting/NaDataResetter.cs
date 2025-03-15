using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NaDataResetter : MonoBehaviour
{
    //[SerializeField] private CharacterSelection characterSelection; // �̸� ���� �Լ� �ִ� ��ũ��Ʈ
    public TMP_InputField ChangeNameInputField;  // TMP_InputField

    public void OnChangeNameButtonClicked()
    {
        // �̱��� �ν��Ͻ��� ���� ����
        //CharacterSelection.Instance.SaveCharacterName();

        string chageName = ChangeNameInputField.text;  // �Էµ� �̸� ��������
        if (!string.IsNullOrEmpty(chageName))
        {
            PlayerPrefs.SetString("CharacterName", chageName);  // PlayerPrefs�� ����
            PlayerPrefs.Save();  // ���� ���� ����
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
        Debug.Log("��� �����Ͱ� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
