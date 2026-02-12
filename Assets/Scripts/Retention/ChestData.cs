using UnityEngine;

/// <summary>
/// Defines the types of chests available in the game.
/// </summary>
public enum ChestType
{
    Free,
    Silver,
    Gold,
    Mega
}

/// <summary>
/// A ScriptableObject that defines the properties of a single type of chest,
/// including its rewards, cooldown, and UI appearance.
/// </summary>
[CreateAssetMenu(fileName = "NewChestData", menuName = "Endless Runner/Chest Data")]
public class ChestData : ScriptableObject
{
    [Header("Chest Info")]
    [Tooltip("The type of this chest.")]
    public ChestType type;

    [Tooltip("The icon displayed in the UI.")]
    public Sprite chestIcon;

    [Tooltip("Cooldown time in hours before the chest can be claimed again.")]
    public float cooldownHours = 3;

    [Header("Reward Contents")]
    [Tooltip("The minimum and maximum number of coins this chest can contain.")]
    public int minCoins = 10;
    public int maxCoins = 50;

    [Tooltip("The minimum and maximum number of gems this chest can contain.")]
    public int minGems = 0;
    public int maxGems = 5;

    [Tooltip("The probability (0.0 to 1.0) of receiving a random skin shard from this chest.")]
    [Range(0f, 1f)]
    public float skinShardChance = 0.05f;

    /// <summary>
    /// Generates a randomized reward package based on this chest's configuration.
    /// </summary>
    /// <returns>A RewardPackage struct containing the final reward amounts.</returns>
    public RewardPackage GenerateRewards()
    {
        return new RewardPackage
        {
            Coins = Random.Range(minCoins, maxCoins + 1),
            Gems = Random.Range(minGems, maxGems + 1),
            ContainsSkin = Random.value < skinShardChance
        };
    }
}
