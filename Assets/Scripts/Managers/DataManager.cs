using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // This class is intended to manage game data, including currency and streaks.
    // Currently, it only contains the coin streak logic.

    public int CoinStreak { get; private set; }

    public void AddToCoinStreak(int amount)
    {
        CoinStreak += amount;
    }

    public void ResetCoinStreak()
    {
        CoinStreak = 0;
    }
}
