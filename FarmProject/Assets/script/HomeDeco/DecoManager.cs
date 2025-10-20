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
    private bool isPlacingItem = false; // ������ ��ġ �� ����
    private bool canPlaceItem = false;  // ������ ��ġ ���� ����

    public LayerMask groundLayer;

    void Start()
    {
        UpdateDecoItems();
        HomeList();
    }

    public void HomeList()
    {
        // ������ 0�� �������� ����Ʈ���� ����
        DecoItems.RemoveAll(item => int.Parse(item.quantity) <= 0);

        // ������ŭ ���� ���̱�
        for (int i = 0; i < DecoSlot.Length; i++)
        {
            if (i < DecoItems.Count)
            {
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
        DecoItems = InventoryManager.Instance.MyItemList.FindAll(item => item.itemType == "HomeDeco");
    }

    public void OnTradeCompleted()
    {
        UpdateDecoItems();
        HomeList();
        // myItems���� ���� ������Ʈ
        //UpdateItemInMyItems(item.itemName, item.quantity);
    }

    public void SpawnDecoItemFollowMouse(int index)
    {
        if (index < 0 || index >= DecoItems.Count)
        {
            Debug.LogError("�߸��� �ε����Դϴ�.");
            return;
        }

        string prefabName = DecoItems[index].itemIcon.Replace("_i", "");
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Item/DecoItem/" + prefabName);

        if (prefab != null)
        {
            if (spawnedDecoItem != null)
            {
                Destroy(spawnedDecoItem);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 worldPosition = hit.point;
                worldPosition.z = 0f;

                spawnedDecoItem = Instantiate(prefab, worldPosition, Quaternion.identity);
                Debug.Log($"������ {prefabName} ��ȯ �Ϸ�!");

                isPlacingItem = true;
                canPlaceItem = false; // ó������ ��ġ �Ұ���
                StartCoroutine(EnablePlacementAfterDelay(1f)); // 2�� �� ��ġ ����

                // ��ġ�� �������� ������ ����
                ReduceItemQuantity(index);
            }
        }
        else
        {
            Debug.LogError($"������ {prefabName}��(��) ã�� �� �����ϴ�.");
        }
    }

    IEnumerator EnablePlacementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlaceItem = true;
        Debug.Log("������ ��ġ�� �����մϴ�!");
    }

    void Update()
    {
        if (spawnedDecoItem != null && isPlacingItem)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 worldPosition = hit.point;
                worldPosition.y = hit.point.y;
                worldPosition.x = hit.point.x;
                worldPosition.z = hit.point.z;

                spawnedDecoItem.transform.position = worldPosition;
            }
        }

        if (Input.GetMouseButtonDown(0) && spawnedDecoItem != null && isPlacingItem)
        {
            if (canPlaceItem) // ��ġ ���� ���� Ȯ��
            {
                PlaceDecoItem();
                AudioManager.Instance.PlaySFX(4);// ������ ȿ����
            }
            else
            {
                Debug.LogWarning("���� ��ġ�� �� �����ϴ�. ��ٷ��ּ���!");
            }
        }
    }

    void PlaySpawnParticle(Vector3 position)
    {
        if (spawnParticlePrefab != null)
        {
            ParticleSystem particle = Instantiate(spawnParticlePrefab, position, Quaternion.identity);
            particle.Play();
            Destroy(particle.gameObject, particle.main.duration);
        }
        else
        {
            Debug.LogWarning("��ƼŬ �������� null�Դϴ�. ��ƼŬ�� ������� �ʾҽ��ϴ�.");
        }
    }

    void PlaceDecoItem()
    {
        if (spawnedDecoItem != null)
        {
            PlaySpawnParticle(spawnedDecoItem.transform.position);
            Debug.Log("������ ��ġ �Ϸ�!");

            // ��ġ ���� �ʱ�ȭ
            isPlacingItem = false;
            spawnedDecoItem = null;
        }
    }

    void ReduceItemQuantity(int index)
    {
        if (index >= 0 && index < DecoItems.Count)
        {
            var item = DecoItems[index];

            // ���� ����
            int curNumber = int.Parse(item.quantity) - 1;
            Debug.Log($"������ {item.itemName} ���� ���� ��: {item.quantity}, ���� ���� ��: {curNumber}");

            if (curNumber <= 0)
            {
                // ������ 0 �����̸� ����Ʈ���� ����
                DecoItems.RemoveAt(index);
                Debug.Log($"������ {item.itemName} ������ 0 ���Ϸ� ����, ������.");

                // myItems������ �ش� �������� ������ 0�̸� ����
                RemoveItemFromMyItems(item.itemName);
            }
            else
            {
                // ���� ������Ʈ
                item.quantity = curNumber.ToString();
                Debug.Log($"������ {item.itemName} ���� ������Ʈ��: {item.quantity}");

                // myItems���� ���� ������Ʈ
                UpdateItemInMyItems(item.itemName, item.quantity);
            }

            // UI ������Ʈ
            HomeList();

            // �κ��丮 ����
            InventoryManager.Instance.Save();
        }
        else
        {
            Debug.LogWarning("������ �ε����� ��ȿ���� �ʽ��ϴ�.");
        }
    }

    // myItems���� �ش� ������ ����
    void RemoveItemFromMyItems(string itemName)
    {
        var itemToRemove = InventoryManager.Instance.MyItemList.Find(x => x.itemName == itemName);
        if (itemToRemove != null)
        {
            InventoryManager.Instance.MyItemList.Remove(itemToRemove);
            Debug.Log($"������ {itemName}��(��) myItems���� �����Ǿ����ϴ�.");
        }
    }

    // myItems���� �ش� �������� ���� ������Ʈ
    void UpdateItemInMyItems(string itemName, string quantity)
    {
        var itemToUpdate = InventoryManager.Instance.MyItemList.Find(x => x.itemName == itemName);
        if (itemToUpdate != null)
        {
            itemToUpdate.quantity = quantity;
            Debug.Log($"������ {itemName}�� ������ myItems���� ������Ʈ�Ǿ����ϴ�.");
        }
    }

}
