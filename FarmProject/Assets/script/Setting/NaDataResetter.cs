using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaDataResetter : MonoBehaviour
{
    //[SerializeField] private CharacterSelection characterSelection; // �̸� ���� �Լ� �ִ� ��ũ��Ʈ

    public void OnChangeNameButtonClicked()
    {
        // �̱��� �ν��Ͻ��� ���� ����
        //CharacterSelection.Instance.SaveCharacterName();
    }

    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("��� �����Ͱ� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
