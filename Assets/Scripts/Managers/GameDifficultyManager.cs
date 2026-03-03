
using UnityEngine;

public class GameDifficultyManager : MonoBehaviour
{
    // EVOLUTION: Kept singleton for global access, but now primarily uses ServiceLocator.
    public static GameDifficultyManager Instance { get; private set; }

    [Header("Difficulty Scaling")]
    [Tooltip("The rate at which difficulty increases over time (units per second).")]
    [SerializeField] private float difficultyIncreaseRate = 0.01f;

    [Tooltip("The maximum difficulty multiplier.")]
    [SerializeField] private float maxDifficultyMultiplier = 3f;

    private float currentDifficultyMultiplier = 1f;
    private float timeElapsed = 0f;

    private void Awake()
    {
        // EVOLUTION: Implement both Singleton and ServiceLocator registration
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        ServiceLocator.Register<GameDifficultyManager>(this);
    }

    private void Start()
    {
        // EVOLUTION: Subscribe to game state changes to reset difficulty
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        // EVOLUTION: Unsubscribe from events and unregister from ServiceLocator
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        ServiceLocator.Unregister<GameDifficultyManager>();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Playing)
        {
            timeElapsed += Time.deltaTime;
            // EVOLUTION: More robust calculation for difficulty ramp
            currentDifficultyMultiplier = 1f + (timeElapsed * difficultyIncreaseRate);
            currentDifficultyMultiplier = Mathf.Min(currentDifficultyMultiplier, maxDifficultyMultiplier);
        }
    }

    /// <summary>
    /// EVOLUTION: Renamed for consistency with existing calls in ObstacleSpawner.
    /// </summary>
    public float GetDifficultyMultiplier()
    {
        return currentDifficultyMultiplier;
    }

    /// <summary>
    /// EVOLUTION: Reset function is now private and called via GameState change.
    /// </summary>
    private void ResetDifficulty()
    {
        timeElapsed = 0f;
        currentDifficultyMultiplier = 1f;
    }

    /// <summary>
    /// EVOLUTION: Handles game state changes to automatically reset the difficulty.
    /// </summary>
    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            ResetDifficulty();
        }
    }
}
