
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance { get; private set; }

    private DataManager dataManager;
    private ChestManager chestManager;

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
        }
    }

    private void Start()
    {
        dataManager = DataManager.Instance;
        chestManager = ChestManager.Instance;
    }

    public void GrantReward(Reward reward)
    {
        if (dataManager == null) return;

        // reward.amount = RemoteConfig.GetInt("RewardAmount_" + reward.type, reward.amount);

        switch(reward.type)
        {
            case RewardType.Coins:
                dataManager.AddCoins(reward.amount);
                break;
            case RewardType.Gems:
                dataManager.AddGems(reward.amount);
                break;
            case RewardType.XP:
                dataManager.AddXP(reward.amount);
                break;
            case RewardType.Chest:
                if (chestManager != null) chestManager.GrantRandomChest();
                break;
            case RewardType.SuperPowerUp:
                // PowerUpManager.Instance.ActivateRandomSuperPowerUp();
                Debug.LogWarning($"Reward type 'SuperPowerUp' not fully implemented.");
                break;
            case RewardType.CosmeticFragment:
                // SkinsManager.Instance.AddSkinFragment(reward.id, reward.amount);
                Debug.LogWarning($"Reward type 'CosmeticFragment' not fully implemented.");
                break;
            case RewardType.LeaguePointBoost:
                if (LeagueManager.Instance != null)
                {
                    LeagueManager.Instance.AddLeaguePoints(reward.amount);
                }
                break;
            case RewardType.MysteryReward:
                // Grant a random reward from a predefined pool, which could call GrantReward recursively.
                Debug.LogWarning($"Reward type 'MysteryReward' not fully implemented.");
                break;
            default:
                Debug.LogWarning($"Unsupported reward type: {reward.type}");
                break;
        }

        Debug.Log($"Granted reward: {reward.description}");
    }
}
