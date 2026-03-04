
using UnityEngine;
using System.Collections.Generic;

public class SessionAnalyticsData
{
    public float ReactionTimeAverage { get; private set; }
    public float DodgeSuccessRate { get; private set; }
    public Dictionary<string, int> DeathCauseFrequency { get; private set; }
    public int ComboPeak { get; private set; }
    public int ReviveFrequency { get; private set; }
    public float SessionDuration { get; private set; }
    public float BossSurvivalRate { get; private set; }
    public bool RageQuitIndicator { get; private set; }

    private float sessionStartTime;
    private int totalDodges;
    private int successfulDodges;
    private int totalBossEncounters;
    private int survivedBossEncounters;

    public SessionAnalyticsData()
    {
        DeathCauseFrequency = new Dictionary<string, int>();
    }

    public void StartSession()
    {
        sessionStartTime = Time.time;
    }

    public void EndSession()
    {
        SessionDuration = Time.time - sessionStartTime;
    }

    public void LogDeath(string cause, float distance)
    {
        if (!DeathCauseFrequency.ContainsKey(cause))
        {
            DeathCauseFrequency[cause] = 0;
        }
        DeathCauseFrequency[cause]++;
    }

    public void LogDodge(bool success)
    {
        totalDodges++;
        if (success)
        {
            successfulDodges++;
        }
        DodgeSuccessRate = (float)successfulDodges / totalDodges;
    }

    public void LogComboPeak(int peak)
    {
        if (peak > ComboPeak)
        {
            ComboPeak = peak;
        }
    }

    public void LogRevive()
    {
        ReviveFrequency++;
    }

    public void LogBossSurvival(bool survived)
    {
        totalBossEncounters++;
        if (survived)
        {
            survivedBossEncounters++;
        }
        BossSurvivalRate = (float)survivedBossEncounters / totalBossEncounters;
    }
}
