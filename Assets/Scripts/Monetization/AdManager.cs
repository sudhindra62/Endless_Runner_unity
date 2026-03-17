
using System;
using EndlessRunner.Managers;
using UnityEngine;

namespace EndlessRunner.Monetization
{
    public enum RewardType
    {
        Revive,
        ExtraCoins,
        ThemeUnlockDiscount
    }

    public class AdManager : MonoBehaviour
    {
        public static AdManager Instance;

        private Action<RewardType> onRewardUser;
        private RewardType currentRewardType;

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

        public void ShowRewardedAd(RewardType rewardType, Action<RewardType> onRewardUser)
        {
            this.currentRewardType = rewardType;
            this.onRewardUser = onRewardUser;

            // --- Placeholder for showing a rewarded ad ---
            Debug.Log($"Showing Rewarded Ad for {rewardType}...");
            OnAdCompleted();
            // ---------------------------------------------
        }

        private void OnAdCompleted()
        {
            Debug.Log("Rewarded Ad Completed!");
            onRewardUser?.Invoke(currentRewardType);
            GiveReward(currentRewardType);
        }

        private void GiveReward(RewardType rewardType)
        {
            switch (rewardType)
            {
                case RewardType.ExtraCoins:
                    CurrencyManager.Instance.AddGems(50); // Or some other amount
                    Debug.Log("Awarded 50 extra gems!");
                    break;
                case RewardType.ThemeUnlockDiscount:
                    // This would likely be handled in the ThemeShopItemUI
                    Debug.Log("Theme unlock discount applied!");
                    break;
                case RewardType.Revive:
                    // This is handled in the GameOverUI
                    break;
            }
        }
    }
}
