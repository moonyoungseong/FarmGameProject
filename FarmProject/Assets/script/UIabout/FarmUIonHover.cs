using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FarmUIonHover : MonoBehaviour
{
    public LayerMask[] layers;  // ������ ���̾� �迭
    public GameObject uiElement; // ���콺 ���� �� ǥ���� UI ���
    public float maxDistance = 10f; // ����ĳ��Ʈ �Ÿ� ����
    private Camera mainCamera;  // ī�޶� ĳ��

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

        // UI ���� �ִ��� üũ
        if (IsPointerOverUI())
        {
            uiElement.SetActive(false);
            return;
        }

        if (mainCamera == null) return; // ī�޶� ������ �������� ����

        // ���콺 ��ġ���� Ray�� ���
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            int hitLayer = hit.collider.gameObject.layer;

            // ������ ������Ʈ�� ������ ���̾ ���ϴ��� Ȯ��
            foreach (LayerMask layer in layers)
            {
                if ((layer.value & (1 << hitLayer)) != 0)
                {
                    uiElement.SetActive(true);
                    return;
                }
            }
        }

        // �ƹ��͵� �������� ������ UI ��Ȱ��ȭ
        uiElement.SetActive(false);
    }

    // UI ��� ���� �ִ��� �����ϴ� �Լ�
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject();
    }
}
