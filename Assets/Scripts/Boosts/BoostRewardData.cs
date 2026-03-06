
using UnityEngine;

public enum BoostType
{
    DoubleCoins,
    DoubleXP,
    MagnetDuration,
    BonusChest,
    ExtraBonusRun
}

public enum BoostDurationType
{
    Runs,
    Minutes,
    Immediate
}

[CreateAssetMenu(fileName = "BoostData", menuName = "Boosts/Boost Reward Data")]
public class BoostRewardData : ScriptableObject
{
    [Header("Core Boost Definition")]
    public BoostType boostType;
    public BoostDurationType durationType;

    [Tooltip("Duration in runs or minutes, depending on the duration type. Not used for Immediate boosts.")]
    public int durationValue;

    [Tooltip("The multiplier or effect value (e.g., 2 for double, 1.5 for 50% increase).")]
    public float effectValue;

    [Header("UI Display")]
    public string description;
    public Sprite icon;

    [Header("Reward Integration")]
    [Tooltip("A direct item reward, like a chest prefab, for Immediate boost types.")]
    public GameObject immediateRewardPrefab;
}
