
using UnityEngine;

/// <summary>
/// Tracks detailed analytics for the tiered revive system.
/// Logs which type of revive was used for balancing and monetization analysis.
/// </summary>
public class ReviveAnalyticsTracker : MonoBehaviour
{
    private PlayerAnalyticsManager analyticsManager;

    private void Start()
    {
        analyticsManager = PlayerAnalyticsManager.Instance;
        ReviveManager.OnPlayerRevived += TrackTieredRevive; // Subscribe to legacy event
    }

    private void OnDestroy()
    { 
        ReviveManager.OnPlayerRevived -= TrackTieredRevive;
    }

    private void TrackTieredRevive()
    {
        if (analyticsManager == null || !IntegrityManager.Instance.IsAnalyticsEnabled()) return;

        int revivesThisRun = ReviveEconomyManager.Instance.GetRevivesThisRun();

        // The legacy ReviveManager fires its event *before* the count is incremented in the new system.
        // So, the current number of revives *is* the tier that was just used.
        string reviveType = "";
        switch (revivesThisRun)
        {
            case 0: 
                reviveType = "AdRevive";
                break;
            case 1:
                reviveType = "GemRevive";
                break;
            case 2:
                reviveType = "TokenRevive";
                break;
        }

        if (!string.IsNullOrEmpty(reviveType))
        {
            analyticsManager.TrackEvent("PlayerRevived", new System.Collections.Generic.Dictionary<string, object>
            {
                { "revive_tier", revivesThisRun + 1 },
                { "revive_type", reviveType }
            });
        }
    }
}
