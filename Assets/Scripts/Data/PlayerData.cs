
using System;

/// <summary>
/// Represents the data structure for all persistent player information.
/// </summary>
[Serializable]
public class PlayerData
{
    public int highScore;
    public int totalCoins;
    public int totalGems;
    public int playerLevel;
    public float currentXP;
    public bool tutorialCompleted;

    public PlayerData()
    {
        highScore = 0;
        totalCoins = 0;
        totalGems = 0;
        playerLevel = 1;
        currentXP = 0f;
        tutorialCompleted = false;
    }
}
