using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FarmController : MonoBehaviour
{
    public CropFactory cropFactory;         // CropFactory를 참조
    public CropAttributes CornAttributes;   // 옥수수 작물 속성 데이터
    public CropAttributes TomatoAttributes; // 토마토 작물 속성 데이터
    public CropAttributes RiceAttributes;   // 쌀 작물 속성 데이터

    public LayerMask groundLayer;           // 바닥 레이어 설정
    public Transform player;                // 플레이어의 Transform
    public float maxPlantDistance = 5f;     // 작물을 심을 수 있는 최대 거리
    public float maxPlantAngle = 30f;       // 플레이어 전방 기준 허용 각도
    public float plantCooldown = 1f;        // 작물을 심을 수 있는 대기시간
    public float minPlantDistance = 1f;     // 다른 작물과의 최소 거리 제한

    private float lastPlantTime = 0f;       // 마지막으로 작물을 심은 시간
    private List<Vector3> plantedCropPositions = new List<Vector3>();
    private PlayerMove playerMoveScript;

    // 현재 선택된 작물을 관리할 변수
    private CropAttributes selectedCropAttributes;

    void Start()
    {
        playerMoveScript = player.GetComponent<PlayerMove>();
        selectedCropAttributes = null; // 기본 상태에서는 아무 것도 심지 않음
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && CanPlant())
        {
            if (selectedCropAttributes == null)
            {
                Debug.Log("선택된 작물이 없습니다.");
                return; // 작물이 선택되지 않은 경우 심기 취소
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
        Debug.Log("옥수수를 선택했습니다.");
    }

    public void SelectTomato()
    {
        selectedCropAttributes = TomatoAttributes;
        Debug.Log("토마토를 선택했습니다.");
    }

    public void SelectRice()
    {
        selectedCropAttributes = RiceAttributes;
        Debug.Log("쌀을 선택했습니다.");
    }

    public void SelectNone()
    {
        selectedCropAttributes = null;
        Debug.Log("아무 것도 선택하지 않았습니다.");
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
                Debug.Log("다른 작물이 너무 가까워서 심을 수 없습니다.");
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
