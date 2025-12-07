using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// FarmTileChanger.cs
/// 밭 타일 위에서 플레이어가 사용하는 도구(괭이, 삽)에 따라 밭의 상태를 변경하는 시스템
///
/// - 마우스 클릭 시 밭 레이어를 검사하여 타일 교체
/// - CursorIcons을 통해 어떤 도구가 선택되었는지 판별
/// - 파티클 효과 및 사운드 재생
/// </summary>
public class FarmTileChanger : MonoBehaviour
{
    public GameObject plowedSoilPrefab;  // 괭이 사용 시 생성될 밭 프리팹
    public GameObject dugSoilPrefab;     // 삽 사용 시 생성될 밭 프리팹

    private int farmTileLayer;           // 기본 밭 레이어
    private int plowedTileLayer;         // 괭이 사용 후 레이어
    private int dugTileLayer;            // 삽 사용 후 레이어

    public CursorIcons cursorIcons;      // 현재 선택된 커서 아이콘 관리

    public ParticleSystem changeSoilParticleEffect; // 밭 변경 효과 파티클


    // 초기 레이어 값을 설정한다.
    private void Start()
    {
        farmTileLayer = LayerMask.NameToLayer("FarmTile");
        plowedTileLayer = LayerMask.NameToLayer("PlowedTile");
        dugTileLayer = LayerMask.NameToLayer("DugTile");
    }


    // 매 프레임마다 마우스 클릭을 체크하고 UI를 클릭 중이면 무시
    private void Update()
    {
        // UI 클릭 중이면 밭 시스템 작동 안함
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이캐스트로 밭 타일 검사
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                int hitLayer = hit.collider.gameObject.layer;
                ChangeFarmTile(hit.collider.gameObject, hitLayer, hit.point);
            }
        }
    }


    // 도구와 타일 레이어를 비교하여 어떤 상태의 밭으로 바꿀지 결정
    private void ChangeFarmTile(GameObject farmTile, int tileLayer, Vector3 position)
    {
        DefaultCursorManager defaultCursorManager = DefaultCursorManager.instance;

        // 필수 객체 누락 체크
        if (defaultCursorManager == null || cursorIcons == null)
        {
            Debug.LogError("DefaultCursorManager 또는 CursorIcons가 null입니다.");
            return;
        }

        // 현재 커서 가져오기
        Texture2D defaultCursor = defaultCursorManager.GetCurrentCursor();
        Texture2D currentCursor = cursorIcons.GetCurrentCursor();

        if (currentCursor == null)
            currentCursor = defaultCursor;

        // 괭이 사용
        if (currentCursor == cursorIcons.FarmTool1 && tileLayer == farmTileLayer)
        {
            ReplaceFarmTile(farmTile, plowedSoilPrefab, position);
        }
        // 삽 사용
        else if (currentCursor == cursorIcons.shovel && tileLayer == plowedTileLayer)
        {
            ReplaceFarmTile(farmTile, dugSoilPrefab, position);
        }
        // 삽 사용
        else if (currentCursor == cursorIcons.shovel && tileLayer == dugTileLayer)
        {
            ReplaceFarmTile(farmTile, null, position);
        }
    }

    /// <summary>
    /// 기존 타일을 삭제하고 새로운 타일로 교체한다.
    /// 파티클 생성 및 효과음 재생까지 담당한다.
    /// </summary>
    /// <param name="oldTile">기존 타일</param>
    /// <param name="newPrefab">교체될 프리팹 (null이면 기본 밭으로 리셋)</param>
    /// <param name="position">파티클 생성 위치</param>
    private void ReplaceFarmTile(GameObject oldTile, GameObject newPrefab, Vector3 position)
    {
        if (newPrefab != null)
        {
            // 기존 타일 위치 그대로 새 타일 생성
            GameObject newTile = Instantiate(newPrefab, oldTile.transform.position, oldTile.transform.rotation);
            newTile.transform.SetParent(oldTile.transform.parent);
        }

        // 기존 타일 삭제
        Destroy(oldTile);

        // 파티클 & 사운드
        if (changeSoilParticleEffect != null)
        {
            ParticleSystem particle = Instantiate(changeSoilParticleEffect, position, Quaternion.identity);
            particle.Play();
            AudioManager.Instance.PlaySFX(14);
        }
    }
}
