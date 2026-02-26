using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int Coins { get; private set; }
    public int Gems { get; private set; }

    private const string CoinsKey = "PlayerCoins";
    private const string GemsKey = "PlayerGems";

    private void Awake()
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

    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(CoinsKey, 0);
        Gems = PlayerPrefs.GetInt(GemsKey, 0);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();
    }

    public void AddGems(int amount)
    {
        Gems += amount;
        PlayerPrefs.SetInt(GemsKey, Gems);
        PlayerPrefs.Save();
    }

    public int GetCoinBalance() => Coins;
    public int GetGemBalance() => Gems;

    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        AddCoins(-amount);
        return true;
    }

    public bool SpendGems(int amount)
    {
        if (Gems < amount) return false;
        AddGems(-amount);
        return true;
    }
}
