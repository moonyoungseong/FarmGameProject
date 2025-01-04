using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; } = 1000;
    public Text goldText; // UI 텍스트 컴포넌트 참조
    public GameObject insufficientGoldUI; // 부족한 골드 UI
    public float uiDisplayTime = 2f; // UI가 보이는 시간 (초)

    public event Action<int> OnGoldChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGold(); // 저장된 골드 데이터 불러오기
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GoldManager.Instance.OnGoldChanged += UpdateGoldUI;
        UpdateGoldUI(GoldManager.Instance.Gold); // 초기 UI 업데이트
    }


    public void AddGold(int amount)
    {
        Gold += amount;
        SaveGold();
        OnGoldChanged?.Invoke(Gold);
    }

    void UpdateGoldUI(int currentGold)
    {
        goldText.text = currentGold.ToString() + "$"; // goldText는 UI 텍스트 컴포넌트
    }

    public void SubtractGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            SaveGold();
            OnGoldChanged?.Invoke(Gold);
        }
    }

    public IEnumerator ShowInsufficientGoldUI()
    {
        insufficientGoldUI.SetActive(true); // 부족한 골드 UI 활성화
        yield return new WaitForSeconds(uiDisplayTime); // 일정 시간 기다림
        insufficientGoldUI.SetActive(false); // UI 비활성화
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt("Gold", Gold);
        PlayerPrefs.Save();
    }

    private void LoadGold()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            Gold = PlayerPrefs.GetInt("Gold");
        }
        else
        {
            Gold = 1000; // 기본값
        }
    }

    public int GetGold()
    {
        return Gold; // 현재 골드를 반환하는 함수
    }
}
