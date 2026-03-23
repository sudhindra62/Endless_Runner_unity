using UnityEngine;


/// <summary>
/// ScriptableObject definition for a rare drop's properties and odds.
/// Global scope.
/// </summary>
[CreateAssetMenu(fileName = "New Rare Drop", menuName = "Endless Runner/Data/Rare Drop")]
public class RareDropData : ScriptableObject
{
    public string itemID;
    public RareDropType dropType;
    public string rarity;
    public float baseChance = 0.01f;
    public int pityThreshold = 10;
    public float pityIncrease = 0.01f;
    public int maxPerRun = 1;
    public float weight = 1.0f;
    public int riskTier = 1;

    // --- Property Aliases for Architectural Sync (Folder 2) ---
    public string dropName { get => itemID; set => itemID = value; }
    public bool isEnabled = true;
}

/// <summary>
/// Serializable data for tracking rare drop state.
/// </summary>
[System.Serializable]
public class RareDropStatus
{
    public string itemID;
    public int currentPity;
}
