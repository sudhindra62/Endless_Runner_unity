using UnityEngine;

public class RankManager : MonoBehaviour
{
    public static RankManager Instance { get; private set; }
    public static event System.Action<LeagueTier> OnRankChanged;
    public static event System.Action<LeagueTier, LeagueTier> OnPromoted;
    public static event System.Action<LeagueTier> OnRankPromoted;
    public static event System.Action<int> OnBestScoreUpdated;
    
    // All logic has been removed to prevent authority conflicts with CompetitiveLeagueManager.
    private LeagueTier _currentTier = LeagueTier.Bronze;
    private int _bestScore;
    [SerializeField] private RankData[] rankData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddRankXP(int amount) {}
    
    public void UpdateRank(int rank)
    {
        PromoteTier(rank);
    }

    public void PromoteTier(LeagueTier tier)
    {
        var oldTier = _currentTier;
        _currentTier = tier;
        OnRankChanged?.Invoke(tier);
        OnPromoted?.Invoke(oldTier, tier);
        OnRankPromoted?.Invoke(tier);
    }

    public void DemoteTier(LeagueTier tier) => _currentTier = tier;

    public LeagueTier GetCurrentTier() => _currentTier;

    public int GetTierProgress() => 0; // Placeholder

    public bool IsAtMaxTier() => _currentTier == LeagueTier.Diamond;

    public void ResetTier() => _currentTier = LeagueTier.Bronze;
    public int GetBestScore() => _bestScore;
    public void SetBestScore(int score)
    {
        _bestScore = Mathf.Max(_bestScore, score);
        OnBestScoreUpdated?.Invoke(_bestScore);
    }

    public RankData GetCurrentRankData()
    {
        if (rankData == null || rankData.Length == 0) return null;

        RankData current = rankData[0];
        foreach (RankData data in rankData)
        {
            if (data != null && _bestScore >= data.scoreThreshold)
            {
                current = data;
            }
        }

        return current;
    }

    public RankData GetNextRankData()
    {
        if (rankData == null) return null;

        RankData current = GetCurrentRankData();
        bool returnNext = current == null;

        foreach (RankData data in rankData)
        {
            if (data == null) continue;
            if (returnNext) return data;
            if (data == current) returnNext = true;
        }

        return null;
    }

    // --- Type Conversion Overloads (Phase 2A: Type Consistency) ---

    public void PromoteTier(int rankValue)
    {
        var tier = TypeConverter.IntToLeagueTier(rankValue);
        PromoteTier(tier);
    }

    public void PromoteTier(string rankName)
    {
        var tier = TypeConverter.StringToLeagueTier(rankName);
        PromoteTier(tier);
    }

    public void DemoteTier(int rankValue)
    {
        var tier = TypeConverter.IntToLeagueTier(rankValue);
        DemoteTier(tier);
    }

    public int GetCurrentTierAsInt() => (int)_currentTier;

    public string GetCurrentTierAsString() => _currentTier.ToString();
}
