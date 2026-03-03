using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private const string SaveKey = "GameSaveData";

    public void SaveBestScore(int bestScore)
    {
        SaveData data = new SaveData { bestScore = bestScore };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Best score saved: " + bestScore);
    }

    public int LoadBestScore()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Best score loaded: " + data.bestScore);
            return data.bestScore;
        }
        return 0; // Default value if no save data is found
    }
}
