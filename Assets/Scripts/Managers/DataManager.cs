using UnityEngine;

/// <summary>
/// High-level API for interacting with GameData.
/// Acts as a proxy for SaveManager to maintain AEIS structural integrity.
/// Global scope Singleton.
/// </summary>
public class DataManager : Singleton<DataManager> 
{ 
    public static event System.Action<int> OnCoinsChanged;
    public static event System.Action<int> OnGemsChanged;
    public GameData GameData => SaveManager.Instance.Data;
    public int Coins => SaveManager.Instance != null ? SaveManager.Instance.Data.totalCoins : 0;
    public int Gems => SaveManager.Instance != null ? SaveManager.Instance.Data.gems : 0;
    public void SaveGameData() => SaveManager.Instance.SaveGame();
    public void LoadData() => SaveManager.Instance.LoadGame();

    public void AddCoins(int amount)
    {
        if (GameManager.Instance != null) GameManager.Instance.AddCoins(amount);
        OnCoinsChanged?.Invoke(Coins);
    }

    public void AddGems(int amount)
    {
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.AddGems(amount);
            OnGemsChanged?.Invoke(Gems);
        }
    }

    public void ApplyCoinMultiplier(int multiplier) { /* Multiplier hooks */ }
    public void ApplyCoinMultiplier(int multiplier, float duration) => ApplyCoinMultiplier(multiplier);
    public void ApplyCoinMultiplier(string sourceId, float multiplier) => ApplyCoinMultiplier(Mathf.RoundToInt(multiplier));
    public void RemoveCoinMultiplier() { }
    public void RemoveCoinMultiplier(string sourceId) => RemoveCoinMultiplier();
}
