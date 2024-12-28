using UnityEngine;

public class GameController : MonoBehaviour
{
    public CropFactory cropFactory;   // CropFactory를 참조
    public CropAttributes CornAttributes;  // 옥수수 작물 속성 데이터

    void Start()
    {
        // 예시: 게임 시작 시 쌀 작물을 생성
        cropFactory.CreateCrop(CornAttributes, new Vector3(0, 0, 0));
    }
}
