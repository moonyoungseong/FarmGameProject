using UnityEngine;
using System;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; } = 1000;
    public Text goldText; // UI �ؽ�Ʈ ������Ʈ ����

    public event Action<int> OnGoldChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGold(); // ����� ��� ������ �ҷ�����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GoldManager.Instance.OnGoldChanged += UpdateGoldUI;
        UpdateGoldUI(GoldManager.Instance.Gold); // �ʱ� UI ������Ʈ
    }


    public void AddGold(int amount)
    {
        Gold += amount;
        SaveGold();
        OnGoldChanged?.Invoke(Gold);
    }

    void UpdateGoldUI(int currentGold)
    {
        goldText.text = currentGold.ToString() + "$"; // goldText�� UI �ؽ�Ʈ ������Ʈ
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
            Debug.LogWarning("��尡 �����մϴ�!");
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
            Gold = 1000; // �⺻��
        }
    }
}
