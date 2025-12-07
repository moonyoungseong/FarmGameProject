using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SkyboxChanger.cs
/// 일정 시간이 지나면 낮, 밤 Skybox를 자동으로 전환하는 스크립트
/// PlayTimeTracker와는 독립적으로, 낮밤 전용 타이머를 사용
/// </summary>
public class SkyboxChanger : MonoBehaviour
{
    // 플레이 시간 트래커
    public PlayTimeTracker playTimeTracker;

    // 낮에 적용될 Skybox 머티리얼.
    public Material daySkybox;

    // 밤에 적용될 Skybox 머티리얼.
    public Material nightSkybox;

    // 전환되는 데 걸리는 시간
    public float switchTime = 300f;

    // 현재 Skybox가 낮 상태인지 여부를 저장
    private bool isDay = true;

    // 낮/밤 교체만을 위한 전용 타이머.
    private float dayNightTimer = 0f;

    // 시작 시 기본적으로 낮 Skybox를 적용한다.
    void Start()
    {
        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment();
    }

    // 매 프레임마다 낮/밤 타이머를 증가시키고 일정 시간이 되면 Skybox를 전환
    void Update()
    {
        if (playTimeTracker == null) return;

        // 낮밤 전용 타이머 증가
        dayNightTimer += Time.deltaTime;

        // 낮 → 밤 전환
        if (isDay && dayNightTimer >= switchTime)
        {
            RenderSettings.skybox = nightSkybox;
            DynamicGI.UpdateEnvironment();
            isDay = false;
        }
        // 밤 → 낮 전환
        else if (!isDay && dayNightTimer >= switchTime * 2)
        {
            RenderSettings.skybox = daySkybox;
            DynamicGI.UpdateEnvironment();
            isDay = true;

            // 낮/밤 타이머 초기화
            dayNightTimer = 0f;
        }
    }
}
