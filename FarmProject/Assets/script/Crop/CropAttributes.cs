using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farming Game/Crop", order = 1)]
public class CropAttributes : ScriptableObject
{
    public string name;                   // 작물 이름
    public int id;                        // 작물 ID
    public int seedCost;                  // 씨앗 가격
    public int sellPrice;                 // 판매 가격

    public float growthTime;              // 성장 시간
    public int growthStages;              // 성장 단계 수
    public int waterRequirement;          // 필요한 물의 양

    public int yieldAmount;               // 수확량
    public GameObject seedPrefab;    // 씨앗 상태 프리팹
    public GameObject growthPrefab;  // 성장 중 상태 프리팹
    public GameObject fullyGrownPrefab; // 완전히 자란 상태 프리팹
}