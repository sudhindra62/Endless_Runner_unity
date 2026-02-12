using UnityEngine;

/// <summary>
/// Defines the player ranks in ascending order of prestige.
/// The order here matters for promotion logic.
/// </summary>
public enum PlayerRank
{
    Bronze,
    Silver,
    Gold,
    Platinum,
    Diamond
}

/// <summary>
/// A ScriptableObject that holds the configuration for a single player rank.
/// </summary>
[CreateAssetMenu(fileName = "New Rank", menuName = "Progression/Rank")]
public class RankData : ScriptableObject
{
    [Header("Rank Details")]
    public PlayerRank rank;
    public string rankName;
    [Tooltip("The minimum score required to achieve this rank.")]
    public int scoreThreshold;
    
    [Header("Visuals")]
    public Sprite rankBadgeSprite;
    public Color rankColor = Color.white;
}
