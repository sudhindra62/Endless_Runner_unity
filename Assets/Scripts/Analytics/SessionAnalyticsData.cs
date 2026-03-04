
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data container for a single gameplay session. This object holds all the raw
/// analytics data collected between the start and end of a run. It's designed to be
/// lightweight and easily serializable.
/// </summary>
[System.Serializable]
public class SessionAnalyticsData
{
    // Session Timing
    public float SessionStartTime { get; private set; }
    public float SessionEndTime { get; private set; }
    public float SessionDuration => SessionEndTime - SessionStartTime;
    public bool WasAbruptlyEnded { get; private set; }

    // Core Gameplay Metrics
    public int TotalDodges { get; private set; }
    public int SuccessfulDodges { get; private set; }
    public float DodgeSuccessRate => TotalDodges > 0 ? (float)SuccessfulDodges / TotalDodges : 0;

    public List<float> ReactionTimes { get; private set; } = new List<float>();
    public float AverageReactionTime
    {
        get
        {
            if (ReactionTimes.Count == 0) return 0;
            float total = 0;
            foreach (var time in ReactionTimes) total += time;
            return total / ReactionTimes.Count;
        }
    }

    // Death Tracking with Timestamps
    public struct DeathEvent
    {
        public float Timestamp;
        public string Cause;
    }
    public List<DeathEvent> DeathHistory { get; private set; } = new List<DeathEvent>();
    public int DeathCount => DeathHistory.Count;

    public int ComboPeak { get; private set; }
    public int ReviveCount { get; private set; }

    // Boss Encounters
    public Dictionary<string, int> BossEncounters { get; private set; } = new Dictionary<string, int>();
    public Dictionary<string, int> BossSurvivability { get; private set; } = new Dictionary<string, int>();
    public float GetBossSurvivalRate(string bossName)
    {
        if (!BossEncounters.ContainsKey(bossName) || BossEncounters[bossName] == 0) return 0;
        int survivals = BossSurvivability.ContainsKey(bossName) ? BossSurvivability[bossName] : 0;
        return (float)survivals / BossEncounters[bossName];
    }

    // --- Session Lifecycle ---

    public void BeginSession()
    {
        SessionStartTime = Time.time;
    }

    public void EndSession(bool wasAbrupt)
    {
        SessionEndTime = Time.time;
        WasAbruptlyEnded = wasAbrupt;
    }

    // --- Data Recording ---

    public void RecordDodge(bool success)
    {
        TotalDodges++;
        if (success) SuccessfulDodges++;
    }

    public void RecordReactionTime(float time)
    {
        ReactionTimes.Add(time);
    }

    public void RecordDeath(string cause, float timestamp)
    {
        DeathHistory.Add(new DeathEvent { Cause = cause, Timestamp = timestamp });
    }

    public void UpdateComboPeak(int peak)
    {
        if (peak > ComboPeak) ComboPeak = peak;
    }

    public void RecordRevive()
    {
        ReviveCount++;
    }

    public void RecordBossEncounter(string bossName, bool survived)
    {
        if (!BossEncounters.ContainsKey(bossName))
        {
            BossEncounters[bossName] = 0;
            BossSurvivability[bossName] = 0;
        }
        BossEncounters[bossName]++;
        if (survived)
        {
            BossSurvivability[bossName]++;
        }
    }
}
