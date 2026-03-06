
using UnityEngine;
using System.Collections.Generic;

public enum PowerUpType
{
    Magnet,
    Shield,
    CoinDoubler,
    ScoreMultiplier,
    FeverMode
}

[System.Serializable]
public class PowerUpUpgradeTier
{
    public int level;
    public float value; // e.g., radius, duration
    public int cost;
    public string currencyType; // "coins" or "gems"
}

[CreateAssetMenu(fileName = "PowerUpUpgradeData", menuName = "Gameplay/Power-Up Upgrade Data")]
public class PowerUpUpgradeData : ScriptableObject
{
    public PowerUpType powerUpType;
    public string powerUpName;
    public string description;
    public Sprite icon;
    public List<PowerUpUpgradeTier> upgradeTiers;
}
