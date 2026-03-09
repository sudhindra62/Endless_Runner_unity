
using UnityEngine;

/// <summary>
/// A ScriptableObject that defines the behavior of the pity system for a specific drop category.
/// This allows designers to configure pity thresholds, boosts, and guarantees without code changes.
/// Authored by the Supreme Guardian Architect v12.
/// </summary>
[CreateAssetMenu(fileName = "PityProfile", menuName = "Rare Drops/Pity Profile")]
public class PityProfile : ScriptableObject
{
    [Header("Identification")]
    [Tooltip("Unique name for this pity category (e.g., 'Legendary', 'Mythic', 'SeasonalChest'). Must match the identifier used in the data.")]
    public string pityCategoryName;

    [Header("Pity Mechanics")]
    [Tooltip("The number of unsuccessful attempts before a guarantee is triggered. Set to 0 to disable the guarantee.")]
    public int pityGuaranteeThreshold = 100;

    [Tooltip("A curve that defines how the pity boost multiplier increases as the counter approaches the threshold. The X-axis is progress (0-1) and the Y-axis is the multiplier (e.g., 1.0 to 2.0).")]
    public AnimationCurve pityBoostCurve = AnimationCurve.Linear(0, 1, 1, 2); // Default: linear boost from 1x to 2x

    [Header("Reset Logic")]
    [Tooltip("A list of other pity categories to reset when this one is successfully triggered. For example, a 'Mythic' drop should reset the 'Epic' and 'Legendary' counters.")]
    public PityProfile[] resetsCategories;
}
