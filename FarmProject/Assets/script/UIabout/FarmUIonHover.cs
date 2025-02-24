using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FarmUIonHover : MonoBehaviour
{
    public LayerMask[] layers;  // 감지할 레이어 배열
    public GameObject uiElement; // 마우스 오버 시 표시할 UI 요소
    public float maxDistance = 10f; // 레이캐스트 거리 제한
    private Camera mainCamera;  // 카메라 캐싱

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Make sure your camera has the 'MainCamera' tag.");
        }

        if (EventSystem.current == null)
        {
            Debug.LogError("EventSystem not found! Make sure there is an EventSystem in the scene.");
        }
    }

    void Update()
    {
        if (uiElement == null)
        {
            Debug.LogError("UI Element is not assigned in the Inspector!");
            return;
        }

        // UI 위에 있는지 체크
        if (IsPointerOverUI())
        {
            uiElement.SetActive(false);
            return;
        }

        if (mainCamera == null) return; // 카메라가 없으면 진행하지 않음

        // 마우스 위치에서 Ray를 쏘기
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            int hitLayer = hit.collider.gameObject.layer;

            // 감지된 오브젝트가 지정된 레이어에 속하는지 확인
            foreach (LayerMask layer in layers)
            {
                if ((layer.value & (1 << hitLayer)) != 0)
                {
                    uiElement.SetActive(true);
                    return;
                }
            }
        }

        // 아무것도 감지되지 않으면 UI 비활성화
        uiElement.SetActive(false);
    }

    // UI 요소 위에 있는지 감지하는 함수
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject();
    }
}
