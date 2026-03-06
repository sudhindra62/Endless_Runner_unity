
using UnityEngine;
using System.Collections.Generic;

public class PowerUpUpgradeManager : Singleton<PowerUpUpgradeManager>
{
    public Dictionary<PowerUpType, PowerUpUpgradeData> upgradeData = new Dictionary<PowerUpType, PowerUpUpgradeData>();

    private void Start()
    {
        // In a real game, you would load these from an AssetBundle or Resources folder
        // For now, let's assume they are assigned in the inspector.
    }

    public int GetPowerUpLevel(PowerUpType type)
    {
        // In a real game, you would get this from PlayerMetaData
        return PlayerPrefs.GetInt(type.ToString() + "Level", 1);
    }

    public void UpgradePowerUp(PowerUpType type)
    {
        int currentLevel = GetPowerUpLevel(type);
        if (!upgradeData.ContainsKey(type) || currentLevel >= upgradeData[type].upgradeTiers.Count)
        {
            Debug.Log("Cannot upgrade further.");
            return;
        }

        PowerUpUpgradeTier nextTier = upgradeData[type].upgradeTiers[currentLevel];
        // In a real game, you would use your CurrencyManager
        // if (CurrencyManager.Instance.Spend(nextTier.cost, nextTier.currencyType))
        // {
        //     PlayerPrefs.SetInt(type.ToString() + "Level", currentLevel + 1);
        //     Debug.Log($"{type} upgraded to level {currentLevel + 1}");
        // }
        // else
        // {
        //     Debug.Log("Not enough currency.");
        // }
    }

    public float GetPowerUpValue(PowerUpType type)
    {
        int level = GetPowerUpLevel(type) - 1; // Tiers are 0-indexed
        if (upgradeData.ContainsKey(type) && level < upgradeData[type].upgradeTiers.Count)
        {
            return upgradeData[type].upgradeTiers[level].value;
        }
        return 0; // Default value if not found
    }
}
