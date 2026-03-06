
using UnityEngine;

public class BehaviorTrendAnalyzer : MonoBehaviour
{
    public static BehaviorTrendAnalyzer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AnalyzeSessionData(SessionAnalyticsData sessionData)
    {
        // In a real-world scenario, this would involve more complex analysis.
        Debug.Log("Analyzing session data...");
        Debug.Log("Dodge Success Rate: " + (float)sessionData.successfulDodges / sessionData.totalDodges);
        Debug.Log("Death Cause Frequency: " + GetDeathCauseFrequency(sessionData));

        // --- MERGED LOGIC ---
        // From Assets/Scripts/Analyzers/BehaviorTrendAnalyzer.cs
        if (sessionData.RageQuitIndicator)
        {
            UnityEngine.Debug.Log("Rage quit detected!");
        }
    }

    private string GetDeathCauseFrequency(SessionAnalyticsData sessionData)
    {
        var deathCounts = new System.Collections.Generic.Dictionary<string, int>();
        foreach (var death in sessionData.deaths)
        {
            if (deathCounts.ContainsKey(death.cause))
            {
                deathCounts[death.cause]++;
            }
            else
            {
                deathCounts[death.cause] = 1;
            }
        }

        string frequency = "";
        foreach (var entry in deathCounts)
        {
            frequency += entry.Key + ": " + entry.Value + ", ";
        }
        return frequency;
    }
}
