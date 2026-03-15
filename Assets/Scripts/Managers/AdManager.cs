
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class AdManager : MonoBehaviour
    {
        public void ShowRewardedVideo(System.Action<bool> onComplete)
        {
            // In a real implementation, this would show a rewarded video ad
            Debug.Log("AD_MANAGER: Showing rewarded video ad...");
            onComplete(true); // Simulate a successful ad view
        }

        public void ShowInterstitialAd()
        {
            // In a real implementation, this would show an interstitial ad
            Debug.Log("AD_MANAGER: Showing interstitial ad...");
        }
    }
}
