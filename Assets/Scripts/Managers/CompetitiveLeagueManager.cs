using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Authoritative manager for the competitive league system.
/// Replaces legacy LeagueManager and RankManager to prevent authority conflicts.
/// Handles player ranking, promotion/demotion, rewards, and anti-cheat.
/// </summary>
public class CompetitiveLeagueManager : Singleton<CompetitiveLeagueManager>
{
    private List<LeagueTierDefinition> _leagueTiers; // Loaded from config
    private PlayerLeagueState _playerState;

    private void OnEnable()
    {
        // Subscribe to events from other authoritative systems
        SeasonManager.OnWeeklyReset += ProcessWeeklyReset;
        SeasonManager.OnSeasonEnd += ProcessSeasonEnd;
        // ScoreManager.OnFinalScoreSubmitted += UpdatePlayerScore; (EXAMPLE)
    }

    private void OnDisable()
    {
        SeasonManager.OnWeeklyReset -= ProcessWeeklyReset;
        SeasonManager.OnSeasonEnd -= ProcessSeasonEnd;
    }

    public void Initialize(List<LeagueTierDefinition> tiers)
    {
        _leagueTiers = tiers;
        LoadPlayerState();
    }

    private void LoadPlayerState()
    {
        // string savedData = SaveSystem.Instance.LoadLeagueData();
        // if (!string.IsNullOrEmpty(savedData))
        // {
        //     _playerState = JsonUtility.FromJson<PlayerLeagueState>(savedData);
        // }
        // else
        // {
        //     _playerState = new PlayerLeagueState(); // New player
        // }
    }

    private void SavePlayerState()
    {
        // string dataToSave = JsonUtility.ToJson(_playerState);
        // SaveSystem.Instance.SaveLeagueData(dataToSave);
    }

    public void UpdatePlayerScore(long score, bool isIntegrityPassed)
    {
        // ANTI-CHEAT: Score is ignored if the session was flagged.
        if (!isIntegrityPassed) return;
        if (score > _playerState.BestScoreThisWeek)
        {
            _playerState.BestScoreThisWeek = score;
            // SavePlayerState(); (Can be optimized to save less frequently)
        }
    }

    private void ProcessWeeklyReset()
    {
        // In a real system, you'd fetch the player's group ranking from a server.
        // Here, we simulate it based on their score relative to division thresholds.
        
        var currentDivision = GetDivisionForScore(_playerState.BestScoreThisWeek);
        if (currentDivision == null) return;
        
        // PROMOTION / DEMOTION LOGIC
        float scorePercentage = GetScorePercentageInDivision(currentDivision, _playerState.BestScoreThisWeek);

        if (scorePercentage >= 0.8f) // Top 20%
        {
            PromotePlayer();
        }
        else if (scorePercentage < 0.2f) // Bottom 20%
        {
            DemotePlayer();
        }
        
        // Grant weekly rewards through the authoritative RewardManager
        // RewardManager.Instance.GrantRewards(_playerState.CurrentTier, isWeekly: true);

        _playerState.BestScoreThisWeek = 0;
        SavePlayerState();
    }

    private void ProcessSeasonEnd()
    { 
        // Grant seasonal rewards (exclusive skins, frames)
        // RewardManager.Instance.GrantRewards(_playerState.HighestTierThisSeason, isSeason: true);

        // PARTIAL RANK DECAY: Drop player by a few tiers (e.g., 2-3 divisions)
        DecayPlayerRank();

        _playerState.HighestTierThisSeason = _playerState.CurrentTier;
        SavePlayerState();
    }

    private void PromotePlayer()
    {
       // Logic to move player to the next division, ensuring no double-promotions
    }

    private void DemotePlayer()
    {
        // Logic to move player to the previous division
    }

    private void DecayPlayerRank()
    {
        // Logic for partial rank reset at season end
    }

    #region Helper Methods
    private LeagueDivisionData GetDivisionForScore(long score)
    {
        // Finds the correct division for a given score from the _leagueTiers config
        return null; 
    }
    
    private float GetScorePercentageInDivision(LeagueDivisionData division, long score)
    {
        long range = division.PromotionScore - division.DemotionScore;
        if (range <= 0) return 0.5f;
        return (float)(score - division.DemotionScore) / range;
    }
    #endregion

    // Internal state of the player in the league system
    private class PlayerLeagueState
    {
        public string CurrentTier;
        public int CurrentDivision;
        public string HighestTierThisSeason;
        public long BestScoreThisWeek;
    }
}
