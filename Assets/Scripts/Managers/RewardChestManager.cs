using UnityEngine;
using System.Collections.Generic;
using System;

public class RewardChestManager : MonoBehaviour
{
    public List<ChestData> availableChests;
    private PlayerDataManager _playerDataManager;

    private void Start()
    {
        _playerDataManager = ServiceLocator.Get<PlayerDataManager>();
    }

    public bool IsChestReady(string chestId)
    {
        PlayerChestState chestState = _playerDataManager.GetChestState(chestId);
        if (chestState == null)
        {
            return true; // First time opening
        }

        ChestData chestData = availableChests.Find(c => c.chestId == chestId);
        if (chestData == null)
        {   
            return false;
        }

        TimeSpan timeSinceLastOpened = DateTime.UtcNow - chestState.lastOpenedTime;
        return timeSinceLastOpened.TotalHours >= chestData.cooldownHours;
    }

    public void OpenChest(string chestId)
    {
        if (!IsChestReady(chestId)) return;

        ChestData chestData = availableChests.Find(c => c.chestId == chestId);
        if (chestData == null) return;

        // Use the CurrencyManager to add the rewards
        CurrencyManager currencyManager = ServiceLocator.Get<CurrencyManager>();
        if (currencyManager != null)
        {
            currencyManager.AddCoins(chestData.coinReward);
        }

        _playerDataManager.UpdateChestState(chestId);
        Debug.Log($"Chest {chestData.chestName} opened! You received {chestData.coinReward} coins.");
    }
}
