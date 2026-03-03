
using System;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int Coins { get; private set; }
    public int Gems { get; private set; }

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnGemsChanged;

    private const string COINS_KEY = "PlayerCoins";
    private const string GEMS_KEY = "PlayerGems";

    private void Start()
    {
        LoadCurrency();
    }

    public void AddCoins(int amount)
    {
        if (amount < 0) return;
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrency();
    }

    public void SpendCoins(int amount)
    {
        if (amount < 0 || Coins < amount) return;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrency();
    }

    public void AddGems(int amount)
    {
        if (amount < 0) return;
        Gems += amount;
        OnGemsChanged?.Invoke(Gems);
        SaveCurrency();
    }

    public void SpendGems(int amount)
    {
        if (amount < 0 || Gems < amount) return;
        Gems -= amount;
        OnGemsChanged?.Invoke(Gems);
        SaveCurrency();
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(COINS_KEY, Coins);
        PlayerPrefs.SetInt(GEMS_KEY, Gems);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(COINS_KEY, 0);
        Gems = PlayerPrefs.GetInt(GEMS_KEY, 0);
    }
}
