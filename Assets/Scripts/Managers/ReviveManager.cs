
using UnityEngine;
using System;

public class ReviveManager : Singleton<ReviveManager>
{
    public static event Action OnPlayerRevived;

    private PlayerAnalyticsManager analyticsManager;

    private void Start()
    {
        analyticsManager = PlayerAnalyticsManager.Instance;
    }

    public void PlayerRevived()
    {
        if (analyticsManager != null && IntegrityManager.Instance.IsAnalyticsEnabled())
        {
            analyticsManager.TrackRevive();
        }

        OnPlayerRevived?.Invoke();
    }
}
