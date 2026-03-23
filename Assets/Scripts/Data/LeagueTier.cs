using System;

/// <summary>
/// Defines the league tiers available in the system.
/// FIXED: Added missing values referenced in errors.
/// </summary>
public enum LeagueTier
{
    Bronze,
    Silver,
    Gold,
    Platinum,
    Diamond
}

/// <summary>
/// Defines the various ranks within the league system.
/// Global scope.
/// </summary>
public enum LeagueRank
{
    Bronze,
    Silver,
    Gold,
    Platinum,
    Diamond
}

/// <summary>
/// A simple data class to represent a league tier instance.
/// Global scope.
/// </summary>
[Serializable]
public class LeagueTierInfo 
{
    public string LeagueName;
    public LeagueRank Rank;
    public int Level;
}