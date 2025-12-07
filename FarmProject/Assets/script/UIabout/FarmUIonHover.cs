using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// FarmUIonHover.cs
/// 마우스 포인터가 지정된 레이어의 오브젝트 위에 있을 때 UI 요소를 활성화/비활성화하는 스크립트
/// 
/// - EventSystem을 검사하여 UI 위에 포인터가 있을 경우 레이캐스트를 무시하도록 처리
/// - layers 배열로 여러 레이어를 감지할 수 있으며, 마우스 위치에서 Raycast를 사용하여 판정
/// </summary>
public class FarmUIonHover : MonoBehaviour
{
    // 감지할 레이어들의 배열 
    public LayerMask[] layers;

    // 마우스 오버 시 활성화할 UI 요소 
    public GameObject uiElement;

    // 레이캐스트 최대 거리
    public float maxDistance = 10f;

    // 메인 카메라 
    private Camera mainCamera;

    // 카메라와 EventSystem 존재 여부를 확인하고 캐싱
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

    /// <summary>
    /// - 매 프레임 마우스 위치에서 Raycast를 수행하여 지정된 레이어에 있으면 uiElement를 활성화
    /// - 포인터가 UI 위에 있을 경우 레이캐스트 판정 및 UI 활성화를 하지 않음
    /// </summary>
    void Update()
    {
        // uiElement가 할당되지 않았으면 에러 로그 출력 후 리턴
        if (uiElement == null)
        {
            Debug.LogError("UI Element is not assigned in the Inspector!");
            return;
        }

        // 마우스가 UI 위에 있으면 탐지 중단(오버랩 방지)
        if (IsPointerOverUI())
        {
            uiElement.SetActive(false);
            return;
        }

        if (mainCamera == null) return; // 카메라가 없으면 더 이상 처리하지 않음

        // 마우스 화면 좌표에서 월드로 Ray를 쏨
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 레이캐스트가 어떤 콜라이더에 맞았는지 검사
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            int hitLayer = hit.collider.gameObject.layer;

            // layers 배열에 포함된 LayerMask들과 비교하여 포함되면 UI 활성화
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

    // 현재 포인터가 UI 요소 위에 있는지 확인하는 함수
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject();
    }
}
