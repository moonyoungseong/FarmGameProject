using UnityEngine;

public class FarmController : MonoBehaviour
{
    public CropFactory cropFactory;   // CropFactory�� ����
    public CropAttributes CornAttributes;  // ������ �۹� �Ӽ� ������
    public CropAttributes TomatoAttributes;  // �丶�� �۹� �Ӽ� ������
    public CropAttributes RiceAttributes;  // �� �۹� �Ӽ� ������

    public LayerMask groundLayer;  // �ٴ� ���̾ ����

    void Update()
    {
        // ���콺 ���� Ŭ���� ����
        if (Input.GetMouseButtonDown(0))  // 0�� ���콺 ���� ��ư
        {
            Vector3 mousePosition = GetMouseWorldPosition();  // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            cropFactory.CreateCrop(CornAttributes, mousePosition);  // ������ �۹��� �ش� ��ġ�� ����
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
}
