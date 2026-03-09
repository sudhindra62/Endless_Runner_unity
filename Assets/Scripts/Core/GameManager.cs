
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

/// <summary>
/// The central nervous system of the game. Manages game state, scene transitions, and coordinates all other managers.
/// Architecturally enhanced by Supreme Guardian Architect v12 to orchestrate the complete player lifecycle, including the tutorial.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    // --- EVENTS ---
    public static event Action<GameState> OnGameStateChanged;

    // --- STATE ---
    public GameState CurrentState { get; private set; }
    private bool _isTutorialCompleted;
    private const string MainSceneName = "MainScene"; // --- ARCHITECTURAL_REFINEMENT: Use a constant for the scene name.

    // --- UNITY LIFECYCLE & SAVE SYSTEM INTEGRATION ---
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // The game always starts in the Main Menu on first launch.
        // We explicitly set a state that doesn't trigger scene loads.
        ChangeState(GameState.MainMenu);
    }

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to critical manager events.
        SaveManager.OnLoad += HandleLoad;
        TutorialManager.OnTutorialComplete += HandleTutorialComplete;
    }

    private void OnDisable()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks.
        SaveManager.OnLoad -= HandleLoad;
        TutorialManager.OnTutorialComplete -= HandleTutorialComplete;
    }

    // --- PUBLIC API for STATE CHANGES ---

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, "Invalid game state specified.");
        }

        OnGameStateChanged?.Invoke(newState);

        // The UIManager is often a direct dependency for state changes.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HandleGameStateChanged(newState);
        }
    }

    // --- PUBLIC API for UI/Button interactions ---

    public void StartGame()
    {
        StartCoroutine(StartGameSequence());
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing) ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused) ChangeState(GameState.Playing);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(StartGameSequence()); // Rerunning the start sequence handles the tutorial check correctly.
    }

    public void QuitGame()
    {
        Debug.Log("Guardian Architect: Application quit requested.");
        Application.Quit();
    }

    // --- PRIVATE WORKFLOWS & HANDLERS ---

    private IEnumerator StartGameSequence()
    {
        Time.timeScale = 1f;

        // Asynchronously load the main game scene to prevent freezing.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MainSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // --- A-TO-Z CONNECTIVITY: Orchestrate the new player experience. ---
        if (!_isTutorialCompleted && TutorialManager.Instance != null)
        {
            Debug.Log("Guardian Architect: New player detected. Starting tutorial sequence.");
            TutorialManager.Instance.StartTutorial();
            // The GameManager will wait for the OnTutorialComplete event before proceeding.
        }
        else
        {
            Debug.Log("Guardian Architect: Returning player. Starting game directly.");
            ChangeState(GameState.Playing);
        }
    }

    private void HandleMainMenu() => Time.timeScale = 1f;
    private void HandlePlaying() => Time.timeScale = 1f;
    private void HandlePaused() => Time.timeScale = 0f;

    private void HandleGameOver()
    {
        Time.timeScale = 1f; // Keep time moving for game over animations.
        if (AdManager.Instance != null) AdManager.Instance.ShowInterstitialAdAfterRun();
    }

    // --- EVENT HANDLERS ---

    private void HandleLoad(SaveData data)
    {
        _isTutorialCompleted = data.TutorialCompleted;
        Debug.Log($"Guardian Architect: Player progression loaded. TutorialCompleted: {_isTutorialCompleted}");
    }

    private void HandleTutorialComplete()
    {
        Debug.Log("Guardian Architect: Tutorial complete signal received. Transitioning to gameplay.");
        ChangeState(GameState.Playing);
    }
}

// Defines the possible states of the game, ensuring a clear and manageable game flow.
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
