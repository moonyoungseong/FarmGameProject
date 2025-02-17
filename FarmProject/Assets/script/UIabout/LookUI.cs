using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUI : MonoBehaviour
{
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        FindCamera();
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라가 비활성화되었으면 다시 찾아봄
        if (cam == null || !cam.gameObject.activeInHierarchy)
        {
            FindCamera();
        }

        if (cam != null && cam.gameObject.activeInHierarchy)
        {
            // UI를 카메라 방향으로 회전
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);
        }
    }

    // 카메라 찾기
    private void FindCamera()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();

        if (cam == null || !cam.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Main Camera not found or is inactive.");
        }
    }
}
