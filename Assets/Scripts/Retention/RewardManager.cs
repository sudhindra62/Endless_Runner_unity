using UnityEngine;

/// <summary>
/// A central service for granting rewards to the player.
/// This singleton script communicates with other managers (CurrencyManager, SkinManager)
/// to distribute coins, gems, skins, or other items.
/// </summary>
public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Grants a package of rewards to the player.
    /// </summary>
    /// <param name="package">The RewardPackage containing the items to grant.</param>
    public void GrantRewards(RewardPackage package)
    {
        // Grant Coins
        if (package.Coins > 0)
        {
            CurrencyManager.Instance.AddCoins(package.Coins);
            Debug.Log($"Granted {package.Coins} coins.");
        }

        // Grant Gems
        if (package.Gems > 0)
        {
            CurrencyManager.Instance.AddGems(package.Gems);
            Debug.Log($"Granted {package.Gems} gems.");
        }

        // Grant a Skin
        if (package.ContainsSkin)
        {
            // TODO: Implement logic to unlock a random, currently locked skin.
            // This requires a new method in SkinManager, e.g., SkinManager.Instance.UnlockRandomSkin();
            Debug.Log("A skin was rewarded! (Pending implementation in SkinManager)");
        }

        // Future: Grant Boosts
        // if (package.BoostTokens > 0) { ... }
    }
}

/// <summary>
/// A simple struct to hold the contents of a reward distribution.
/// This is created by a reward source (like a Chest) and consumed by the RewardManager.
/// </summary>
public struct RewardPackage
{
    public int Coins;
    public int Gems;
    public bool ContainsSkin;
    // public int BoostTokens; // Future use
}
