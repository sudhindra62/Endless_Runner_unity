using UnityEngine;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    public enum RewardType { Revive, ThemeDiscount, ExtraCoins }

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

    public void ShowRewardedAd(RewardType rewardType, Action onComplete, Action onFailed = null)
    {
        // This is a placeholder for showing a rewarded ad.
        // In a real project, you would integrate an ad SDK (e.g., Google AdMob, Unity Ads).
        Debug.Log("Showing Rewarded Ad for: " + rewardType.ToString());

        // Simulate successful ad completion.
        if (onComplete != null)
        {
            HandleReward(rewardType);
            onComplete.Invoke();
        }
    }

    private void HandleReward(RewardType rewardType)
    {
        switch (rewardType)
        {
            case RewardType.Revive:
                // Player revival logic would be handled in the calling script.
                Debug.Log("Player Revived!");
                break;
            case RewardType.ThemeDiscount:
                // The discount logic will be handled in the ThemeShopItem.
                Debug.Log("Theme Discount Applied!");
                break;
            case RewardType.ExtraCoins:
                CurrencyManager.Instance.AddCoins(100); // Grant 100 coins
                Debug.Log("100 Extra Coins Granted!");
                break;
        }
    }
}
