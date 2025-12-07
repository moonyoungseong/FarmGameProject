using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CropAttributes.cs
/// 작물 속성 정보를 담는 ScriptableObject
/// 씨앗, 성장 단계 등 작물 관련 데이터를 관리
/// ScriptableObject로 만들어서 에디터에서 쉽게 데이터 생성/수정 가능
/// </summary>
[CreateAssetMenu(fileName = "New Crop", menuName = "Farming Game/Crop", order = 1)]
public class CropAttributes : ScriptableObject
{
    // 기본 정보
    public string SeedName;   // 씨앗 이름
    public string name;       // 작물 이름
    public string id;         // 작물 고유 ID

    // 경제적 정보
    public int seedCost;      // 씨앗 구매 가격
    public int sellPrice;     // 판매 가격

    // 성장 정보
    public float growthTime;  // 총 성장 시간
    public int growthStages;  // 성장 단계 수

    // 수확 및 프리팹
    public GameObject seedPrefab;     // 씨앗 상태 프리팹
    public GameObject growthPrefab;   // 성장 중 상태 프리팹
    public GameObject fullyGrownPrefab; // 완전히 자란 상태 프리팹
}
