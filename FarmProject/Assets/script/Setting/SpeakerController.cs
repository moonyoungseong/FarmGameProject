using UnityEngine;
using UnityEngine.UI;

public class SpeakerController : MonoBehaviour
{
    public Sprite iconOn;  // 소리 켜진 아이콘
    public Sprite iconOff; // 음소거 아이콘
    public Image muteButtonImage; // 버튼의 이미지 컴포넌트
    public Slider volumeSlider; // 볼륨 슬라이더

    private bool isMuted = false;
    private float previousVolume = 1f; // 음소거 전에 저장된 볼륨 값

    void Start()
    {
        if (volumeSlider != null)
        {
            previousVolume = volumeSlider.value; // 현재 볼륨 값 저장
            volumeSlider.onValueChanged.AddListener(SetVolume); // 슬라이더 변경 감지
        }
        UpdateIcon();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            previousVolume = volumeSlider.value; // 현재 볼륨 저장
            volumeSlider.value = 0f; // 슬라이더 값을 0으로 설정
            AudioListener.volume = 0f; // 오디오 볼륨 0
        }
        else
        {
            volumeSlider.value = previousVolume; // 이전 볼륨으로 복구
            AudioListener.volume = previousVolume; // 오디오 볼륨 복구
        }

        UpdateIcon();
    }

    void SetVolume(float value)
    {
        if (!isMuted) // 음소거 상태가 아닐 때만 볼륨 조절 가능
        {
            AudioListener.volume = value;
            previousVolume = value; // 새로운 볼륨 값 저장
        }
    }

    void UpdateIcon()
    {
        if (muteButtonImage != null)
        {
            muteButtonImage.sprite = isMuted ? iconOff : iconOn;
        }
    }
}
