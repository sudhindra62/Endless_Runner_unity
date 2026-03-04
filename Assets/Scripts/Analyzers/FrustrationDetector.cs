
using UnityEngine;

public class FrustrationDetector
{
    private float lastDeathTime = -1;
    private int quickSuccessiveDeaths = 0;

    public void TrackDeath()
    {
        if (Time.time - lastDeathTime < 5.0f) // 5 seconds threshold for quick successive deaths
        {
            quickSuccessiveDeaths++;
        }
        else
        {
            quickSuccessiveDeaths = 1;
        }

        lastDeathTime = Time.time;

        if (quickSuccessiveDeaths >= 3) // 3 quick deaths in a row indicates frustration
        {
            Debug.Log("Frustration detected!");
            // In a real implementation, you might trigger an event here
            // to offer the player some help or adjust the difficulty.
        }
    }

    public void ProcessSession(SessionAnalyticsData currentSession)
    {
        // This is a placeholder for any session-based frustration analysis.
    }
}
