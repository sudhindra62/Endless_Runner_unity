using UnityEngine;
using System.Collections.Generic;

public class LevelRewardManager : MonoBehaviour
{
    [SerializeField] private List<LevelRewardData> levelRewards;
    private PlayerDataManager _playerDataManager;
    private CurrencyManager _currencyManager;

    private void Start()
    {
        _playerDataManager = ServiceLocator.Get<PlayerDataManager>();
        _currencyManager = ServiceLocator.Get<CurrencyManager>();

        if (_playerDataManager == null) 
        {
            Debug.LogError("PlayerDataManager not found!");
            return;
        }

        _playerDataManager.OnLevelUp += GiveLevelUpReward;
    }

    private void GiveLevelUpReward(int newLevel)
    {
        LevelRewardData rewardData = levelRewards.Find(reward => reward.level == newLevel);
        if (rewardData != null)
        {
            if (_currencyManager != null)
            {
                _currencyManager.AddGems(rewardData.rewardAmount);
            }
        }
    }
}
