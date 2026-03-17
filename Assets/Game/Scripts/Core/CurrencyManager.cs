using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    private const string COINS_KEY = "PlayerCoins";
    private const string GEMS_KEY = "PlayerGems";

    public int Coins { get; private set; }
    public int Gems { get; private set; }

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

    public void AddCoins(int amount)
    {
        Coins += amount;
        SaveCurrency();
    }

    public void AddGems(int amount)
    {
        Gems += amount;
        SaveCurrency();
    }

    public bool SpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            SaveCurrency();
            return true;
        }
        return false;
    }

    public bool SpendGems(int amount)
    {
        if (Gems >= amount)
        {
            Gems -= amount;
            SaveCurrency();
            return true;
        }
        return false;
    }

    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(COINS_KEY, 0);
        Gems = PlayerPrefs.GetInt(GEMS_KEY, 0);
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(COINS_KEY, Coins);
        PlayerPrefs.SetInt(GEMS_KEY, Gems);
        PlayerPrefs.Save();
    }
}
