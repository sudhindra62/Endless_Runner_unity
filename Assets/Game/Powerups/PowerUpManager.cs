using UnityEngine;
using System.Collections.Generic;

public partial class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    private Dictionary<PowerUpType, int> powerUpInventory = new Dictionary<PowerUpType, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ConsumePowerUp(PowerUpType type)
    {
        if (GetInventoryCount(type) > 0)
        {
            powerUpInventory[type]--;
            SaveInventory();
            return true;
        }
        return false;
    }

    public void AddPowerUps(PowerUpType type, int amount)
    {
        powerUpInventory[type] = GetInventoryCount(type) + amount;
        SaveInventory();
    }

    public int GetInventoryCount(PowerUpType type)
    {
        return powerUpInventory.ContainsKey(type) ? powerUpInventory[type] : 0;
    }

    private void LoadInventory()
    {
        powerUpInventory[PowerUpType.CoinDoubler] =
            PlayerPrefs.GetInt("Inventory_" + PowerUpType.CoinDoubler, 0);

        powerUpInventory[PowerUpType.ScoreBooster] =
            PlayerPrefs.GetInt("Inventory_" + PowerUpType.ScoreBooster, 0);
    }

    private void SaveInventory()
    {
        PlayerPrefs.SetInt("Inventory_" + PowerUpType.CoinDoubler,
            powerUpInventory[PowerUpType.CoinDoubler]);

        PlayerPrefs.SetInt("Inventory_" + PowerUpType.ScoreBooster,
            powerUpInventory[PowerUpType.ScoreBooster]);

        PlayerPrefs.Save();
    }
}
