using UnityEngine;
using UnityEngine.UI;

public class SpeakerController : MonoBehaviour
{
    public Sprite iconOn;  // �Ҹ� ���� ������
    public Sprite iconOff; // ���Ұ� ������
    public Image muteButtonImage; // ��ư�� �̹��� ������Ʈ
    public Slider volumeSlider; // ���� �����̴�

    private bool isMuted = false;
    private float previousVolume = 1f; // ���Ұ� ���� ����� ���� ��

    void Start()
    {
        if (volumeSlider != null)
        {
            previousVolume = volumeSlider.value; // ���� ���� �� ����
            volumeSlider.onValueChanged.AddListener(SetVolume); // �����̴� ���� ����
        }
        UpdateIcon();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            previousVolume = volumeSlider.value; // ���� ���� ����
            volumeSlider.value = 0f; // �����̴� ���� 0���� ����
            AudioListener.volume = 0f; // ����� ���� 0
        }
        else
        {
            volumeSlider.value = previousVolume; // ���� �������� ����
            AudioListener.volume = previousVolume; // ����� ���� ����
        }

        UpdateIcon();
    }

    void SetVolume(float value)
    {
        if (!isMuted) // ���Ұ� ���°� �ƴ� ���� ���� ���� ����
        {
            AudioListener.volume = value;
            previousVolume = value; // ���ο� ���� �� ����
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
