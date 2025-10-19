using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioClipData", menuName = "Audio/AudioClipData")]
public class AudioClipData : ScriptableObject
{
    public string clipName;
    public AudioClip clip;

    [Header("기본 설정")]
    public bool isLoop;
    public float volume = 1f;
    public float pitch = 1f;

    [Header("랜덤 옵션")]
    public bool useRandomPitch;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
}

