using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NpcCameraSwitch.cs
/// 
/// - 플레이어와 NPC 충돌 시 카메라 전환을 담당하는 매니저.
/// - 충돌 시 NPC 카메라로 전환하고, 충돌 종료 시 다시 플레이어 카메라로 전환.
/// - 각 NPC 별로 지정된 카메라 위치와 회전, UI 오브젝트를 적용.
/// </summary>
public class NpcCameraSwitch : MonoBehaviour
{
    // 플레이어 카메라
    public Camera playerCamera;

    // NPC 카메라
    public Camera npcCamera;

    // NPC별 카메라 전환 정보 배열
    public NpcData[] npcData;

    // 현재 NPC 카메라가 바라보는 대상
    private Transform currentTarget;

    void Start()
    {
        // 초기 상태: 플레이어 카메라 활성화, NPC 카메라 비활성화
        playerCamera.gameObject.SetActive(true);
        npcCamera.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // NPC와 충돌하면 해당 NPC 정보로 카메라 전환
        foreach (var data in npcData)
        {
            if (collision.transform == data.npcTransform)
            {
                SwitchToNpcCamera(data);
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // NPC와 충돌이 끝나면 플레이어 카메라로 전환
        foreach (var data in npcData)
        {
            if (collision.transform == data.npcTransform)
            {
                SwitchToPlayerCamera();
                return;
            }
        }
    }

    /// <summary>
    /// NPC 충돌 시 플레이어 카메라 → NPC 카메라 전환
    /// </summary>
    /// <param name="data">전환할 NPC 데이터</param>
    private void SwitchToNpcCamera(NpcData data)
    {
        playerCamera.gameObject.SetActive(false);
        npcCamera.gameObject.SetActive(true);
        currentTarget = data.npcTransform;

        if (data.uiObject != null)
        {
            data.uiObject.SetActive(true);
        }

        // NPC 카메라 위치 및 회전 적용
        npcCamera.transform.position = data.cameraPosition;
        npcCamera.transform.rotation = Quaternion.Euler(data.cameraRotation);
    }

    /// <summary>
    /// 충돌 종료 시 NPC 카메라 → 플레이어 카메라 전환
    /// </summary>
    private void SwitchToPlayerCamera()
    {
        playerCamera.gameObject.SetActive(true);
        npcCamera.gameObject.SetActive(false);
        foreach (var data in npcData)
        {
            if (data.uiObject != null)
            {
                data.uiObject.SetActive(false); // UI 비활성화
            }
        }
        currentTarget = null;
    }
}

// NPC별 카메라 전환 및 UI 정보를 담는 데이터 클래스
[System.Serializable]
public class NpcData
{
    public string npcName;
    public Transform npcTransform;

    // 카메라 위치
    public Vector3 cameraPosition;

    // 카메라 회전
    public Vector3 cameraRotation;

    public GameObject uiObject;
}
