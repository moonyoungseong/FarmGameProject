using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// GoldManager.cs
/// 게임 내 골드(화폐)를 관리하는 매니저.
/// 
/// - 싱글톤으로 운영되어 씬 전환에도 유지됨
/// - 골드 증감, UI 갱신, PlayerPrefs 저장 및 로딩 기능 포함
/// - 골드 부족 시 UI 표시 기능 포함
/// </summary>
public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    // 시작 보유 골드
    public int Gold { get; private set; } = 1000;

    // 골드 UI 텍스트
    public Text goldText;

    // 골드 부족 시 표시할 UI 오브젝트
    public GameObject insufficientGoldUI;

    // 부족 골드 UI 표시 시간
    public float uiDisplayTime = 2f;

    // 골드 변경 이벤트
    public event Action<int> OnGoldChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGold(); // 저장된 골드 로딩
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // UI 초기 설정 및 이벤트 등록
    private void Start()
    {
        GoldManager.Instance.OnGoldChanged += UpdateGoldUI;
        UpdateGoldUI(GoldManager.Instance.Gold);
    }

    // 골드 추가, 저장, UI 갱신 이벤트 호출
    public void AddGold(int amount)
    {
        Gold += amount;
        SaveGold();
        OnGoldChanged?.Invoke(Gold);
    }

    // 골드 UI 텍스트 업데이트
    private void UpdateGoldUI(int currentGold)
    {
        goldText.text = currentGold.ToString() + "$";
    }

    // 골드 차감, 저장, UI 갱신 이벤트 호출
    public void SubtractGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            SaveGold();
            OnGoldChanged?.Invoke(Gold);
        }
    }

    // 일정 시간 동안 부족 골드 UI 표시
    public IEnumerator ShowInsufficientGoldUI()
    {
        insufficientGoldUI.SetActive(true);
        yield return new WaitForSeconds(uiDisplayTime);
        insufficientGoldUI.SetActive(false);
    }

    // PlayerPrefs에 골드 저장
    private void SaveGold()
    {
        PlayerPrefs.SetInt("Gold", Gold);
        PlayerPrefs.Save();
    }

    // PlayerPrefs에서 골드 로드, 없으면 기본값 1000 사용
    private void LoadGold()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            Gold = PlayerPrefs.GetInt("Gold");
        }
        else
        {
            Gold = 1000;
        }
    }

    // 현재 골드 반환
    public int GetGold()
    {
        return Gold;
    }
}
