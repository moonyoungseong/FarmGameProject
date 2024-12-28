using UnityEngine;
using System.Collections.Generic;  // List ���

public class FarmController : MonoBehaviour
{
    public CropFactory cropFactory;      // CropFactory�� ����
    public CropAttributes CornAttributes;  // ������ �۹� �Ӽ� ������

    public LayerMask groundLayer;        // �ٴ� ���̾� ����
    public Transform player;             // �÷��̾��� Transform
    public float maxPlantDistance = 5f;  // �۹��� ���� �� �ִ� �ִ� �Ÿ�
    public float maxPlantAngle = 30f;    // �÷��̾� ���� ���� ��� ���� (30��: ���鸸 ���)
    public float plantCooldown = 1f;     // �۹��� ���� �� �ִ� ���ð� (�� ����)
    public float minPlantDistance = 1f;  // �ٸ� �۹����� �ּ� �Ÿ� ����

    private float lastPlantTime = 0f;    // ���������� �۹��� ���� �ð� ���
    private List<Vector3> plantedCropPositions = new List<Vector3>();  // �ɾ��� �۹� ��ġ ����

    void Update()
    {
        // ���콺 ���� Ŭ���� ����
        if (Input.GetMouseButtonDown(0) && CanPlant())
        {
            Vector3 mousePosition = GetMouseWorldPosition();  // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            if (IsWithinPlantingRange(mousePosition) && IsFarEnoughFromOtherCrops(mousePosition))  // �Ÿ� �� ���� Ȯ��
            {
                cropFactory.CreateCrop(CornAttributes, mousePosition);  // ������ �۹��� ����
                plantedCropPositions.Add(mousePosition);  // ������ �۹� ��ġ�� ����Ʈ�� �߰�
                lastPlantTime = Time.time;  // ������ ���� �ð� ���
            }
        }
    }

    // ���콺�� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ ��, ����ĳ��Ʈ�� �ٴڿ� �浹�� ��ġ ��ȯ
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // ���콺 ȭ�� ��ǥ���� ���� ����
        RaycastHit hit;

        // ����ĳ��Ʈ�� �ٴ� ���̾�� �浹�ϸ�, �ش� ������ ���� ��ǥ ��ȯ
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;  // ���̰� �浹�� ������ ���� ��ǥ
        }

        return Vector3.zero;  // �浹�� ������ (��: �ٴ��� �ƴ� �� Ŭ�� ��) (0,0,0) ��ȯ
    }

    // Ŭ�� ��ġ�� �÷��̾� ���� ��� �Ÿ��� ���� ������ Ȯ��
    bool IsWithinPlantingRange(Vector3 position)
    {
        // �Ÿ� ���
        float distance = Vector3.Distance(player.position, position);

        // ���� ���� ���
        Vector3 directionToMouse = (position - player.position).normalized;

        // �÷��̾� ���� ���� ���� ���
        float angle = Vector3.Angle(player.forward, directionToMouse);

        // �Ÿ��� ���� ��� ���� (���鿡���� ����)
        return distance <= maxPlantDistance && angle <= maxPlantAngle;
    }

    // Ŭ�� ��ġ�� �ٸ� �۹� ��ġ���� �ּ� �Ÿ����� ����� ���� Ȯ��
    bool IsFarEnoughFromOtherCrops(Vector3 position)
    {
        foreach (Vector3 cropPosition in plantedCropPositions)
        {
            if (Vector3.Distance(position, cropPosition) < minPlantDistance)
            {
                Debug.Log("�ٸ� �۹��� �ʹ� ������� ���� �� �����ϴ�.");
                return false;  // �ٸ� �۹����� �Ÿ��� �ʹ� ������ false ��ȯ
            }
        }
        return true;  // ��� �۹����� �Ÿ��� ����� �ָ� true ��ȯ
    }

    // ���ð��� ���� ��쿡�� true ��ȯ
    bool CanPlant()
    {
        return Time.time >= lastPlantTime + plantCooldown;
    }
}
