using UnityEngine;
using System;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; } = 1000;
    public Text goldText; // UI 텍스트 컴포넌트 참조

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
        else
        {
            Debug.LogWarning("골드가 부족합니다!");
        }
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
}
