using UnityEngine;

public class FarmController : MonoBehaviour
{
    public CropFactory cropFactory;      // CropFactory�� ����
    public CropAttributes CornAttributes;  // ������ �۹� �Ӽ� ������

    public LayerMask groundLayer;        // �ٴ� ���̾� ����
    public Transform player;             // �÷��̾��� Transform
    public float maxPlantDistance = 5f;  // �۹��� ���� �� �ִ� �ִ� �Ÿ�
    public float maxPlantAngle = 30f;    // �÷��̾� ���� ���� ��� ���� (30��: ���鸸 ���)

    void Update()
    {
        // ���콺 ���� Ŭ���� ����
        if (Input.GetMouseButtonDown(0))  // 0�� ���콺 ���� ��ư
        {
            Vector3 mousePosition = GetMouseWorldPosition();  // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            if (IsWithinPlantingRange(mousePosition))  // �÷��̾� ���� ���� �Ÿ��� ���� Ȯ��
            {
                cropFactory.CreateCrop(CornAttributes, mousePosition);  // ������ �۹��� ����
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
}
