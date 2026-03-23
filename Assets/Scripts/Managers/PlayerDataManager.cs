
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages persistent player data like best score and best time.
/// </summary>
public class PlayerDataManager : MonoBehaviour
{

    public static PlayerDataManager Instance;
    private SaveManager saveManager;

    public static System.Action<int> OnCoinsChanged;
    public static System.Action<int> OnGemsChanged;
    public static System.Action<int> OnLevelUp;

    public int Coins => saveManager != null ? saveManager.Data.totalCoins : 0;
    public int Gems => saveManager != null ? saveManager.Data.gems : 0;
    public int Level => saveManager != null ? saveManager.Data.playerLevel : 1;
    public float XP => saveManager != null ? saveManager.Data.currentXP : 0f;
    public int XpForNextLevel => Mathf.Max(100, Level * 100);
    public event System.Action<int> OnLevelChanged;
    public event System.Action<float, int> OnXPChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        saveManager = SaveManager.Instance;
    }

    public void AddCoins(int amount) 
    {
        if (PlayerCoinManager.Instance != null) PlayerCoinManager.Instance.AddCoins(amount, true);
        else UpdateCurrency(CurrencyType.Coins, amount);
    }
    public void AddGems(int amount) => UpdateCurrency(CurrencyType.Gems, amount);
    public void UpdateCoins(int amount) => AddCoins(amount);
    public void UpdateGems(int amount) => AddGems(amount);
    public bool TrySpendGems(int amount) => SpendCurrency(CurrencyType.Gems, amount);

    // --- Generic Currency API for Folder 3 Sync ---
    public int GetCurrency(CurrencyType type)
    {
        if (saveManager == null) return 0;
        return type == CurrencyType.Coins ? saveManager.Data.totalCoins : saveManager.Data.gems;
    }

    public void AddCurrency(CurrencyType type, int amount) => UpdateCurrency(type, amount);
    public int GetTotalCoins() => Coins;

    public void UpdateCurrency(CurrencyType type, int amount)
    {
        if (saveManager == null) return;
        
        switch (type)
        {
            case CurrencyType.Coins:
                saveManager.Data.totalCoins += amount;
                break;
            case CurrencyType.Gems:
                saveManager.Data.gems += amount;
                break;
        }
        saveManager.SaveGame();
        
        // Notify managers
        if (type == CurrencyType.Coins)
        {
            if (PlayerCoinManager.Instance != null) PlayerCoinManager.Instance.UpdateCoins(0);
            OnCoinsChanged?.Invoke(saveManager.Data.totalCoins);
        }
        else if (type == CurrencyType.Gems)
        {
            OnGemsChanged?.Invoke(saveManager.Data.gems);
        }
    }

    public bool SpendCurrency(CurrencyType type, int amount)
    {
        if (saveManager == null) return false;

        switch (type)
        {
            case CurrencyType.Coins:
                if (saveManager.Data.totalCoins < amount) return false;
                saveManager.Data.totalCoins -= amount;
                break;
            case CurrencyType.Gems:
                if (saveManager.Data.gems < amount) return false;
                saveManager.Data.gems -= amount;
                break;
            default: return false;
        }
        saveManager.SaveGame();
        
        if (type == CurrencyType.Coins) OnCoinsChanged?.Invoke(saveManager.Data.totalCoins);
        else if (type == CurrencyType.Gems) OnGemsChanged?.Invoke(saveManager.Data.gems);
        
        return true;
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PlayerDataManager>();
    }

    public void UpdateBestScore(int score)
    {
        if (saveManager == null) return;
        if (score > saveManager.Data.highScore)
        {
            saveManager.Data.highScore = score;
            saveManager.SaveGame();
        }
    }

    public int GetBestScore()
    {
        return saveManager != null ? saveManager.Data.highScore : 0;
    }

    public void UpdateBestTime(float time)
    {
        if (saveManager == null) return;
        if (time > saveManager.Data.bestTime)
        {
            saveManager.Data.bestTime = time;
            saveManager.SaveGame();
        }
    }

    public float GetBestTime()
    {
        return saveManager != null ? saveManager.Data.bestTime : 0f;
    }

    public void SetPlayerLevel(int level)
    {
        if (saveManager == null) return;
        saveManager.Data.playerLevel = level;
        saveManager.SaveGame();
        OnLevelUp?.Invoke(level);
        OnLevelChanged?.Invoke(level);
    }

    public void AddXP(int amount)
    {
        if (saveManager == null) return;
        saveManager.Data.currentXP += amount;
        while (saveManager.Data.currentXP >= XpForNextLevel)
        {
            saveManager.Data.currentXP -= XpForNextLevel;
            saveManager.Data.playerLevel++;
            OnLevelUp?.Invoke(saveManager.Data.playerLevel);
            OnLevelChanged?.Invoke(saveManager.Data.playerLevel);
        }
        saveManager.SaveGame();
        OnXPChanged?.Invoke(saveManager.Data.currentXP, XpForNextLevel);
    }

    public void SyncRemoteData(PlayerData data)
    {
        if (saveManager == null || data == null) return;
        Debug.Log("[PlayerDataManager] Remote data sync initiated.");
    }

    public PlayerStats GetPlayerStats()
    {
        if (saveManager == null) return new PlayerStats();
        return new PlayerStats
        {
            coins = saveManager.Data.totalCoins,
            gems = saveManager.Data.gems,
            level = saveManager.Data.playerLevel
        };
    }

    // --- Inventory System ---
    public bool HasItem(string itemId)
    {
        if (saveManager == null || saveManager.Data.inventoryItemIds == null) return false;
        return saveManager.Data.inventoryItemIds.Contains(itemId);
    }

    public void AddItem(string itemId)
    {
        if (saveManager == null) return;
        if (saveManager.Data.inventoryItemIds == null) saveManager.Data.inventoryItemIds = new List<string>();
        
        if (!saveManager.Data.inventoryItemIds.Contains(itemId))
        {
            saveManager.Data.inventoryItemIds.Add(itemId);
            saveManager.SaveGame();
            Debug.Log($"Guardian Architect: Item {itemId} added to inventory.");
        }
    }

    public void RemoveItem(string itemId)
    {
        if (saveManager == null || saveManager.Data.inventoryItemIds == null) return;
        
        if (saveManager.Data.inventoryItemIds.Contains(itemId))
        {
            saveManager.Data.inventoryItemIds.Remove(itemId);
            saveManager.SaveGame();
            Debug.Log($"Guardian Architect: Item {itemId} consumed from inventory.");
        }
    }

    public List<string> GetInventory()
    {
        if (saveManager == null || saveManager.Data.inventoryItemIds == null) return new List<string>();
        return new List<string>(saveManager.Data.inventoryItemIds);
    }

    // --- Chest State API for RewardManager Sync (Folder 3) ---
    public PlayerChestState GetChestState(string id)
    {
        if (saveManager == null) return null;
        if (saveManager.Data.chestStates == null) saveManager.Data.chestStates = new List<PlayerChestState>();
        return saveManager.Data.chestStates.Find(c => c.chestId == id);
    }

    public void UpdateChestState(string id)
    {
        if (saveManager == null) return;
        if (saveManager.Data.chestStates == null) saveManager.Data.chestStates = new List<PlayerChestState>();
        
        PlayerChestState state = saveManager.Data.chestStates.Find(c => c.chestId == id);
        if (state == null)
        {
            state = new PlayerChestState { chestId = id, lastOpenedTime = System.DateTime.UtcNow };
            saveManager.Data.chestStates.Add(state);
        }
        else
        {
            state.lastOpenedTime = System.DateTime.UtcNow;
        }
        
        saveManager.SaveGame();
    }

    // --- Type Conversion Bridges (Phase 2A: Type Consistency) ---
    
    public void AddCoins(long coinAmount)
    {
        AddCoins((int)System.Math.Min(coinAmount, int.MaxValue));
    }

    public void AddGems(long gemAmount)
    {
        AddGems((int)System.Math.Min(gemAmount, int.MaxValue));
    }

    public void UpdateBestScore(long score)
    {
        UpdateBestScore((int)System.Math.Min(score, int.MaxValue));
    }

    public void UpdateBestTime(double time)
    {
        UpdateBestTime((float)time);
    }

    public void SetPlayerLevel(long level)
    {
        SetPlayerLevel((int)System.Math.Min(level, int.MaxValue));
    }

    public void AddXP(long xpAmount)
    {
        AddXP((int)System.Math.Min(xpAmount, int.MaxValue));
    }

    public bool SpendCurrency(CurrencyType type, long amount)
    {
        return SpendCurrency(type, (int)System.Math.Min(amount, int.MaxValue));
    }

    public void UpdateCurrency(CurrencyType type, long amount)
    {
        UpdateCurrency(type, (int)System.Math.Min(amount, int.MaxValue));
    }

    public void SetCoins(int amount)
    {
        if (saveManager == null) return;
        saveManager.Data.totalCoins = Mathf.Max(0, amount);
        saveManager.SaveGame();
        OnCoinsChanged?.Invoke(saveManager.Data.totalCoins);
    }
}
