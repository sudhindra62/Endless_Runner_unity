
using System;
using UnityEngine;

[Serializable]
public struct Rank
{
    public string Name;
    public int MinLevel;
}

/// <summary>
/// Calculates player rank based on level. It does not store rank, as it is derived data.
/// </summary>
public class RankManager : Singleton<RankManager>
{
    public static event Action<Rank> OnRankChanged;

    [SerializeField] private Rank[] ranks;
    private Rank _currentRank;

    private void OnEnable()
    {
        XPManager.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        XPManager.OnLevelUp -= HandleLevelUp;
    }

    private void HandleLevelUp(int newLevel)
    {
        Rank newRank = GetRankForLevel(newLevel);
        if (newRank.Name != _currentRank.Name)
        {
            _currentRank = newRank;
            OnRankChanged?.Invoke(_currentRank);
            Debug.Log($"Player promoted to new rank: {_currentRank.Name}");
        }
    }

    public Rank GetRankForLevel(int level)
    {
        for (int i = ranks.Length - 1; i >= 0; i--)
        {
            if (level >= ranks[i].MinLevel)
            {
                return ranks[i];
            }
        }
        return ranks[0]; // Default to the first rank
    }
}
