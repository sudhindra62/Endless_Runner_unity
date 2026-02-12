using UnityEngine;

/// <summary>
/// Runtime run-session tracker (MonoBehaviour version)
/// </summary>
public class RunSessionData : MonoBehaviour
{
    public int CoinsCollectedThisRun { get; private set; }
    public float DistanceThisRun { get; private set; }
    public float TimeThisRun { get; private set; }

    private float runStartTime;

    public void StartNewRun()
    {
        CoinsCollectedThisRun = 0;
        DistanceThisRun = 0f;
        runStartTime = Time.time;
    }
    public void Reset()
{
    // Compatibility ONLY
}


    public void EndRun()
    {
        TimeThisRun = Time.time - runStartTime;
    }

    public void AddCoins(int amount)
    {
        if (amount > 0)
            CoinsCollectedThisRun += amount;
    }

    public void UpdateDistance(float totalDistance)
    {
        DistanceThisRun = totalDistance;
    }
}
