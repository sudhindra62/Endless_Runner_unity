
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // Example: Player data
    public int Coins { get; set; }
    public int Gems { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadData()
    {
        // Load data from PlayerPrefs or a file
    }

    public void SaveData()
    {
        // Save data to PlayerPrefs or a file
    }
}
