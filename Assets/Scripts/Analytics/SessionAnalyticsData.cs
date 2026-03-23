using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct PlayerDeathEvent
{
    public string cause;
    public float distance;
}

[System.Serializable]
public struct BossEncounterEvent
{
    public string bossName;
    public bool survived;
}

[System.Serializable]
public class SessionAnalyticsData
{
    // --- RAW DATA ---
    public float sessionStartTime;
    public float sessionEndTime;
    public bool sessionEndedAbruptly;

    public List<PlayerDeathEvent> deaths = new List<PlayerDeathEvent>();
    public int totalDodges;
    public int successfulDodges;
    public int comboPeak;
    public int reviveCount;
    public List<BossEncounterEvent> bossEncounters = new List<BossEncounterEvent>();
    
    // --- CALCULATED PROPERTIES ---
    public float SessionDuration => sessionEndTime > 0 ? (sessionEndTime - sessionStartTime) : (Time.time - sessionStartTime);
    public float DodgeSuccessRate => totalDodges > 0 ? (float)successfulDodges / totalDodges : 0;
    public int ComboPeak => comboPeak;
    public int ReviveFrequency => reviveCount;
    public float BossSurvivalRate => bossEncounters.Count > 0 ? (float)bossEncounters.Count(e => e.survived) / bossEncounters.Count : 0;
    public bool RageQuitIndicator => sessionEndedAbruptly;
    public Dictionary<string, int> DeathCauseFrequency 
    {
        get 
        {
            var frequency = new Dictionary<string, int>();
            if (deaths == null) return frequency;
            foreach (var death in deaths)
            {
                if (death.cause != null && !frequency.ContainsKey(death.cause))
                {
                    frequency[death.cause] = 0;
                }
                if (death.cause != null) frequency[death.cause]++;
            }
            return frequency;
        }
    }

    public SessionAnalyticsData()
    {
        deaths = new List<PlayerDeathEvent>();
        bossEncounters = new List<BossEncounterEvent>();
    }

    public void StartSession()
    {
        sessionStartTime = Time.time;
        sessionEndTime = 0;
        sessionEndedAbruptly = false;
        
        deaths.Clear();
        totalDodges = 0;
        successfulDodges = 0;
        comboPeak = 0;
        reviveCount = 0;
        bossEncounters.Clear();
    }

    public void EndSession(bool abrupt = false)
    {
        sessionEndTime = Time.time;
        sessionEndedAbruptly = abrupt;
    }

    public void RecordDeath(string cause, float distance)
    {
        if (deaths == null) deaths = new List<PlayerDeathEvent>();
        deaths.Add(new PlayerDeathEvent { cause = cause, distance = distance });
    }

    public void RecordDodge(bool success)
    {
        totalDodges++;
        if (success)
        {
            successfulDodges++;
        }
    }

    public void RecordCombo(int peak)
    {
        if (peak > comboPeak)
        {
            comboPeak = peak;
        }
    }

    public void RecordRevive()
    {
        reviveCount++;
    }

    public void RecordBossEncounter(string bossName, bool survived)
    {
        if (bossEncounters == null) bossEncounters = new List<BossEncounterEvent>();
        bossEncounters.Add(new BossEncounterEvent { bossName = bossName, survived = survived });
    }
}
