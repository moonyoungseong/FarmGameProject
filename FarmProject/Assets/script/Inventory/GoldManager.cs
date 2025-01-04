using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; } = 1000;
    public Text goldText; // UI �ؽ�Ʈ ������Ʈ ����
    public GameObject insufficientGoldUI; // ������ ��� UI
    public float uiDisplayTime = 2f; // UI�� ���̴� �ð� (��)

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
    }

    public IEnumerator ShowInsufficientGoldUI()
    {
        insufficientGoldUI.SetActive(true); // ������ ��� UI Ȱ��ȭ
        yield return new WaitForSeconds(uiDisplayTime); // ���� �ð� ��ٸ�
        insufficientGoldUI.SetActive(false); // UI ��Ȱ��ȭ
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

    public int GetGold()
    {
        return Gold; // ���� ��带 ��ȯ�ϴ� �Լ�
    }
}
