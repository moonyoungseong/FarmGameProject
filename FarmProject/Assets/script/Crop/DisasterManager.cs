using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public Crop crop;

    // 자연재해 발생 (랜덤)
    public void TriggerDisaster()
    {
        // 자연재해 확률 (0~1 사이의 랜덤 값으로 결정)
        float disasterChance = Random.Range(0f, 1f);

        if (disasterChance > 0.7f) // 예: 70% 확률로 자연재해 발생
        {
            Debug.Log("자연재해가 발생했습니다!");
            crop.ApplyDisaster(); // 자연재해 처리
        }
    }
}
