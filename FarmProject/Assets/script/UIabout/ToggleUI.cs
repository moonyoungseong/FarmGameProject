using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject targetUI;  // ���� ���� UI ������Ʈ
    private bool isUIActive = false;  // UI�� Ȱ��ȭ �Ǿ� �ִ��� ����

    // ��ư Ŭ�� �� ȣ��� �Լ�
    public void ToggleUIVisibility()
    {
        isUIActive = !isUIActive;  // UI Ȱ��ȭ ���¸� ����
        targetUI.SetActive(isUIActive);  // UI Ȱ��ȭ/��Ȱ��ȭ
    }
}
