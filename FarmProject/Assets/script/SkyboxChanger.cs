using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public PlayTimeTracker playTimeTracker;
    public Material daySkybox;
    public Material nightSkybox;
    public float switchTime = 300f;

    private bool isDay = true;
    private float dayNightTimer = 0f; //  ���� �д�!

    void Start()
    {
        // ���� �������ڸ��� �� skybox ����
        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment();
    }

    void Update()
    {
        if (playTimeTracker == null) return;

        dayNightTimer += Time.deltaTime; // ���� ���� Ÿ�̸�

        if (isDay && dayNightTimer >= switchTime)
        {
            RenderSettings.skybox = nightSkybox;
            DynamicGI.UpdateEnvironment();
            isDay = false;
        }
        else if (!isDay && dayNightTimer >= switchTime * 2)
        {
            RenderSettings.skybox = daySkybox;
            DynamicGI.UpdateEnvironment();
            isDay = true;
            dayNightTimer = 0f; // ���� Ÿ�̸Ӹ� �ʱ�ȭ
        }
    }
}

