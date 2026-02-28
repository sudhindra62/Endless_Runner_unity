
using System;
using UnityEngine;

public class AdMobManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<AdMobManager>();
    }

    public void ShowRewardedAd(Action onSuccess, Action onFailure)
    {
        // In a real implementation, this would show a rewarded ad
        // and call onSuccess or onFailure based on the result
        // For now, we'll just call onSuccess to simulate a successful ad watch
        onSuccess?.Invoke();
    }
}
