using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farming Game/Crop", order = 1)]
public class CropAttributes : ScriptableObject
{
    public string name;                   // �۹� �̸�
    public int id;                        // �۹� ID
    public int seedCost;                  // ���� ����
    public int sellPrice;                 // �Ǹ� ����

    public float growthTime;              // ���� �ð�
    public int growthStages;              // ���� �ܰ� ��
    public int waterRequirement;          // �ʿ��� ���� ��

    public int yieldAmount;               // ��Ȯ��
    public GameObject seedPrefab;    // ���� ���� ������
    public GameObject growthPrefab;  // ���� �� ���� ������
    public GameObject fullyGrownPrefab; // ������ �ڶ� ���� ������
}