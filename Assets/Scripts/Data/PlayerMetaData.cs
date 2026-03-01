
using UnityEngine;

/// <summary>
/// Holds persistent, non-currency data about the player.
/// e.g., Player name, creation date, total playtime, etc.
/// </summary>
public class PlayerMetaData : Singleton<PlayerMetaData>
{
    public string PlayerName { get; set; } = "Player1";
    public int PlayerLevel { get; private set; } = 1;

    public void IncreaseLevel()
    {
        PlayerLevel++;
        // In a real game, this might trigger events for other systems.
    }
}
