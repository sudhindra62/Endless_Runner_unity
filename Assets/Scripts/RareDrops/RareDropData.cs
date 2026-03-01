
using UnityEngine;

[CreateAssetMenu(fileName = "RareDropData", menuName = "Rare Drops/Rare Drop Data")]
public class RareDropData : ScriptableObject
{
    public RareDropType dropType;
    public float baseChance = 0.01f; // 1%
    public int pityThreshold = 10;
    public int maxPerRun = 1;
    public float weight = 1.0f;
    public int riskTier = 1;
}
