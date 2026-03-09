
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int HighScore;
    public int TotalCurrency;
    public int PremiumCurrency; // Added by Supreme Guardian Architect v12 to fix CurrencyManager integration
    public bool TutorialCompleted;

    // Example of storing more complex data, like owned character skins
    public List<string> UnlockedCharacterIDs;

    public SaveData()
    {
        HighScore = 0;
        TotalCurrency = 0;
        PremiumCurrency = 0;
        TutorialCompleted = false;
        UnlockedCharacterIDs = new List<string>();
    }
}
