
using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour
{
    public int Level { get; private set; }
    public int XP { get; private set; }
    public int XPForNextLevel { get; private set; }

    public List<PlayerChestState> chestStates;


    [Header("XP Configuration")]
    [SerializeField] private int baseXpRequirement = 100;
    [SerializeField] private float xpMultiplierPerLevel = 1.5f;
    [SerializeField] private float xpPerScorePoint = 0.1f;
    [SerializeField] private int gemsPerLevel = 5;

    public static event Action<int> OnLevelChanged;
    public static event Action<int, int> OnXPChanged;

    private CurrencyManager currencyManager;

    private void Awake()
    {
        ServiceLocator.Register<PlayerDataManager>(this);
        LoadData();
    }

    private void Start()
    {
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager not found in ServiceLocator. PlayerDataManager requires it to award gems on level up.");
        }
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PlayerDataManager>();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void AddXPFromRun(int scoreFromRun)
    {
        if (scoreFromRun <= 0) return;

        int xpGained = Mathf.FloorToInt(scoreFromRun * xpPerScorePoint);
        if (xpGained <= 0) return;

        XP += xpGained;
        Debug.Log($"Granted {xpGained} XP for a score of {scoreFromRun}.");

        CheckForLevelUp();
        OnXPChanged?.Invoke(XP, XPForNextLevel);
    }

    public PlayerChestState GetChestState(string chestId)
    {
        return chestStates.Find(c => c.chestId == chestId);
    }

    public void UpdateChestState(string chestId)
    {
        PlayerChestState chestState = GetChestState(chestId);
        if (chestState != null)
        {
            chestState.lastOpenedTime = DateTime.UtcNow;
        } else {
            chestStates.Add(new PlayerChestState { chestId = chestId, lastOpenedTime = DateTime.UtcNow });
        }
        SaveData();
    }


    private void CheckForLevelUp()
    {
        while (XP >= XPForNextLevel)
        {
            XP -= XPForNextLevel;
            Level++;
            UpdateXpRequirement();

            if (currencyManager != null)
            {
                currencyManager.AddGems(gemsPerLevel);
                Debug.Log($"{gemsPerLevel} gems awarded for reaching Level {Level}.");
            }
            
            Debug.Log($"Player leveled up to Level {Level}!");
            OnLevelChanged?.Invoke(Level);
        }
    }

    private void UpdateXpRequirement()
    {
        XPForNextLevel = GetXPForLevel(Level + 1);
    }

    private int GetXPForLevel(int level)
    {
        if (level <= 1) return baseXpRequirement;
        return (int)(baseXpRequirement * Mathf.Pow(level, xpMultiplierPerLevel));
    }

    private void LoadData()
    {
        Level = PlayerPrefs.GetInt("PlayerLevel", 1);
        XP = PlayerPrefs.GetInt("PlayerXP", 0);
        string chestStatesJson = PlayerPrefs.GetString("ChestStates", "");
        if (!string.IsNullOrEmpty(chestStatesJson))
        {
            chestStates = JsonUtility.FromJson<List<PlayerChestState>>(chestStatesJson);
        }
        else
        {
            chestStates = new List<PlayerChestState>();
        }
        UpdateXpRequirement();

        OnLevelChanged?.Invoke(Level);
        OnXPChanged?.Invoke(XP, XPForNextLevel);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("PlayerLevel", Level);
        PlayerPrefs.SetInt("PlayerXP", XP);
        string chestStatesJson = JsonUtility.ToJson(chestStates);
        PlayerPrefs.SetString("ChestStates", chestStatesJson);
        PlayerPrefs.Save();
        Debug.Log("Player data saved successfully.");
    }
}
