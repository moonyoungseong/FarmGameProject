using UnityEngine;
using System.Collections.Generic;  // List 사용

public class FarmController : MonoBehaviour
{
    public CropFactory cropFactory;      // CropFactory를 참조
    public CropAttributes CornAttributes;  // 옥수수 작물 속성 데이터

    public LayerMask groundLayer;        // 바닥 레이어 설정
    public Transform player;             // 플레이어의 Transform
    public float maxPlantDistance = 5f;  // 작물을 심을 수 있는 최대 거리
    public float maxPlantAngle = 30f;    // 플레이어 전방 기준 허용 각도 (30도: 정면만 허용)
    public float plantCooldown = 1f;     // 작물을 심을 수 있는 대기시간 (초 단위)
    public float minPlantDistance = 1f;  // 다른 작물과의 최소 거리 제한

    private float lastPlantTime = 0f;    // 마지막으로 작물을 심은 시간 기록
    private List<Vector3> plantedCropPositions = new List<Vector3>();  // 심어진 작물 위치 저장

    void Update()
    {
        // 마우스 왼쪽 클릭을 감지
        if (Input.GetMouseButtonDown(0) && CanPlant())
        {
            Vector3 mousePosition = GetMouseWorldPosition();  // 마우스 위치를 월드 좌표로 변환
            if (IsWithinPlantingRange(mousePosition) && IsFarEnoughFromOtherCrops(mousePosition))  // 거리 및 각도 확인
            {
                cropFactory.CreateCrop(CornAttributes, mousePosition);  // 옥수수 작물을 생성
                plantedCropPositions.Add(mousePosition);  // 생성된 작물 위치를 리스트에 추가
                lastPlantTime = Time.time;  // 마지막 심은 시간 기록
            }
        }
    }

    // 마우스의 화면 좌표를 월드 좌표로 변환 후, 레이캐스트로 바닥에 충돌한 위치 반환
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // 마우스 화면 좌표에서 레이 생성
        RaycastHit hit;

        // 레이캐스트가 바닥 레이어와 충돌하면, 해당 지점에 월드 좌표 반환
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;  // 레이가 충돌한 지점의 월드 좌표
        }

        return Vector3.zero;  // 충돌이 없으면 (예: 바닥이 아닌 곳 클릭 시) (0,0,0) 반환
    }

    // 클릭 위치가 플레이어 전방 허용 거리와 각도 내인지 확인
    bool IsWithinPlantingRange(Vector3 position)
    {
        // 거리 계산
        float distance = Vector3.Distance(player.position, position);

        // 방향 벡터 계산
        Vector3 directionToMouse = (position - player.position).normalized;

        // 플레이어 전방 기준 각도 계산
        float angle = Vector3.Angle(player.forward, directionToMouse);

        // 거리와 각도 모두 만족 (정면에서만 가능)
        return distance <= maxPlantDistance && angle <= maxPlantAngle;
    }

    // 클릭 위치가 다른 작물 위치와의 최소 거리보다 충분히 먼지 확인
    bool IsFarEnoughFromOtherCrops(Vector3 position)
    {
        foreach (Vector3 cropPosition in plantedCropPositions)
        {
            if (Vector3.Distance(position, cropPosition) < minPlantDistance)
            {
                Debug.Log("다른 작물이 너무 가까워서 심을 수 없습니다.");
                return false;  // 다른 작물과의 거리가 너무 가까우면 false 반환
            }
        }
        return true;  // 모든 작물과의 거리가 충분히 멀면 true 반환
    }

    // 대기시간이 지난 경우에만 true 반환
    bool CanPlant()
    {
        return Time.time >= lastPlantTime + plantCooldown;
    }
}
