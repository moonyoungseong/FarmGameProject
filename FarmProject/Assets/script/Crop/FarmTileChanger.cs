using UnityEngine;

public class FarmTileChanger : MonoBehaviour
{
    public GameObject plowedSoilPrefab; // 괭이 사용 시 변경될 밭
    public GameObject dugSoilPrefab;    // 삽 사용 시 변경될 밭
    private int farmTileLayer;
    private int plowedTileLayer;
    private int dugTileLayer; // 삽으로 변할 밭 레이어 추가

    public CursorIcons cursorIcons;  // 인스펙터에서 CursorIcons 참조

    public ParticleSystem changeSoilParticleEffect; // 파티클 시스템 추가

    private void Start()
    {
        farmTileLayer = LayerMask.NameToLayer("FarmTile");   // 기본 밭 레이어
        plowedTileLayer = LayerMask.NameToLayer("PlowedTile"); // 괭이로 갈아엎은 밭 레이어
        dugTileLayer = LayerMask.NameToLayer("DugTile"); // 삽으로 변화할 밭 레이어
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
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
        // 기본 커서 관리 클래스
        DefaultCursorManager defaultCursorManager = DefaultCursorManager.instance;

        if (defaultCursorManager == null || cursorIcons == null)
        {
            Debug.LogError("DefaultCursorManager 또는 CursorIcons가 null입니다. 스크립트를 확인하세요.");
            return;
        }

        // 기본 커서 가져오기
        Texture2D currentDefaultCursor = defaultCursorManager.GetCurrentCursor();
        // 다른 커서 가져오기
        Texture2D currentCursor = cursorIcons.GetCurrentCursor();  // "farmTool1"을 예시로 사용

        // 커서가 없을 경우 기본 커서 사용
        if (currentCursor == null)
        {
            currentCursor = currentDefaultCursor;  // 기본 커서로 대체
        }

        Debug.Log($"현재 커서: {currentCursor.name}, 선택한 오브젝트 레이어: {tileLayer}, 레이어 이름: {LayerMask.LayerToName(tileLayer)}");

        // 커서가 farmTool1 (괭이)일 때, 밭을 갈아엎은 레이어로 변경
        if (currentCursor == cursorIcons.FarmTool1 && tileLayer == farmTileLayer)
        {
            Debug.Log("괭이 사용 → 밭 변경!");
            ReplaceFarmTile(farmTile, plowedSoilPrefab, position);
        }
        // 커서가 shovel (삽)일 때, 갈아엎은 밭을 삽으로 변경
        else if (currentCursor == cursorIcons.shovel && tileLayer == plowedTileLayer)
        {
            Debug.Log("삽 사용 → 밭 변경!");
            ReplaceFarmTile(farmTile, dugSoilPrefab, position);
        }
        // 삽을 사용하고, 이미 삽으로 변환된 밭이면 다시 기본 밭으로 돌아가기
        else if (currentCursor == cursorIcons.shovel && tileLayer == dugTileLayer)
        {
            Debug.Log("기본 밭으로 돌아가기!");
            ReplaceFarmTile(farmTile, null, position); // 기본 밭으로 리셋
        }
    }

    void ReplaceFarmTile(GameObject oldTile, GameObject newPrefab, Vector3 position)
    {
        if (newPrefab != null)
        {
            // 기존 밭의 위치와 회전을 유지
            GameObject newTile = Instantiate(newPrefab, oldTile.transform.position, oldTile.transform.rotation);

            // 계층 유지 (필요한 경우)
            newTile.transform.SetParent(oldTile.transform.parent);

            // 기존 밭 삭제
            Destroy(oldTile);

            // 파티클 효과 재생 (하나의 파티클만 사용)
            if (changeSoilParticleEffect != null)
            {
                ParticleSystem particle = Instantiate(changeSoilParticleEffect, position, Quaternion.identity);
                particle.Play();
            }
        }
    }
}
