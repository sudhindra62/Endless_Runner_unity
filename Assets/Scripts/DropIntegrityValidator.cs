
using UnityEngine;

public class DropIntegrityValidator : MonoBehaviour
{
    // Theoretical maximums - to be fine-tuned with designers
    private const float MAX_THEORETICAL_SCORE_PER_SECOND = 1000;
    private const float MAX_TIME_SCALE_ALLOWED = 1.2f; // Allow for minor, legitimate fluctuations

    public static DropIntegrityValidator Instance { get; private set; }

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

    public bool IsRunValid(RunSessionData runData, bool bossDefeated)
    {
        // 1. Validate Run Duration and Score
        if (runData.duration > 0 && (runData.score / runData.duration) > MAX_THEORETICAL_SCORE_PER_SECOND)
        {
            Debug.LogWarning("INTEGRITY FAIL: Score per second exceeds theoretical maximum. Potential cheat.");
            return false;
        }

        // 2. Time Scale Verification (if TimeControlManager exists)
        // This requires a TimeControlManager that logs the max timeScale during the run.
        // For now, we'll use a placeholder check.
        if (Time.timeScale > MAX_TIME_SCALE_ALLOWED)
        {
            Debug.LogWarning("INTEGRITY FAIL: Time.timeScale is abnormally high. Potential speed hack.");
            // In a real implementation, we'd check a value logged by a dedicated manager.
            return false;
        }

        // 3. Revive Abuse Check
        // This rule depends on game design. Example: a run is invalid if revived more than 3 times.
        if (runData.reviveCount > 3) 
        {
             Debug.LogWarning("INTEGRITY FAIL: Excessive revives detected. Run invalidated for rare drop.");
            return false;
        }

        // All checks passed
        return true;
    }
}
