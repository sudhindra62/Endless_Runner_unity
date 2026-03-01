
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public static event Action<int> OnCoinsChanged;
    public static event Action<int> OnGemsChanged;
    public static event Action<int> OnXpChanged;

    [Header("Initial Values")]
    [SerializeField] private int startingCoins = 100;
    [SerializeField] private int startingGems = 20;

    [Header("Flow Combo Settings")]
    [SerializeField] private int coinStreakThreshold = 20;
    private int currentCoinStreak = 0;

    public int Coins { get; private set; }
    public int Gems { get; private set; }
    public int XP { get; private set; }

    private readonly Dictionary<string, float> coinMultipliers = new Dictionary<string, float>();
    private readonly Dictionary<string, float> gemMultipliers = new Dictionary<string, float>();
    private readonly Dictionary<string, float> xpMultipliers = new Dictionary<string, float>();

    private const string CoinsSaveKey = "PlayerCoins";
    private const string GemsSaveKey = "PlayerGems";
    private const string XpSaveKey = "PlayerXP";

    protected override void Awake()
    {
        base.Awake();
        LoadData();
        currentCoinStreak = 0;
    }

    public void AddCoins(int baseAmount)
    {
        if (baseAmount <= 0) return;
        float finalAmount = baseAmount;
        foreach (var mult in coinMultipliers.Values) { finalAmount *= mult; }
        Coins += (int)finalAmount;
        OnCoinsChanged?.Invoke(Coins);
        SaveData();

        // --- Coin Streak Logic ---
        currentCoinStreak += baseAmount;
        if (currentCoinStreak >= coinStreakThreshold)
        {
            int combosToAdd = currentCoinStreak / coinStreakThreshold;
            for(int i = 0; i < combosToAdd; i++)
            {
                 FlowComboManager.Instance.AddToCombo();
            }
            Debug.Log($"Coin streak threshold reached! Added {combosToAdd} to combo.");
            currentCoinStreak %= coinStreakThreshold;
        }
    }

    public void ResetCoinStreak()
    {
        if (currentCoinStreak > 0)
        {
            Debug.Log($"Coin streak of {currentCoinStreak} broken.");
            currentCoinStreak = 0;
        }
    }

    public void AddGems(int baseAmount)
    {
        if (baseAmount <= 0) return;
        float finalAmount = baseAmount;
        foreach (var mult in gemMultipliers.Values) { finalAmount *= mult; }
        Gems += (int)finalAmount;
        OnGemsChanged?.Invoke(Gems);
        SaveData();
    }

    public void AddXP(int baseAmount)
    {
        if (baseAmount <= 0) return;
        float finalAmount = baseAmount;
        foreach (var mult in xpMultipliers.Values) { finalAmount *= mult; }
        XP += (int)finalAmount;
        OnXpChanged?.Invoke(XP);
        SaveData();
    }

    public void ApplyCoinMultiplier(string sourceId, float multiplier) => coinMultipliers[sourceId] = multiplier;
    public void RemoveCoinMultiplier(string sourceId) => coinMultipliers.Remove(sourceId);

    public void ApplyGemMultiplier(string sourceId, float multiplier) => gemMultipliers[sourceId] = multiplier;
    public void RemoveGemMultiplier(string sourceId) => gemMultipliers.Remove(sourceId);

    public void ApplyXpMultiplier(string sourceId, float multiplier) => xpMultipliers[sourceId] = multiplier;
    public void RemoveXpMultiplier(string sourceId) => xpMultipliers.Remove(sourceId);

    private void LoadData()
    {
        Coins = PlayerPrefs.GetInt(CoinsSaveKey, startingCoins);
        Gems = PlayerPrefs.GetInt(GemsSaveKey, startingGems);
        XP = PlayerPrefs.GetInt(XpSaveKey, 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(CoinsSaveKey, Coins);
        PlayerPrefs.SetInt(GemsSaveKey, Gems);
        PlayerPrefs.SetInt(XpSaveKey, XP);
    }
}
