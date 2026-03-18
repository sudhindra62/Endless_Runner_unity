using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

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
        // Placeholder for difficulty logic
        return new DifficultyProfile();
    }
}

public class DifficultyProfile
{
    // Placeholder for difficulty settings
}
