using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// Defines the properties of a single competitive league tier.
[Serializable]
public struct LeagueTier
{
    public string LeagueName;
    [Tooltip("The base score required to enter this league. This will be modified by LiveOps.")]
    public int BaseScoreThreshold;
    [Tooltip("The drop rate or reward multiplier a player gets for being in this league.")]
    public float RewardBoostMultiplier;
}

// Manages the player's progression through the league system and applies LiveOps adjustments.
public class LeagueManager : Singleton<LeagueManager>
{
    // --- EVENTS ---
    public static event Action<float> OnLeagueBoostChanged; // Preserved from original script.
    public static event Action<LeagueTier> OnPlayerLeagueChanged;

    // --- CONFIGURATION ---
    [Header("League Structure")]
    [Tooltip("The list of all leagues, sorted from lowest to highest score threshold.")]
    [SerializeField] private List<LeagueTier> leagueTiers = new List<LeagueTier>();

    // --- STATE ---
    private LeagueTier currentPlayerLeague;
    private int playerLeagueScore = 0; // Simulated player score

    private void Start()
    {
        // Sort the tiers to guarantee they are in ascending order of threshold.
        leagueTiers = leagueTiers.OrderBy(tier => tier.BaseScoreThreshold).ToList();
        
        // Initialize player's league on start.
        UpdatePlayerLeagueStatus(playerLeagueScore);
    }

    #region Public API & LiveOps Integration

    /// <summary>
    /// *** LIVE OPS INTEGRATION POINT ***
    /// Gets the score threshold for a specific league, adjusted by the LiveOps multiplier.
    /// </summary>
    /// <param name="leagueName">The name of the league to check.</param>
    /// <returns>The final, adjusted score threshold. Returns -1 if league not found.</returns>
    public int GetAdjustedThreshold(string leagueName)
    {
        // Find the base threshold from this manager's authoritative data.
        LeagueTier? tier = GetTierByName(leagueName);
        if (tier == null) 
        {
            Debug.LogWarning($"League '{leagueName}' not found!");
            return -1;
        }

        int baseThreshold = tier.Value.BaseScoreThreshold;

        // PULL the modifier from the LiveOpsManager.
        float liveOpsMultiplier = 1.0f;
        if (LiveOpsManager.Instance != null)
        {
            liveOpsMultiplier = LiveOpsManager.Instance.LeagueThresholdMultiplier;
        }

        // Apply the modifier and return the final value.
        int adjustedThreshold = Mathf.RoundToInt(baseThreshold * liveOpsMultiplier);
        
        // Debug.Log($"Adjusted threshold for {leagueName}: {adjustedThreshold} (Base: {baseThreshold}, LiveOps Multi: {liveOpsMultiplier})");

        return adjustedThreshold;
    }

    /// <summary>
    /// Updates the player's league based on their score and the adjusted thresholds.
    /// </summary>
    public void UpdatePlayerLeagueStatus(int newPlayerScore)
    {
        playerLeagueScore = newPlayerScore;
        LeagueTier newLeague = DetermineLeagueForScore(newPlayerScore);

        // Check if the league has changed.
        if (newLeague.LeagueName != currentPlayerLeague.LeagueName)
        {
            Debug.Log($"Player promoted to {newLeague.LeagueName} League!");
            currentPlayerLeague = newLeague;

            // Fire events to notify other systems.
            OnPlayerLeagueChanged?.Invoke(newLeague);
            
            // --- LOGIC PRESERVATION ---
            // The original event is fired with the new boost value.
            OnLeagueBoostChanged?.Invoke(newLeague.RewardBoostMultiplier);
        }
    }

    public LeagueTier GetCurrentPlayerLeague()
    {
        return currentPlayerLeague;
    }

    #endregion

    #region Private Helper Methods

    private LeagueTier DetermineLeagueForScore(int score)
    {
        LeagueTier determinedTier = leagueTiers.FirstOrDefault(); // Default to the lowest league.

        // Iterate backwards from the top league.
        for (int i = leagueTiers.Count - 1; i >= 0; i--)
        {
            int adjustedThreshold = GetAdjustedThreshold(leagueTiers[i].LeagueName);
            if (score >= adjustedThreshold)
            {
                determinedTier = leagueTiers[i];
                break; // Found the highest qualifying league.
            }
        }
        return determinedTier;
    }

    private LeagueTier? GetTierByName(string name)
    {
        foreach (var tier in leagueTiers)
        {
            if (tier.LeagueName == name) return tier;
        }
        return null;
    }

    #endregion
}
