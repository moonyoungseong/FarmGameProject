using UnityEngine;

public class FarmController : MonoBehaviour
{
    public CropFactory cropFactory;   // CropFactory를 참조
    public CropAttributes CornAttributes;  // 옥수수 작물 속성 데이터
    public CropAttributes TomatoAttributes;  // 토마토 작물 속성 데이터
    public CropAttributes RiceAttributes;  // 쌀 작물 속성 데이터

    public LayerMask groundLayer;  // 바닥 레이어를 설정

    void Update()
    {
        // 마우스 왼쪽 클릭을 감지
        if (Input.GetMouseButtonDown(0))  // 0은 마우스 왼쪽 버튼
        {
            Vector3 mousePosition = GetMouseWorldPosition();  // 마우스 위치를 월드 좌표로 변환
            cropFactory.CreateCrop(CornAttributes, mousePosition);  // 옥수수 작물을 해당 위치에 생성
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
}
