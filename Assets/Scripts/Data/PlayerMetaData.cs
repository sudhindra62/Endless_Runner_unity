using System;

/// <summary>
/// Holds persistent, non-currency data about the player.
/// This is a serializable DTO used by GameData.
/// Created by Supreme Guardian Architect v12 for AEIS Phase 1 Stabilization.
/// </summary>
[Serializable]
public class PlayerMetaData
{
    public string playerName = "Player1";
    public int playerLevel = 1;
    public int coins = 0;
    public int gems = 0;
    public float xp = 0f;
    public string playerMetaData = "{}"; // Metadata field expected by some managers
    public long totalPlayTime = 0;
    public string creationDate = "";
}
