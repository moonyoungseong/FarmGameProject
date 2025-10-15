using UnityEngine;

public class NpcCameraSwitch : MonoBehaviour
{
    public Camera playerCamera; // 플레이어 카메라
    public Camera npcCamera;    // NPC 카메라
    public NpcData[] npcData;   // NPC 데이터 배열

    private Transform currentTarget; // 현재 NPC 카메라의 목표

    void Start()
    {
        // 초기 상태에서 플레이어 카메라 활성화
        playerCamera.gameObject.SetActive(true);
        npcCamera.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // NPC와 충돌했을 때 카메라 전환
        foreach (var data in npcData)
        {
            if (collision.transform == data.npcTransform)
            {
                Debug.Log($"Collided with NPC: {data.npcName}");
                SwitchToNpcCamera(data);
                return;
            }
        }
        //Debug.Log("Collision detected, but no matching NPC found.");
    }

    private void OnCollisionExit(Collision collision)
    {
        // NPC와의 충돌이 끝났을 때 플레이어 카메라로 전환
        foreach (var data in npcData)
        {
            if (collision.transform == data.npcTransform)
            {
                //Debug.Log($"Stopped colliding with NPC: {data.npcName}");
                SwitchToPlayerCamera();
                return;
            }
        }
        //Debug.Log("Collision exit detected, but no matching NPC found.");
    }

    private void SwitchToNpcCamera(NpcData data)
    {
        // 플레이어 카메라 비활성화, NPC 카메라 활성화
        playerCamera.gameObject.SetActive(false);
        npcCamera.gameObject.SetActive(true);
        currentTarget = data.npcTransform;

        if (data.uiObject != null)
        {
            data.uiObject.SetActive(true);
        }

        // NPC 카메라의 위치와 방향을 NPC 데이터에 따라 설정
        npcCamera.transform.position = data.cameraPosition;
        npcCamera.transform.rotation = Quaternion.Euler(data.cameraRotation);
    }

    private void SwitchToPlayerCamera()
    {
        // 플레이어 카메라 활성화, NPC 카메라 비활성화
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

[System.Serializable]
public class NpcData
{
    public string npcName;        // NPC 이름
    public Transform npcTransform; // NPC의 Transform
    public Vector3 cameraPosition; // 카메라의 위치
    public Vector3 cameraRotation; // 카메라의 회전값 (EulerAngles)
    public GameObject uiObject;   // NPC에 관련된 UI 오브젝트
}
