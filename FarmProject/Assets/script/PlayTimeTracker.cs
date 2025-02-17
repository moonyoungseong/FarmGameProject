using UnityEngine;
using TMPro;

public class PlayTimeTracker : MonoBehaviour
{
    public TextMeshProUGUI playTimeText; // UI 텍스트 연결

    private float playTime;

    void Start()
    {
        playTime = PlayerPrefs.GetFloat("PlayTime", 0f);
    }

    void Update()
    {
        playTime += Time.deltaTime;
        PlayerPrefs.SetFloat("PlayTime", playTime);

        UpdatePlayTimeText();
    }

    void UpdatePlayTimeText()
    {
        int hours = Mathf.FloorToInt(playTime / 3600);
        int minutes = Mathf.FloorToInt((playTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(playTime % 60);

        playTimeText.text = $"플레이 시간: {hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    public void ResetPlayTime()
    {
        playTime = 0f;
        PlayerPrefs.SetFloat("PlayTime", 0f);
        UpdatePlayTimeText();
    }
}
