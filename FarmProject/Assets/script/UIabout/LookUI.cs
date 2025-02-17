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
        // ī�޶� ��Ȱ��ȭ�Ǿ����� �ٽ� ã�ƺ�
        if (cam == null || !cam.gameObject.activeInHierarchy)
        {
            FindCamera();
        }

        if (cam != null && cam.gameObject.activeInHierarchy)
        {
            // UI�� ī�޶� �������� ȸ��
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);
        }
    }

    // ī�޶� ã��
    private void FindCamera()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();

        if (cam == null || !cam.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Main Camera not found or is inactive.");
        }
    }
}
