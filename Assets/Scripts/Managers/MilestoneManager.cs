
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authoritative singleton for tracking and completing all long-term milestones.
/// It subscribes to game events and persists all milestone data to prevent exploits.
/// </summary>
public class MilestoneManager : Singleton<MilestoneManager>
{
    [SerializeField] private MilestoneDatabase milestoneDatabase;

    private const string CompletedMilestonesKey = "CompletedMilestones";
    private HashSet<string> _completedMilestones = new HashSet<string>();

    protected override void Awake()
    {
        base.Awake();
        LoadMilestoneData();
    }

    private void OnEnable()
    {
        // Subscribe to relevant game events
        XPManager.OnLevelUp += HandleLevelUp;
        // Example: ScoreManager.OnScoreIncreased += HandleScoreIncreased;
    }

    private void OnDisable()
    {
        XPManager.OnLevelUp -= HandleLevelUp;
        // Example: ScoreManager.OnScoreIncreased -= HandleScoreIncreased;
    }

    private void LoadMilestoneData()
    {
        string completedMilestonesString = PlayerPrefs.GetString(CompletedMilestonesKey, string.Empty);
        if (!string.IsNullOrEmpty(completedMilestonesString))
        {
            _completedMilestones = new HashSet<string>(completedMilestonesString.Split(','));
        }
    }

    private void SaveMilestoneData()
    {
        PlayerPrefs.SetString(CompletedMilestonesKey, string.Join(",", _completedMilestones));
        PlayerPrefs.Save();
    }

    private void HandleLevelUp(int newLevel)
    {
        List<Milestone> levelMilestones = milestoneDatabase.GetMilestonesByType("Level");
        foreach (Milestone milestone in levelMilestones)
        {
            if (newLevel >= milestone.RequiredAmount)
            {
                CompleteMilestone(milestone);
            }
        }
    }

    // Example handler for a score-based milestone
    // private void HandleScoreIncreased(int newScore)
    // {
    //     List<Milestone> scoreMilestones = milestoneDatabase.GetMilestonesByType("Score");
    //     foreach (Milestone milestone in scoreMilestones)
    //     {
    //         if (newScore >= milestone.RequiredAmount)
    //         {
    //             CompleteMilestone(milestone);
    //         }
    //     }
    // }

    private void CompleteMilestone(Milestone milestone)
    {
        if (IsMilestoneCompleted(milestone.MilestoneId)) return;

        _completedMilestones.Add(milestone.MilestoneId);
        RewardManager.Instance.GrantMissionReward(milestone.MilestoneId, milestone.CoinReward, milestone.GemReward, milestone.XpReward);
        SaveMilestoneData();
        Debug.Log($"Milestone {milestone.MilestoneId} completed!");
    }

    public bool IsMilestoneCompleted(string milestoneId)
    {
        return _completedMilestones.Contains(milestoneId);
    }
}
