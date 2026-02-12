
using System;

/// <summary>
/// A serializable data container that holds the state of the player's progression.
/// This includes their current level, XP, and the XP required for the next level.
/// This class contains no logic and is managed by the XPManager.
/// </summary>
[Serializable]
public class PlayerProgressionData
{
    public int CurrentLevel;
    public int CurrentXP;
    public int XPToNextLevel;
    public long TotalXP;
}
