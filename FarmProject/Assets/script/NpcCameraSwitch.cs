using UnityEngine;

public class NpcCameraSwitch : MonoBehaviour
{
    public Camera playerCamera;  // �÷��̾� ���� ī�޶�
    public Camera npcCamera;     // NPC ���� ī�޶�
    public Transform[] npcPositions;  // NPC���� ��ġ�� ȸ�� ����

    // �� NPC�� �̸��� ����
    public string[] npcNames;  // ��: {"NPC1", "NPC2", "NPC3", ...}

    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� �̸��� Ȯ���Ͽ� NPC�� �´� ī�޶�� ��ȯ
        for (int i = 0; i < npcNames.Length; i++)
        {
            if (collision.gameObject.name == npcNames[i])  // �浹�� ��ü�� �̸��� ��ġ�ϴ��� Ȯ��
            {
                SwitchCamera(i);  // �ش� NPC�� ī�޶�� ��ȯ
                break;
            }
        }
    }

    // ī�޶� ��ȯ
    void SwitchCamera(int npcIndex)
    {
        // �÷��̾� ī�޶� ��Ȱ��ȭ
        playerCamera.gameObject.SetActive(false);

        // NPC ī�޶� Ȱ��ȭ
        npcCamera.gameObject.SetActive(true);

        // �ش� NPC�� ��ġ�� ȸ������ �����Ͽ� ī�޶� ��ġ ����
        npcCamera.transform.position = npcPositions[npcIndex].position;
        npcCamera.transform.rotation = npcPositions[npcIndex].rotation;
    }

    // ��ư�� ������ �� ���� ī�޶�� ����
    public void ResetCamera()
    {
        // NPC ī�޶� ��Ȱ��ȭ
        npcCamera.gameObject.SetActive(false);

        // �÷��̾� ī�޶� Ȱ��ȭ
        playerCamera.gameObject.SetActive(true);
    }
}
