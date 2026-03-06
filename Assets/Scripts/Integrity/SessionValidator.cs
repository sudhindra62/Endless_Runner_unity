using UnityEngine;

public class SessionValidator
{
    private float lastTimeScale = 1.0f;
    private float timeScaleSpikeThreshold = 5.0f;

    public bool IsTimeScaleValid()
    {
        if (Time.timeScale > lastTimeScale + timeScaleSpikeThreshold)
        {
            Debug.LogWarning("Time scale has spiked unexpectedly!");
            return false;
        }
        lastTimeScale = Time.timeScale;
        return true;
    }

    public bool IsRunDataValid(float runDistance, float runTime)
    {
        // A very basic check. A more robust implementation would involve
        // a more sophisticated model of expected performance.
        float maxPossibleDistance = runTime * 30.0f; // Assuming a generous max speed of 30 units/sec
        if (runDistance > maxPossibleDistance)
        {
            Debug.LogWarning("Run distance exceeds theoretical maximum.");
            return false;
        }
        return true;
    }
}
