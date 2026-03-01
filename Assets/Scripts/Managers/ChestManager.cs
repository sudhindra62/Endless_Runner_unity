
using System;
using UnityEngine;

[Serializable]
public class PlayerChestState
{
    public string ChestId;
    public Chest.ChestState State = Chest.ChestState.Locked;
    public DateTime UnlockTime;

    public PlayerChestState(string id)
    {
        ChestId = id;
    }
}

[Serializable]
public class Chest
{
    public enum ChestState { Locked, Unlocking, ReadyToOpen, Opened }
    
    public string ChestId;
    public int UnlockDurationSeconds;
    public int CoinReward;
    public int GemReward;
    public int XpReward;
}

public class ChestManager : Singleton<ChestManager>
{
    [SerializeField] private Chest[] availableChests;
    private const string ChestStateKeyPrefix = "ChestState_";

    public void GrantChest(string chestId)
    {
        PlayerChestState chestState = GetChestState(chestId);
        if (chestState.State != Chest.ChestState.Locked)
        {
            Debug.LogWarning($"Attempted to grant chest {chestId}, but a chest with this ID already exists and is not in the default state.");
            return;
        }

        SaveChestState(chestState);
        Debug.Log($"Chest {chestId} has been granted to the player.");
    }

    public void GrantRandomChest()
    {
        if (availableChests.Length == 0) return;
        
        Chest randomChest = availableChests[UnityEngine.Random.Range(0, availableChests.Length)];
        GrantChest(randomChest.ChestId);
    }

    public void StartUnlockingChest(string chestId)
    {
        PlayerChestState chestState = GetChestState(chestId);
        if (chestState.State != Chest.ChestState.Locked)
        {
            Debug.LogWarning($"Chest {chestId} is not in a locked state.");
            return;
        }

        chestState.State = Chest.ChestState.Unlocking;
        chestState.UnlockTime = DateTime.UtcNow.AddSeconds(GetChestById(chestId).UnlockDurationSeconds);
        SaveChestState(chestState);
        Debug.Log($"Chest {chestId} is now unlocking and will be ready at {chestState.UnlockTime}.");
    }

    public void OpenChest(string chestId)
    {
        PlayerChestState chestState = GetChestState(chestId);
        Chest chest = GetChestById(chestId);

        if (chestState.State != Chest.ChestState.ReadyToOpen)
        {
            if(chestState.State == Chest.ChestState.Unlocking && DateTime.UtcNow >= chestState.UnlockTime)
            {
                 chestState.State = Chest.ChestState.ReadyToOpen;
            }
            else
            {
                Debug.LogWarning($"Chest {chestId} is not ready to be opened.");
                return;
            }
        }

        RewardManager.Instance.GrantReward(new Reward("Chest Reward", RewardType.Coins, chest.CoinReward));
        RewardManager.Instance.GrantReward(new Reward("Chest Reward", RewardType.Gems, chest.GemReward));
        RewardManager.Instance.GrantReward(new Reward("Chest Reward", RewardType.XP, chest.XpReward));
        chestState.State = Chest.ChestState.Opened;
        SaveChestState(chestState);
        Debug.Log($"Chest {chestId} has been opened.");
    }

    public PlayerChestState GetChestState(string chestId)
    {
        string json = PlayerPrefs.GetString(ChestStateKeyPrefix + chestId, null);
        if (string.IsNullOrEmpty(json))
        { 
            return new PlayerChestState(chestId);
        }
        return JsonUtility.FromJson<PlayerChestState>(json);
    }

    private void SaveChestState(PlayerChestState chestState)
    {
        string json = JsonUtility.ToJson(chestState);
        PlayerPrefs.SetString(ChestStateKeyPrefix + chestState.ChestId, json);
        PlayerPrefs.Save();
    }

    private Chest GetChestById(string chestId)
    {
        foreach (Chest chest in availableChests)
        {
            if (chest.ChestId == chestId)
            {
                return chest;
            }
        }
        return null;
    }
    
    [ContextMenu("Reset All Chests")]
    public void ResetAllChests()
    {
        foreach (var chest in availableChests)
        {
            PlayerPrefs.DeleteKey(ChestStateKeyPrefix + chest.ChestId);
        }
        Debug.Log("All chest data has been reset.");
    }
}
