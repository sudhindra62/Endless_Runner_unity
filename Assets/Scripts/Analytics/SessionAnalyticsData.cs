using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SessionAnalyticsData
{
    public float sessionStartTime;
    public float sessionEndTime;
    public float sessionDuration;
    public List<PlayerDeath> deaths = new List<PlayerDeath>();
    public int totalDodges;
    public int successfulDodges;
    public int comboPeak;
    public int reviveCount;
    public List<BossEncounter> bossEncounters = new List<BossEncounter>();

    public void StartSession()
    {
        sessionStartTime = Time.time;
    }

    public void EndSession()
    {
        sessionEndTime = Time.time;
        sessionDuration = sessionEndTime - sessionStartTime;
    }

    public void RecordDeath(string cause, float distance)
    {
        deaths.Add(new PlayerDeath { cause = cause, distance = distance });
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
        bossEncounters.Add(new BossEncounter { bossName = bossName, survived = survived });
    }
}

[System.Serializable]
public struct PlayerDeath
{
    public string cause;
    public float distance;
}

[System.Serializable]
public struct BossEncounter
{
    public string bossName;
    public bool survived;
}
