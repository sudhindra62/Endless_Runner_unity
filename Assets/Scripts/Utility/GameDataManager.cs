using UnityEngine;

/// <summary>
/// Centralizes and manages all game data persistence to PlayerPrefs.
/// This singleton batches save operations, preventing frequent disk writes during gameplay.
/// Data is saved only when the application is paused or quits.
/// </summary>
public class GameDataManager : Singleton<GameDataManager>
{
    private bool isDataDirty = false;

    protected override void Awake()
    {
        base.Awake();
        // The Singleton pattern ensures this object persists across scene loads.
    }

    /// <summary>
    /// Marks the data as "dirty," indicating that it needs to be saved.
    /// This method does NOT immediately save the data.
    /// </summary>
    public void MarkDataDirty()
    {
        isDataDirty = true;
    }

    /// <summary>
    /// Saves all game data to PlayerPrefs if any changes have been marked.
    /// This is the only place where PlayerPrefs.Save() should be called.
    /// </summary>
    public void SaveAllData()
    {
        if (!isDataDirty) return;

        // In a real project, you would trigger saving methods from other managers here.
        // For now, we just call the explicit save.
        PlayerPrefs.Save();
        isDataDirty = false;
        Debug.Log("GameDataManager: All application data saved to disk.");
    }

    /// <summary>
    /// Called by Unity when the application is paused or loses focus.
    /// This is the primary trigger for saving data to ensure no progress is lost.
    /// </summary>
    /// <param name="pauseStatus">True if the application is paused, false otherwise.</param>
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAllData();
        }
    }

    /// <summary>
    /// Called by Unity when the application is about to quit.
    /// This serves as a final catch-all to ensure data is saved before the app closes.
    /// </summary>
    void OnApplicationQuit()
    {
        SaveAllData();
    }
}
