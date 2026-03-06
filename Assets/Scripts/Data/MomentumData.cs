using UnityEngine;

/// <summary>
/// Data structure for passing momentum modifier information through events.
/// </summary>
public class MomentumData
{
    public int Tier { get; }
    public float SpeedModifier { get; }
    public float ScoreMultiplier { get; }

    public MomentumData(int tier, float speedModifier, float scoreMultiplier)
    {
        Tier = tier;
        SpeedModifier = speedModifier;
        ScoreMultiplier = scoreMultiplier;
    }
}
