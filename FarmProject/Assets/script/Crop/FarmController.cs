using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FarmController : MonoBehaviour
{
    public GameObject player1;               // �÷��̾� 1
    public GameObject player2;               // �÷��̾� 2
    public CropFactory cropFactory;          // CropFactory�� ����
    public CropAttributes CornAttributes;    // ������ �۹� �Ӽ� ������
    public CropAttributes TomatoAttributes;  // �丶�� �۹� �Ӽ� ������
    public CropAttributes RiceAttributes;    // �� �۹� �Ӽ� ������

    public LayerMask groundLayer;            // �ٴ� ���̾� ����
    private Transform player;                 // �÷��̾��� Transform
    public float maxPlantDistance = 5f;      // �۹��� ���� �� �ִ� �ִ� �Ÿ�
    public float maxPlantAngle = 30f;        // �÷��̾� ���� ���� ��� ����
    public float plantCooldown = 1f;         // �۹��� ���� �� �ִ� ���ð�
    public float minPlantDistance = 1f;      // �ٸ� �۹����� �ּ� �Ÿ� ����

    private float lastPlantTime = 0f;        // ���������� �۹��� ���� �ð�
    public GameObject NoSeedUI;               // �۹� ������ ������ UI
    private List<Vector3> plantedCropPositions = new List<Vector3>();

    // ���� ���õ� �۹��� ������ ����
    private CropAttributes selectedCropAttributes;

    // ������ �ִ� playerMoveScript ���� ���
    private PlayerMove playerMoveScript;

    void Start()
    {
        // �� �ε� �� Ȱ��ȭ�� �÷��̾ ���� playerMoveScript �Ҵ�
        if (player1.activeInHierarchy)
        {
            playerMoveScript = player1.GetComponent<PlayerMove>(); // Player1�� PlayerMove ��ũ��Ʈ �Ҵ�
            player = player1.transform;  // �÷��̾� Ʈ������ ����
        }
        else if (player2.activeInHierarchy)
        {
            playerMoveScript = player2.GetComponent<PlayerMove>(); // Player2�� PlayerMove ��ũ��Ʈ �Ҵ�
            player = player2.transform;  // �÷��̾� Ʈ������ ����
        }

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

            // ������ ������ ���� �ʵ��� ó��
            if (!HasSeed(selectedCropAttributes))
            {
                NoSeedUI.SetActive(true);

                // 2�� �Ŀ� UI ��Ȱ��ȭ
                StartCoroutine(DisableUIAfterTime(2f));

                Debug.Log("������ �۹��� ������ �����ϴ�.");
                return;
            }

            Vector3 mousePosition = GetMouseWorldPosition();
            if (IsWithinPlantingRange(mousePosition) && IsFarEnoughFromOtherCrops(mousePosition))
            {
                cropFactory.CreateCrop(selectedCropAttributes, mousePosition);
                plantedCropPositions.Add(mousePosition);
                lastPlantTime = Time.time;

                // �κ��丮���� ������ ������ 1���� ����
                ReduceSeedCount(selectedCropAttributes);

                StartCoroutine(playerMoveScript.PlantingAnimationCoroutine(10f)); // �ִϸ��̼� ����
            }
        }
    }

    // ������ �ִ��� Ȯ���ϴ� �Լ�
    bool HasSeed(CropAttributes cropAttributes)
    {
        Item seedItem = InventoryManager.Instance.MyItemList.Find(item => item.itemName == cropAttributes.SeedName);
        return seedItem != null && int.Parse(seedItem.quantity) > 0;
    }

    private System.Collections.IEnumerator DisableUIAfterTime(float delayTime)
    {
        // 2�� ���
        yield return new WaitForSeconds(delayTime);

        // UI ��Ȱ��ȭ
        NoSeedUI.SetActive(false);
    }


    // ������ ���� �� �κ��丮���� �ش� ������ ������ ���̴� �Լ�
    void ReduceSeedCount(CropAttributes cropAttributes)
    {
        // �κ��丮���� �ش� ���� ������ ã��
        Item seedItem = InventoryManager.Instance.MyItemList.Find(item => item.itemName == cropAttributes.SeedName);

        if (seedItem != null && int.Parse(seedItem.quantity) > 0)
        {
            // ���� ����
            int newQuantity = int.Parse(seedItem.quantity) - 1;
            seedItem.quantity = newQuantity.ToString();

            // ������ 0�̸� �κ��丮���� ����
            if (newQuantity == 0)
            {
                InventoryManager.Instance.MyItemList.Remove(seedItem);
                Debug.Log($"{cropAttributes.SeedName} ������ 0���� �Ǿ� �κ��丮���� ���ŵǾ����ϴ�.");
            }

            // �κ��丮 ����
            InventoryManager.Instance.Save();
        }
        else
        {
            Debug.Log("������ �����ϰų� �������� �ʽ��ϴ�.");
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


