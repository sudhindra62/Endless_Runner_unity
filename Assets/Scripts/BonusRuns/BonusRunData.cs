
using UnityEngine;

[CreateAssetMenu(fileName = "BonusRunData", menuName = "Gameplay/Bonus Run Data")]
public class BonusRunData : ScriptableObject
{
    public int bonusRunsPerDay = 5;
    public float coinMultiplier = 2f;
    public float xpMultiplier = 2f;
    public float rareDropChanceMultiplier = 1.5f;
}
