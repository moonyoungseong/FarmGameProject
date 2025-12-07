using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// PlayTimeTracker.cs
/// 플레이 시간을 자동으로 누적·저장
/// UI에 실시간으로 표시하는 클래스
/// </summary>
public class PlayTimeTracker : MonoBehaviour
{
    // 누적된 플레이 시간을 표시할 UI
    public TextMeshProUGUI playTimeText;

    // 실제로 누적되는 플레이 시간(초 단위)
    private float playTime;

    /// <summary>
    /// 저장된 플레이 시간을 불러오는 초기 작업.
    /// 게임 실행 시 이전 플레이 기록을 이어받는다.
    /// </summary>
    void Start()
    {
        playTime = PlayerPrefs.GetFloat("PlayTime", 0f);
    }

    // 매 프레임 플레이 시간을 누적하고 PlayerPrefs에 저장한 뒤 UI 텍스트를 갱신
    void Update()
    {
        playTime += Time.deltaTime;

        // 현재 플레이 시간을 저장하여 종료 후에도 유지되도록 함
        PlayerPrefs.SetFloat("PlayTime", playTime);

        UpdatePlayTimeText();
    }

    /// <summary>
    /// 누적 시간(초)을 시, 분, 초 단위로 변환 후
    /// TextMeshPro UI에 표시하는 역할을 수행.
    /// </summary>
    void UpdatePlayTimeText()
    {
        int hours = Mathf.FloorToInt(playTime / 3600);
        int minutes = Mathf.FloorToInt((playTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(playTime % 60);

        playTimeText.text = $"플레이 시간: {hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    /// <summary>
    /// 플레이 타이머를 완전히 초기화한다.
    /// PlayerPrefs 저장값도 0으로 재설정한 뒤 UI를 업데이트
    /// </summary>
    public void ResetPlayTime()
    {
        playTime = 0f;
        PlayerPrefs.SetFloat("PlayTime", 0f);
        UpdatePlayTimeText();
    }
}
