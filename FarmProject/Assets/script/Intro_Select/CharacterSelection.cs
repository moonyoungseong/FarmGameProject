using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public GameObject maleCharacter;  // ���� ĳ���� ������Ʈ
    public GameObject femaleCharacter; // ���� ĳ���� ������Ʈ
    public Animator animator;         // ĳ������ �ִϸ�����
    public TMP_InputField characterNameInputField;  // TMP_InputField

    // ���� ĳ���� ����
    public void ShowMaleCharacter()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("CharacterName", "")))
        {
            SaveCharacterName(); // �̸��� ���� ����
        }

        animator.SetTrigger("SwitchAnimation");  // �ִϸ��̼� Ʈ���� ����
        maleCharacter.SetActive(true);  // ���� ĳ���� Ȱ��ȭ
        femaleCharacter.SetActive(false);  // ���� ĳ���� ��Ȱ��ȭ
        PlayerPrefs.SetString("SelectedCharacter", "MaleCharacter");  // ������ ĳ���� ����
        PlayerPrefs.Save();
    }

    // ���� ĳ���� ����
    public void ShowFemaleCharacter()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("CharacterName", "")))
        {
            SaveCharacterName(); // �̸��� ���� ����
        }

        animator.SetTrigger("SwitchAnimation");  // �ִϸ��̼� Ʈ���� ����
        maleCharacter.SetActive(false);  // ���� ĳ���� ��Ȱ��ȭ
        femaleCharacter.SetActive(true);  // ���� ĳ���� Ȱ��ȭ
        PlayerPrefs.SetString("SelectedCharacter", "FemaleCharacter");  // ������ ĳ���� ����
        PlayerPrefs.Save();
    }

    // ĳ���� �̸� ����
    public void SaveCharacterName()
    {
        string characterName = characterNameInputField.text;  // �Էµ� �̸� ��������
        if (!string.IsNullOrEmpty(characterName))
        {
            PlayerPrefs.SetString("CharacterName", characterName);  // PlayerPrefs�� ����
            PlayerPrefs.Save();  // ���� ���� ����
        }
        else
        {
            Debug.LogWarning("Character name is empty!");
        }
    }

    // �� ��ȯ �Լ�
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);  // �� �ε�
    }
}
