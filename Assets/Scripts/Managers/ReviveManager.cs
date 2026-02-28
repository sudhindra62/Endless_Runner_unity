using UnityEngine;
using System;

public class ReviveManager : MonoBehaviour
{
    public event Action OnPlayerRevived;
    public event Action OnReviveDeclined;

    // Dependencies
    public ReviveTokenManager reviveTokenManager;
    public RunSessionData runSessionData;

    private CurrencyManager currencyManager;
    private BuildSettings buildSettings;
    private AdMobManager adMobManager;

    private void Awake()
    {
        ServiceLocator.Register<ReviveManager>(this);
    }

    private void Start()
    {
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        buildSettings = ServiceLocator.Get<BuildSettings>();
        adMobManager = ServiceLocator.Get<AdMobManager>();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<ReviveManager>();
    }

    public void ResetReviveState() { }

    public void InitiateReviveFlow()
    {
        // The UI will now determine which revive options are available.
        // This method can be used to trigger the revive UI.
    }

    public void ReviveWithGems(int gemCost)
    {
        if (currencyManager != null && currencyManager.SpendGems(gemCost))
        {
            GrantRevive();
        }
        else
        {
            DeclineRevive();
        }
    }

    public void ReviveWithToken()
    {
        if (reviveTokenManager != null && reviveTokenManager.UseToken())
        {
            GrantRevive();
        }
        else
        {
            DeclineRevive();
        }
    }

    public void ReviveWithAd()
    {
        if (buildSettings != null && buildSettings.AdsEnabled && adMobManager != null)
        { 
            adMobManager.ShowRewardedAd(GrantRevive);
        }
        else
        {
            DeclineRevive();
        }
    }

    public void DeclineRevive()
    {
        OnReviveDeclined?.Invoke();
    }

    private void GrantRevive()
    {
        if (runSessionData != null)
        {
            runSessionData.RevivesUsed++;
        }

        OnPlayerRevived?.Invoke();
    }
}
