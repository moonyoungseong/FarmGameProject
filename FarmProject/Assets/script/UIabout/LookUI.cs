/// <summary>
/// 유튜브 : 더블엘
/// 출처: https://www.youtube.com/watch?v=8IBIRPg8wJw
/// 코드 부분 사용
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LookUI.cs
/// UI(또는 3D 오브젝트)가 항상 메인 카메라를 바라보도록 회전시키는 스크립트
/// </summary>
public class LookUI : MonoBehaviour
{
    // MainCamera 태그 카메라
    private Camera cam;

    void Start()
    {
        FindCamera();
    }

    void Update()
    {
        // 카메라가 없거나 비활성화되었으면 재검색
        if (cam == null || !cam.gameObject.activeInHierarchy)
        {
            FindCamera();
        }

        // 카메라가 정상 활성화된 경우, UI가 카메라를 향하도록 회전
        if (cam != null && cam.gameObject.activeInHierarchy)
        {
            transform.LookAt(
                transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up
            );
        }
    }

    /// <summary>
    /// 메인 카메라(MainCamera 태그)를 검색하여 cam 변수에 할당함
    /// 없거나 비활성화된 경우 경고 로그 출력
    /// </summary>
    private void FindCamera()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();

        if (cam == null || !cam.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Main Camera not found or is inactive.");
        }
    }
}
