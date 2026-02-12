using UnityEngine;
using EndlessRunner.OwnerControl;

namespace EndlessRunner.Ads.Coordination
{
    /// <summary>
    /// only yes or no to show ads
    /// Controls when interstitial ads are allowed to show.
    /// Uses run count + owner-configured frequency.
    /// </summary>
    public static class InterstitialPolicy
    {
        private const string RunCountKey = "INTERSTITIAL_RUN_COUNT";

        /// <summary>
        /// Call this at the end of a run.
        /// Returns true if an interstitial ad is allowed now.
        /// </summary>
        public static bool ShouldShowInterstitial()
        {
            if (!AdAvailabilityService.AreInterstitialAdsEnabled())
                return false;

            int frequency = Mathf.Max(1, OwnerConfigProvider.InterstitialFrequency);

            int runCount = PlayerPrefs.GetInt(RunCountKey, 0);
            runCount++;
            PlayerPrefs.SetInt(RunCountKey, runCount);

            return (runCount % frequency) == 0;
        }

        /// <summary>
        /// Resets interstitial counters (optional).
        /// Useful for testing or special game modes.
        /// </summary>
        public static void Reset()
        {
            PlayerPrefs.DeleteKey(RunCountKey);
        }
    }
}
