using UnityEngine;

public class NpcCameraSwitch : MonoBehaviour
{
    public Camera playerCamera;  // 플레이어 전용 카메라
    public Camera npcCamera;     // NPC 전용 카메라
    public Transform[] npcPositions;  // NPC들의 위치와 회전 값들

    // 각 NPC의 이름을 설정
    public string[] npcNames;  // 예: {"NPC1", "NPC2", "NPC3", ...}

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 객체의 이름을 확인하여 NPC에 맞는 카메라로 전환
        for (int i = 0; i < npcNames.Length; i++)
        {
            if (collision.gameObject.name == npcNames[i])  // 충돌한 객체의 이름이 일치하는지 확인
            {
                SwitchCamera(i);  // 해당 NPC의 카메라로 전환
                break;
            }
        }
    }

    // 카메라 전환
    void SwitchCamera(int npcIndex)
    {
        // 플레이어 카메라 비활성화
        playerCamera.gameObject.SetActive(false);

        // NPC 카메라 활성화
        npcCamera.gameObject.SetActive(true);

        // 해당 NPC의 위치와 회전값을 설정하여 카메라 위치 조정
        npcCamera.transform.position = npcPositions[npcIndex].position;
        npcCamera.transform.rotation = npcPositions[npcIndex].rotation;
    }

    // 버튼을 눌렀을 때 원래 카메라로 복귀
    public void ResetCamera()
    {
        // NPC 카메라 비활성화
        npcCamera.gameObject.SetActive(false);

        // 플레이어 카메라 활성화
        playerCamera.gameObject.SetActive(true);
    }
}
