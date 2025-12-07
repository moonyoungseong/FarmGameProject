using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AudioClipData.cs
/// 개별 오디오 클립의 설정값을 저장하는 데이터 오브젝트
/// 
/// - 볼륨, 피치, 루프 여부 등 재생 설정을 소리마다 다르게 적용
/// - 랜덤 피치 옵션을 사용하면 자연스러운 음향 연출
/// </summary>
[CreateAssetMenu(fileName = "NewAudioClipData", menuName = "Audio/AudioClipData")]
public class AudioClipData : ScriptableObject
{
    public string clipName;   
    public AudioClip clip;    // 재생할 오디오 클립 파일

    [Header("기본 설정")]
    public bool isLoop;       
    public float volume = 1f; 
    public float pitch = 1f;  // 기본 재생 피치 

    [Header("랜덤 옵션")]
    public bool useRandomPitch;  // 랜덤 피치 사용 여부
    public float minPitch = 0.8f; 
    public float maxPitch = 1.2f; 
}
