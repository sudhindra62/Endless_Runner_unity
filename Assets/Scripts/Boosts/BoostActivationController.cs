
using UnityEngine;
using System;

/// <summary>
/// Tracks the state and duration of a single activated boost.
/// This class handles the countdown logic for time-based and run-based boosts.
/// </summary>
[System.Serializable]
public class BoostActivationController
{
    public BoostRewardData BoostData { get; private set; }
    private float timeRemaining;
    private int runsRemaining;

    public BoostActivationController(BoostRewardData data)
    {
        this.BoostData = data;
        if (data.durationType == BoostDurationType.Minutes)
        {
            this.timeRemaining = data.durationValue * 60f;
        }
        else if (data.durationType == BoostDurationType.Runs)
        {
            this.runsRemaining = data.durationValue;
        }
    }

    // This method needs to be called by a global ticker (e.g., Update in RewardedBoostManager) for time-based boosts
    public void Update(float deltaTime)
    {
        if (BoostData.durationType == BoostDurationType.Minutes && timeRemaining > 0)
        {
            timeRemaining -= deltaTime;
        }
    }

    public void DecrementDuration()
    {
        if (BoostData.durationType == BoostDurationType.Runs && runsRemaining > 0)
        {
            runsRemaining--;
        }
    }

    public void ExtendDuration(int amount)
    {
        if (BoostData.durationType == BoostDurationType.Minutes)
        {
            timeRemaining += amount * 60f;
        }
        else if (BoostData.durationType == BoostDurationType.Runs)
        {
            runsRemaining += amount;
        }
    }

    public bool IsExpired()
    {
        if (BoostData.durationType == BoostDurationType.Minutes)
        {
            return timeRemaining <= 0;
        }
        if (BoostData.durationType == BoostDurationType.Runs)
        {
            return runsRemaining <= 0;
        }
        return false; // Immediate boosts don't expire in the traditional sense
    }
}
