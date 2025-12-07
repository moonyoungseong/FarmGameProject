using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SpeakerController.cs
/// 전체 게임 오디오 볼륨을 관리하고, 음소거 및 아이콘 변경을 담당하는 스크립트.
/// 볼륨 슬라이더와 소리 on/off 버튼을 연동하여 사용
/// </summary>
public class SpeakerController : MonoBehaviour
{
    public Sprite iconOn;            // 소리가 켜진 상태의 아이콘
    public Sprite iconOff;           // 음소거 상태의 아이콘
    public Image muteButtonImage;    // UI 버튼의 이미지 (아이콘 변경용)
    public Slider volumeSlider;      // 볼륨 슬라이더 UI

    private bool isMuted = false;    // 현재 음소거 상태 여부
    private float previousVolume = 1f; // 음소거 전 볼륨값 저장

    void Start()
    {
        if (volumeSlider != null)
        {
            previousVolume = volumeSlider.value; // 현재 슬라이더 볼륨 저장
            volumeSlider.onValueChanged.AddListener(SetVolume); // 슬라이더 값 변경 감지
        }
        UpdateIcon();
    }

    // 소리 켜기/끄기 버튼 클릭 시 실행되는 함수
    public void ToggleSound()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            previousVolume = volumeSlider.value; // 현재 볼륨 저장
            volumeSlider.value = 0f;             // 슬라이더 0으로
            AudioListener.volume = 0f;           // 실제 오디오 볼륨 0
        }
        else
        {
            volumeSlider.value = previousVolume; // 이전 볼륨으로 복구
            AudioListener.volume = previousVolume;
        }

        UpdateIcon();
    }

    // 슬라이더가 움직일 때 자동으로 호출되는 함수.
    void SetVolume(float value)
    {
        if (!isMuted)
        {
            AudioListener.volume = value;
            previousVolume = value; // 새 볼륨 저장 
        }
    }

    /// <summary>
    /// 현재 isMuted 상태에 따라 버튼 아이콘을 변경하는 함수.
    /// iconOn, iconOff 로 전환한다.
    /// </summary>
    void UpdateIcon()
    {
        if (muteButtonImage != null)
        {
            muteButtonImage.sprite = isMuted ? iconOff : iconOn;
        }
    }
}
