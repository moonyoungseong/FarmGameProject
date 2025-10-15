using UnityEngine;

public class NpcCameraSwitch : MonoBehaviour
{
    public Camera playerCamera; // �÷��̾� ī�޶�
    public Camera npcCamera;    // NPC ī�޶�
    public NpcData[] npcData;   // NPC ������ �迭

    private Transform currentTarget; // ���� NPC ī�޶��� ��ǥ

    void Start()
    {
        // �ʱ� ���¿��� �÷��̾� ī�޶� Ȱ��ȭ
        playerCamera.gameObject.SetActive(true);
        npcCamera.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // NPC�� �浹���� �� ī�޶� ��ȯ
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
        // NPC���� �浹�� ������ �� �÷��̾� ī�޶�� ��ȯ
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
        // �÷��̾� ī�޶� ��Ȱ��ȭ, NPC ī�޶� Ȱ��ȭ
        playerCamera.gameObject.SetActive(false);
        npcCamera.gameObject.SetActive(true);
        currentTarget = data.npcTransform;

        if (data.uiObject != null)
        {
            data.uiObject.SetActive(true);
        }

        // NPC ī�޶��� ��ġ�� ������ NPC �����Ϳ� ���� ����
        npcCamera.transform.position = data.cameraPosition;
        npcCamera.transform.rotation = Quaternion.Euler(data.cameraRotation);
    }

    private void SwitchToPlayerCamera()
    {
        // �÷��̾� ī�޶� Ȱ��ȭ, NPC ī�޶� ��Ȱ��ȭ
        playerCamera.gameObject.SetActive(true);
        npcCamera.gameObject.SetActive(false);
        foreach (var data in npcData)
        {
            if (data.uiObject != null)
            {
                data.uiObject.SetActive(false); // UI ��Ȱ��ȭ
            }
            
        }
        currentTarget = null;
    }
}

[System.Serializable]
public class NpcData
{
    public string npcName;        // NPC �̸�
    public Transform npcTransform; // NPC�� Transform
    public Vector3 cameraPosition; // ī�޶��� ��ġ
    public Vector3 cameraRotation; // ī�޶��� ȸ���� (EulerAngles)
    public GameObject uiObject;   // NPC�� ���õ� UI ������Ʈ
}
