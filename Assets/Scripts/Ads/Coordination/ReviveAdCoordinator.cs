using UnityEngine;
using EndlessRunner.OwnerControl;

namespace EndlessRunner.Ads.Coordination
{
    /// <summary>
    /// This file answers one question only:“Is the player allowed to revive via a rewarded ad right now?”
    /// Coordinates revive-via-rewarded-ad logic.
    /// Ensures only one ad revive per run.
    /// Does NOT show ads directly.
    /// </summary>
    public static class ReviveAdCoordinator
    {
        private const string AdReviveUsedKey = "AD_REVIVE_USED";

        /// <summary>
        /// Call this at the start of each run.
        /// </summary>
        public static void ResetForNewRun()
        {
            PlayerPrefs.DeleteKey(AdReviveUsedKey);
        }

        /// <summary>
        /// Returns true if revive via rewarded ad is allowed.
        /// </summary>
        public static bool CanReviveWithAd()
        {
            if (!OwnerConfigProvider.ReviveAdEnabled)
                return false;

            if (!AdAvailabilityService.AreRewardedAdsEnabled())
                return false;

            if (IsAdReviveAlreadyUsed())
                return false;

            return true;
        }

        /// <summary>
        /// Call this AFTER a rewarded revive succeeds.
        /// </summary>
        public static void MarkAdReviveUsed()
        {
            PlayerPrefs.SetInt(AdReviveUsedKey, 1);
            PlayerPrefs.Save();
        }

        private static bool IsAdReviveAlreadyUsed()
        {
            return PlayerPrefs.GetInt(AdReviveUsedKey, 0) == 1;
        }
    }
}
