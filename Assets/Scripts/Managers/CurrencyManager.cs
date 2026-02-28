
using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public static event Action<int> OnCoinsChanged;
    public static event Action<int> OnGemsChanged;

    public int Coins { get; private set; }
    public int Gems { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<CurrencyManager>(this);
            LoadCurrencies();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            ServiceLocator.Unregister<CurrencyManager>();
            Instance = null;
        }
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrencies();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || Coins < amount) return false;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrencies();
        return true;
    }

    public void AddGems(int amount)
    {
        if (amount <= 0) return;
        Gems += amount;
        OnGemsChanged?.Invoke(Gems);
        SaveCurrencies();
    }

    public bool SpendGems(int amount)
    {
        if (amount <= 0 || Gems < amount) return false;
        Gems -= amount;
        OnGemsChanged?.Invoke(Gems);
        SaveCurrencies();
        return true;
    }

    private void LoadCurrencies()
    {
        Coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        Gems = PlayerPrefs.GetInt("PlayerGems", 0);
        // Invoke events to update UI on load
        OnCoinsChanged?.Invoke(Coins);
        OnGemsChanged?.Invoke(Gems);
    }

    private void SaveCurrencies()
    {
        PlayerPrefs.SetInt("PlayerCoins", Coins);
        PlayerPrefs.SetInt("PlayerGems", Gems);
    }

    private void OnApplicationQuit()
    {
        SaveCurrencies();
        PlayerPrefs.Save();
    }
}
