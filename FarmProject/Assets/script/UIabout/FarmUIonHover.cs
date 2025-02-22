using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmUIonHover : MonoBehaviour
{
    public LayerMask layer1; // Layer1 ���̾�
    public LayerMask layer2; // Layer2 ���̾�
    public GameObject uiElement; // ���콺�� ������ ��� ǥ���� UI ���

    void Update()
    {
        // ���콺 ��ġ���� ����ĳ��Ʈ�� ��� ������Ʈ�� ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // ����ĳ��Ʈ�� ���� ������Ʈ�� Layer1 �Ǵ� Layer2�� ���ϴ��� Ȯ��
            if (((1 << hit.collider.gameObject.layer) & layer1) != 0 || ((1 << hit.collider.gameObject.layer) & layer2) != 0)
            {
                // UI ��� Ȱ��ȭ
                uiElement.SetActive(true);
            }
            else
            {
                // UI ��� ��Ȱ��ȭ
                uiElement.SetActive(false);
            }
        }
        else
        {
            // ����ĳ��Ʈ�� �ƹ��͵� ���� ������ UI ��Ȱ��ȭ
            uiElement.SetActive(false);
        }
    }
}
