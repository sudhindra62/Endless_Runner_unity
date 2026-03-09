
using System;

/// <summary>
/// Represents the data structure for all persistent player information.
/// </summary>
[Serializable]
public class PlayerData
{
    public int highScore;
    public int totalCoins;
    public bool tutorialCompleted;

    public PlayerData()
    {
        highScore = 0;
        totalCoins = 0;
        tutorialCompleted = false;
    }
}
