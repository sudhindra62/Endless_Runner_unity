
using System;
using UnityEngine;

public class AdManager : Singleton<AdManager>
{
    public event Action OnAdCompleted;

    public void RequestRewardedAd()
    {
        // Simulate ad playback
        Debug.Log("Showing a rewarded ad...");
        Invoke("SimulateAdCompletion", 2f); // Simulate a 2-second ad
    }

    private void SimulateAdCompletion()
    {
        Debug.Log("Rewarded ad completed.");
        OnAdCompleted?.Invoke();
    }
}
