using UnityEngine;

public class FrustrationDetector : MonoBehaviour
{
    public static FrustrationDetector Instance { get; private set; }

    private int quickDeathCounter;
    private float lastDeathTime;

    private const int QUICK_DEATH_THRESHOLD = 3;
    private const float QUICK_DEATH_WINDOW = 60f; // 1 minute

    public bool IsPlayerFrustrated { get; private set; }

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

    public void ReportPlayerDeath()
    {
        if (Time.time - lastDeathTime < QUICK_DEATH_WINDOW)
        {
            quickDeathCounter++;
            if (quickDeathCounter >= QUICK_DEATH_THRESHOLD)
            {
                IsPlayerFrustrated = true;
                FlagForSoftEasing();
            }
        }
        else
        {
            quickDeathCounter = 1;
        }
        lastDeathTime = Time.time;
    }

    public void AnalyzeSession(SessionAnalyticsData sessionData)
    {
        if (sessionData == null) return;

        IsPlayerFrustrated = false; // Reset at the start of analysis

        // 1. Quick Deaths
        if (quickDeathCounter >= QUICK_DEATH_THRESHOLD)
        {
            IsPlayerFrustrated = true;
        }

        // 2. High deaths, low distance
        if (sessionData.deaths.Count > 5)
        {
            float totalDistance = 0;
            foreach (var death in sessionData.deaths)
            {
                totalDistance += death.distance;
            }
            float avgDistance = totalDistance / sessionData.deaths.Count;
            if (avgDistance < 200f)
            {
                IsPlayerFrustrated = true;
                Debug.Log("Frustration Detected: High death count with low average distance.");
            }
        }

        // 3. Low dodge success rate
        if (sessionData.dodges.total > 10)
        {
            float successRate = (float)sessionData.dodges.successful / sessionData.dodges.total;
            if (successRate < 0.3f)
            {
                IsPlayerFrustrated = true;
                Debug.Log("Frustration Detected: Low dodge success rate.");
            }
        }

        // 4. Consistently failing boss encounters
        int failedBossEncounters = 0;
        foreach (var encounter in sessionData.bossEncounters)
        {
            if (!encounter.survived)
            {
                failedBossEncounters++;
            }
        }
        if (failedBossEncounters >= 2)
        {
            IsPlayerFrustrated = true;
            Debug.Log("Frustration Detected: Consistently failing boss encounters.");
        }


        if (IsPlayerFrustrated)
        {
            FlagForSoftEasing();
        }
    }

    public void OnSessionEnd(SessionAnalyticsData sessionData)
    {
        Debug.Log("Session End Analysis. Player frustrated state: " + IsPlayerFrustrated);
    }


    private void FlagForSoftEasing()
    {
        if (AdaptiveDifficultyManager.Instance != null)
        {
            AdaptiveDifficultyManager.Instance.ApplySoftEasing();
        }
    }
}
