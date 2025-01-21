using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecoManager : MonoBehaviour
{
    public List<Item> DecoItems = new List<Item>();
    public GameObject[] DecoSlot;
    public ParticleSystem spawnParticlePrefab; // ��ƼŬ �ý��� ������

    private GameObject spawnedDecoItem; // ��ȯ�� �������� ������ ����
    private bool isPlacingItem = false;  // ������ ��ġ �� ����

    public LayerMask groundLayer;

    void Start()
    {
        UpdateDecoItems();
        HomeList();
    }

    public void HomeList()
    {
        // ������ŭ ���� ���̱�
        for (int i = 0; i < DecoSlot.Length; i++)
        {
            if (i < DecoItems.Count)
            {
                // ���Կ� ������ �̸��� ������ ����
                Transform slotTransform = DecoSlot[i].transform;
                slotTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = DecoItems[i].itemName;
                slotTransform.Find("Count").GetComponent<TextMeshProUGUI>().text = DecoItems[i].quantity + "��";
                slotTransform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + DecoItems[i].itemIcon);
                DecoSlot[i].SetActive(true);
            }
            else
            {
                DecoSlot[i].SetActive(false);
            }
        }
    }

    public void UpdateDecoItems()
    {
        // InventoryManager�� MyItemList���� HomeDeco ������ ���͸�
        DecoItems = InventoryManager.Instance.MyItemList.FindAll(item => item.itemType == "HomeDeco");
    }

    // �ŷ� �̺�Ʈ �߻� �� ȣ��
    public void OnTradeCompleted()
    {
        UpdateDecoItems(); // DecoItems ����
        HomeList();        // UI ������Ʈ
    }

    // ���콺 ��ġ�� ���� ������ ��ȯ
    public void SpawnDecoItemFollowMouse(int index)
    {
        if (index < 0 || index >= DecoItems.Count)
        {
            Debug.LogError("�߸��� �ε����Դϴ�.");
            return;
        }

        // DecoItem�� ������ �̸� ��������
        string prefabName = DecoItems[index].itemIcon.Replace("_i", ""); // �ʿ� �� "_i" ����
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Item/DecoItem/" + prefabName);

        if (prefab != null)
        {
            // ������ ��ȯ�� �������� ������ ����
            if (spawnedDecoItem != null)
            {
                Destroy(spawnedDecoItem);
            }

            // Raycast�� ���콺 ��ġ�� ���� ���� ��ġ�� ã��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // �����ɽ�Ʈ�� ���� ������ ������Ʈ ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 worldPosition = hit.point; // Raycast�� ���� �������� ��ġ ����

                // Z���� ������Ű��, X, Y ���� ������Ʈ
                worldPosition.z = 0f; // ���� Z�� ����

                // ������ ��ȯ
                spawnedDecoItem = Instantiate(prefab, worldPosition, Quaternion.identity);
                Debug.Log($"������ {prefabName} ��ȯ �Ϸ�!");

                isPlacingItem = true;  // ������ ��ġ ��
            }
        }
        else
        {
            Debug.LogError($"������ {prefabName}��(��) ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        // ���콺 �����͸� ���� ��ȯ�� ������ �̵�
        if (spawnedDecoItem != null && isPlacingItem)
        {
            // Raycast�� ���콺 ��ġ�� ���� ���� ��ġ�� ã��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast�� ������ �����ϵ��� ���̾� ����ũ�� ���
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // Raycast�� ���� �������� ��ġ ����
                Vector3 worldPosition = hit.point;

                // Y���� �����ɽ�Ʈ�� ���� ������ ����
                worldPosition.y = hit.point.y; // �����ɽ�Ʈ�� ���� y������ ����

                // X�� Z���� ���콺 ��ġ�� ���� �̵�
                worldPosition.x = hit.point.x;
                worldPosition.z = hit.point.z;

                // ������Ʈ�� ��ġ�� worldPosition���� ����
                spawnedDecoItem.transform.position = worldPosition;
            }
        }

        // ���콺 Ŭ�� �� ������ ��ġ
        if (Input.GetMouseButtonDown(0) && spawnedDecoItem != null && isPlacingItem)
        {
            PlaceDecoItem(); // ������ ��ġ
  
            isPlacingItem = false;  // ������ ��ġ �� ���� ����
            spawnedDecoItem = null; // ������ ����
        }
    }

    void PlaySpawnParticle(Vector3 position)
    {
        if (spawnParticlePrefab != null)  // ��ƼŬ �������� null���� üũ
        {
            // ��ƼŬ�� Ư�� ��ġ�� ���
            ParticleSystem particle = Instantiate(spawnParticlePrefab, position, Quaternion.identity);
            particle.Play(); // ��ƼŬ ����

            Debug.Log("��ƼŬ�� ���۵Ǿ����ϴ�: " + particle.isPlaying); // ��ƼŬ�� ����Ǵ��� Ȯ��

            Destroy(particle.gameObject, particle.main.duration); // ��ƼŬ�� ������ �ڵ����� ����
        }
        else
        {
            Debug.LogWarning("��ƼŬ �������� null�Դϴ�. ��ƼŬ�� ������� �ʾҽ��ϴ�.");
        }
    }


    // �������� ��ġ�ϰ� ���ο� �������� �غ�
    void PlaceDecoItem()
    {
        if (spawnedDecoItem != null)
        {

            if (spawnedDecoItem != null) // spawnedDecoItem�� null�� �ƴϸ� ��ƼŬ ���
            {
                PlaySpawnParticle(spawnedDecoItem.transform.position); // ��ƼŬ ���
            }
            else
            {
                Debug.LogWarning("spawnedDecoItem�� null�̹Ƿ� ��ƼŬ�� ����� �� �����ϴ�.");
            }

            // �������� ��ġ (���⼭�� �׳� ���´ٰ� ����)
            Debug.Log("������ ��ġ �Ϸ�!");

            // ���ο� �������� ��ȯ�� �غ�
            isPlacingItem = false;  // ��ġ �Ϸ�

            // ���� �������� ��ġ�� �غ�
            spawnedDecoItem = null; // ���� �������� null�� �����Ͽ� �ʱ�ȭ           
        }
    }
}
