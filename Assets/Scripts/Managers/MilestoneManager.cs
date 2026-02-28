using UnityEngine;
using System;
using System.Collections.Generic;

public class MilestoneManager : MonoBehaviour
{
    public static event Action<MilestoneData> OnMilestoneCompleted;

    [SerializeField] private List<MilestoneData> allMilestones;
    private PlayerDataManager _playerDataManager;

    private void Start()
    {
        _playerDataManager = ServiceLocator.Get<PlayerDataManager>();
        if (_playerDataManager == null) 
        {
            Debug.LogError("PlayerDataManager not found!");
            return;
        }

        _playerDataManager.OnLevelUp += CheckForLevelMilestones;
        _playerDataManager.OnNewHighScore += CheckForScoreMilestones;
    }

    private void CheckForLevelMilestones(int newLevel)
    {
        foreach (var milestone in allMilestones)
        {
            if (milestone.milestoneType == MilestoneType.Level && newLevel >= milestone.targetValue)
            {
                CompleteMilestone(milestone);
            }
        }
    }

    private void CheckForScoreMilestones(int newScore)
    {
        foreach (var milestone in allMilestones)
        {
            if (milestone.milestoneType == MilestoneType.Score && newScore >= milestone.targetValue)
            {
                CompleteMilestone(milestone);
            }
        }
    }

    private void CompleteMilestone(MilestoneData milestone)
    {
        if (!_playerDataManager.IsMilestoneCompleted(milestone.milestoneId))
        {
            _playerDataManager.CompleteMilestone(milestone.milestoneId);
            OnMilestoneCompleted?.Invoke(milestone);
        }
    }
}
