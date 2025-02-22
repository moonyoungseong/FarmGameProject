using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmUIonHover : MonoBehaviour
{
    public LayerMask layer1; // Layer1 레이어
    public LayerMask layer2; // Layer2 레이어
    public GameObject uiElement; // 마우스가 오버된 경우 표시할 UI 요소

    void Update()
    {
        // 마우스 위치에서 레이캐스트를 쏘아 오브젝트를 감지
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 레이캐스트로 맞은 오브젝트가 Layer1 또는 Layer2에 속하는지 확인
            if (((1 << hit.collider.gameObject.layer) & layer1) != 0 || ((1 << hit.collider.gameObject.layer) & layer2) != 0)
            {
                // UI 요소 활성화
                uiElement.SetActive(true);
            }
            else
            {
                // UI 요소 비활성화
                uiElement.SetActive(false);
            }
        }
        else
        {
            // 레이캐스트가 아무것도 맞지 않으면 UI 비활성화
            uiElement.SetActive(false);
        }
    }
}
