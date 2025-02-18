using UnityEngine;

public class FarmTileChanger : MonoBehaviour
{
    public GameObject plowedSoilPrefab; // ���� ��� �� ����� ��
    public GameObject dugSoilPrefab;    // �� ��� �� ����� ��
    private int farmTileLayer;
    private int plowedTileLayer;
    private int dugTileLayer; // ������ ���� �� ���̾� �߰�

    public CursorIcons cursorIcons;  // �ν����Ϳ��� CursorIcons ����

    public ParticleSystem changeSoilParticleEffect; // ��ƼŬ �ý��� �߰�

    private void Start()
    {
        farmTileLayer = LayerMask.NameToLayer("FarmTile");   // �⺻ �� ���̾�
        plowedTileLayer = LayerMask.NameToLayer("PlowedTile"); // ���̷� ���ƾ��� �� ���̾�
        dugTileLayer = LayerMask.NameToLayer("DugTile"); // ������ ��ȭ�� �� ���̾�
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� Ŭ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                int hitLayer = hit.collider.gameObject.layer;
                ChangeFarmTile(hit.collider.gameObject, hitLayer, hit.point);
            }
        }
    }

    void ChangeFarmTile(GameObject farmTile, int tileLayer, Vector3 position)
    {
        // �⺻ Ŀ�� ���� Ŭ����
        DefaultCursorManager defaultCursorManager = DefaultCursorManager.instance;

        if (defaultCursorManager == null || cursorIcons == null)
        {
            Debug.LogError("DefaultCursorManager �Ǵ� CursorIcons�� null�Դϴ�. ��ũ��Ʈ�� Ȯ���ϼ���.");
            return;
        }

        // �⺻ Ŀ�� ��������
        Texture2D currentDefaultCursor = defaultCursorManager.GetCurrentCursor();
        // �ٸ� Ŀ�� ��������
        Texture2D currentCursor = cursorIcons.GetCurrentCursor();  // "farmTool1"�� ���÷� ���

        // Ŀ���� ���� ��� �⺻ Ŀ�� ���
        if (currentCursor == null)
        {
            currentCursor = currentDefaultCursor;  // �⺻ Ŀ���� ��ü
        }

        Debug.Log($"���� Ŀ��: {currentCursor.name}, ������ ������Ʈ ���̾�: {tileLayer}, ���̾� �̸�: {LayerMask.LayerToName(tileLayer)}");

        // Ŀ���� farmTool1 (����)�� ��, ���� ���ƾ��� ���̾�� ����
        if (currentCursor == cursorIcons.FarmTool1 && tileLayer == farmTileLayer)
        {
            Debug.Log("���� ��� �� �� ����!");
            ReplaceFarmTile(farmTile, plowedSoilPrefab, position);
        }
        // Ŀ���� shovel (��)�� ��, ���ƾ��� ���� ������ ����
        else if (currentCursor == cursorIcons.shovel && tileLayer == plowedTileLayer)
        {
            Debug.Log("�� ��� �� �� ����!");
            ReplaceFarmTile(farmTile, dugSoilPrefab, position);
        }
        // ���� ����ϰ�, �̹� ������ ��ȯ�� ���̸� �ٽ� �⺻ ������ ���ư���
        else if (currentCursor == cursorIcons.shovel && tileLayer == dugTileLayer)
        {
            Debug.Log("�⺻ ������ ���ư���!");
            ReplaceFarmTile(farmTile, null, position); // �⺻ ������ ����
        }
    }

    void ReplaceFarmTile(GameObject oldTile, GameObject newPrefab, Vector3 position)
    {
        if (newPrefab != null)
        {
            // ���� ���� ��ġ�� ȸ���� ����
            GameObject newTile = Instantiate(newPrefab, oldTile.transform.position, oldTile.transform.rotation);

            // ���� ���� (�ʿ��� ���)
            newTile.transform.SetParent(oldTile.transform.parent);

            // ���� �� ����
            Destroy(oldTile);

            // ��ƼŬ ȿ�� ��� (�ϳ��� ��ƼŬ�� ���)
            if (changeSoilParticleEffect != null)
            {
                ParticleSystem particle = Instantiate(changeSoilParticleEffect, position, Quaternion.identity);
                particle.Play();
            }
        }
    }
}
