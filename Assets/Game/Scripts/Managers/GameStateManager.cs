using UnityEngine;
using EndlessRunner.Data;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public DifficultyProfile[] difficultyProfiles;
    private int _currentDifficultyIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public DifficultyProfile GetCurrentDifficultyProfile()
    {
        if (difficultyProfiles == null || difficultyProfiles.Length == 0)
        {
            Debug.LogWarning("No difficulty profiles have been set in the GameStateManager.");
            return null; // Or return a default profile
        }
        return difficultyProfiles[_currentDifficultyIndex];
    }

    // You could add logic here to progress the difficulty over time.
    public void IncreaseDifficulty()
    {
        if (_currentDifficultyIndex < difficultyProfiles.Length - 1)
        {
            _currentDifficultyIndex++;
        }
    }
}
