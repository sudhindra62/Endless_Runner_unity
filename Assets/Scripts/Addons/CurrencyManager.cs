using UnityEngine;
using System;

public partial class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public static event Action<int> OnCoinsChanged;
    public static event Action<int> OnGemsChanged;

    public int Coins { get; private set; }
    public int Gems { get; private set; }

    private const string CoinsKey = "PlayerCurrency_Coins";
    private const string GemsKey = "PlayerCurrency_Gems";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCurrency();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        OnCoinsChanged?.Invoke(Coins);
        OnGemsChanged?.Invoke(Gems);
    }

    public void AddCoins(int amount)
    {
        if (amount == 0) return;
        Coins = Mathf.Max(0, Coins + amount);
        SaveCurrency();
        OnCoinsChanged?.Invoke(Coins);
    }

    public void AddGems(int amount)
    {
        if (amount == 0) return;
        Gems = Mathf.Max(0, Gems + amount);
        SaveCurrency();
        OnGemsChanged?.Invoke(Gems);
    }

    public bool CanAfford(int amount, string currencyType)
    {
        if (currencyType.ToLower() == "coins") return Coins >= amount;
        if (currencyType.ToLower() == "gems") return Gems >= amount;
        return false;
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.SetInt(GemsKey, Gems);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(CoinsKey, 0);
        Gems = PlayerPrefs.GetInt(GemsKey, 0);
    }
}
