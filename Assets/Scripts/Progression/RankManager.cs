using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the player's rank based on their best score.
/// It loads rank data, tracks the best score, and handles promotions.
/// </summary>
public class RankManager : MonoBehaviour
{
    public static RankManager Instance { get; private set; }

    private List<RankData> allRanks;
    private PlayerRank currentRank;
    private int bestScore;

    // Event signature: new rank, old rank
    public static event Action<RankData, RankData> OnRankPromoted;
    // Event signature: new best score
    public static event Action<int> OnBestScoreUpdated;

    private const string BEST_SCORE_KEY = "PlayerBestScore";
    private const string CURRENT_RANK_KEY = "PlayerCurrentRank";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadRanksFromResources();
            LoadPlayerData();
        }
    }

    private void LoadRanksFromResources()
    {
        // Load all RankData assets and sort them by the score required
        allRanks = Resources.LoadAll<RankData>("Ranks").OrderBy(r => r.scoreThreshold).ToList();
        Debug.Log($"Loaded {allRanks.Count} ranks from Resources.");
    }

    private void LoadPlayerData()
    {
        bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        currentRank = (PlayerRank)PlayerPrefs.GetInt(CURRENT_RANK_KEY, (int)PlayerRank.Bronze);
    }

    /// <summary>
    /// Called at the end of a run to submit the final score.
    /// </summary>
    public void SubmitScore(int finalScore)
    {
        if (finalScore <= bestScore) return; // Not a new best score

        bestScore = finalScore;
        PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
        OnBestScoreUpdated?.Invoke(bestScore);

        CheckForPromotion();

        PlayerPrefs.Save();
        Debug.Log($"New best score submitted: {bestScore}. Saved to PlayerPrefs.");
    }

    private void CheckForPromotion()
    {
        RankData newRankData = GetRankForScore(bestScore);
        if (newRankData == null) return;

        if (newRankData.rank > currentRank)
        {
            RankData oldRankData = GetRankData(currentRank);
            currentRank = newRankData.rank;
            PlayerPrefs.SetInt(CURRENT_RANK_KEY, (int)currentRank);
            
            OnRankPromoted?.Invoke(newRankData, oldRankData);
            Debug.Log($"Player promoted to {newRankData.rankName}!");
        }
    }

    #region Public Getters

    public RankData GetCurrentRankData() => GetRankData(currentRank);

    public RankData GetRankForScore(int score)
    {
        // Iterate backwards from the best rank to find the highest one achieved
        for (int i = allRanks.Count - 1; i >= 0; i--)
        {
            if (score >= allRanks[i].scoreThreshold)
            {
                return allRanks[i];
            }
        }
        return null; // Should not happen if ranks are configured from 0 score
    }

    public RankData GetNextRankData()
    {
        RankData currentRankData = GetCurrentRankData();
        if (currentRankData == null || currentRankData.rank == allRanks.Last().rank) return null; // Already at max rank
        
        int nextRankEnumIndex = (int)currentRankData.rank + 1;
        return GetRankData((PlayerRank)nextRankEnumIndex);
    }
    
    public RankData GetRankData(PlayerRank rank)
    {
        return allRanks.FirstOrDefault(r => r.rank == rank);
    }

    public int GetBestScore() => bestScore;

    #endregion
}
