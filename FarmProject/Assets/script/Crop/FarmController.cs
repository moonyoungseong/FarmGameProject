using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FarmController : MonoBehaviour
{
    public CropFactory cropFactory;         // CropFactory�� ����
    public CropAttributes CornAttributes;   // ������ �۹� �Ӽ� ������
    public CropAttributes TomatoAttributes; // �丶�� �۹� �Ӽ� ������
    public CropAttributes RiceAttributes;   // �� �۹� �Ӽ� ������

    public LayerMask groundLayer;           // �ٴ� ���̾� ����
    public Transform player;                // �÷��̾��� Transform
    public float maxPlantDistance = 5f;     // �۹��� ���� �� �ִ� �ִ� �Ÿ�
    public float maxPlantAngle = 30f;       // �÷��̾� ���� ���� ��� ����
    public float plantCooldown = 1f;        // �۹��� ���� �� �ִ� ���ð�
    public float minPlantDistance = 1f;     // �ٸ� �۹����� �ּ� �Ÿ� ����

    private float lastPlantTime = 0f;       // ���������� �۹��� ���� �ð�
    private List<Vector3> plantedCropPositions = new List<Vector3>();
    private PlayerMove playerMoveScript;

    // ���� ���õ� �۹��� ������ ����
    private CropAttributes selectedCropAttributes;

    void Start()
    {
        playerMoveScript = player.GetComponent<PlayerMove>();
        selectedCropAttributes = null; // �⺻ ���¿����� �ƹ� �͵� ���� ����
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && CanPlant())
        {
            if (selectedCropAttributes == null)
            {
                Debug.Log("���õ� �۹��� �����ϴ�.");
                return; // �۹��� ���õ��� ���� ��� �ɱ� ���
            }

            Vector3 mousePosition = GetMouseWorldPosition();
            if (IsWithinPlantingRange(mousePosition) && IsFarEnoughFromOtherCrops(mousePosition))
            {
                cropFactory.CreateCrop(selectedCropAttributes, mousePosition);
                plantedCropPositions.Add(mousePosition);
                lastPlantTime = Time.time;

                StartCoroutine(playerMoveScript.PlantingAnimationCoroutine(10f));
            }
        }
    }

    public void SelectCorn()
    {
        selectedCropAttributes = CornAttributes;
        Debug.Log("�������� �����߽��ϴ�.");
    }

    public void SelectTomato()
    {
        selectedCropAttributes = TomatoAttributes;
        Debug.Log("�丶�並 �����߽��ϴ�.");
    }

    public void SelectRice()
    {
        selectedCropAttributes = RiceAttributes;
        Debug.Log("���� �����߽��ϴ�.");
    }

    public void SelectNone()
    {
        selectedCropAttributes = null;
        Debug.Log("�ƹ� �͵� �������� �ʾҽ��ϴ�.");
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    bool IsWithinPlantingRange(Vector3 position)
    {
        float distance = Vector3.Distance(player.position, position);
        Vector3 directionToMouse = (position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToMouse);

        return distance <= maxPlantDistance && angle <= maxPlantAngle;
    }

    bool IsFarEnoughFromOtherCrops(Vector3 position)
    {
        foreach (Vector3 cropPosition in plantedCropPositions)
        {
            if (Vector3.Distance(position, cropPosition) < minPlantDistance)
            {
                Debug.Log("�ٸ� �۹��� �ʹ� ������� ���� �� �����ϴ�.");
                return false;
            }
        }
        return true;
    }

    bool CanPlant()
    {
        return Time.time >= lastPlantTime + plantCooldown;
    }
}
