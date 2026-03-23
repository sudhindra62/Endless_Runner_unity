using UnityEngine;

/// <summary>
/// A global proxy for Analytics to resolve CS0246 (Type or namespace not found).
/// Adheres to AEIS System Preservation.
/// </summary>
public class PlayerAnalyticsManager : Singleton<PlayerAnalyticsManager>
{
    public void TrackEvent(string eventName) 
    {
        Debug.Log($"[Analytics] Tracked: {eventName}");
    }

    public void TrackPurchase(string itemId, int cost, string currencyType)
    {
        Debug.Log($"[Analytics] Purchase: {itemId} for {cost} {currencyType}");
    }

    public void TrackRunEnd(int score, float distance)
    {
        Debug.Log($"[Analytics] Run Ended: Score {score}, Distance {distance}");
    }

    public void TrackBossEncounter(string bossId)
    {
        Debug.Log($"[Analytics] Boss Encountered: {bossId}");
    }

    public void TrackBossEncounter(string bossId, bool success)
    {
        Debug.Log($"[Analytics] Boss Encounter: {bossId} | Success: {success}");
    }
}
