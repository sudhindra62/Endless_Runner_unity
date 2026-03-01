
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // Example: Player data
    public int Coins { get; set; }
    public int Gems { get; set; }

    public void LoadData()
    {
        // Load data from PlayerPrefs or a file
    }

    public void SaveData()
    {
        // Save data to PlayerPrefs or a file
    }
}
