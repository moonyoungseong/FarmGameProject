using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NaDataResetter : MonoBehaviour
{
    //[SerializeField] private CharacterSelection characterSelection; // �̸� ���� �Լ� �ִ� ��ũ��Ʈ
    public TMP_InputField ChangeNameInputField;  // TMP_InputField
    public TMP_Text MainNameText;  // ���� ȭ�鿡 �ִ� �̸� Text


    public void OnChangeNameButtonClicked()
    {
        // �̱��� �ν��Ͻ��� ���� ����
        //CharacterSelection.Instance.SaveCharacterName();

        string changeName = ChangeNameInputField.text;  // �Էµ� �̸� ��������
        if (!string.IsNullOrEmpty(changeName))
        {
            PlayerPrefs.SetString("CharacterName", changeName);  // PlayerPrefs�� ����
            PlayerPrefs.Save();  // ���� ���� ����

            // �Էµ� �̸��� UI�� �ٷ� �ݿ�
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
        Debug.Log("��� �����Ͱ� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
