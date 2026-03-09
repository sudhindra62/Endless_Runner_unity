
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The central nervous system of the game. Manages game state, player lifecycle, and high-level systems.
/// Ensures that all other managers are correctly initialized and coordinated.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public enum GameState { MainMenu, Loading, Playing, Paused, GameOver }

    [Header("Game State")]
    [SerializeField] private GameState currentState = GameState.MainMenu;

    // System Managers
    private UIManager uiManager;
    private ScoreManager scoreManager;
    private PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        // Find all essential managers. This is a crucial step for interconnectivity.
        uiManager = FindObjectOfType<UIManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Start()
    {
        // Based on the scene, determine the initial game state.
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            ChangeState(GameState.MainMenu);
        }
        else
        {
            ChangeState(GameState.Playing);
        }
    }

    /// <summary>
    /// Changes the current game state and performs actions associated with the new state.
    /// </summary>
    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log($"Guardian Architect Log: Game state changed to {currentState}");

        switch (currentState)
        {
            case GameState.MainMenu:
                // Logic for main menu setup
                Time.timeScale = 1f;
                break;
            case GameState.Loading:
                // Show loading screen, load assets, etc.
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                // Start the game
                Time.timeScale = 1f;
                // Find the player in the scene
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) playerController = playerObj.GetComponent<PlayerController>();
                scoreManager?.ResetSession();
                break;
            case GameState.Paused:
                // Pause the game
                Time.timeScale = 0f;
                uiManager?.TogglePauseMenu();
                break;
            case GameState.GameOver:
                // Handle game over logic
                Time.timeScale = 0f;
                scoreManager?.EndSession();
                if (uiManager != null && scoreManager != null)
                {
                    uiManager.ShowGameOverScreen(scoreManager.GetCurrentScore());
                }
                break;
        }
    }

    public void PlayerDied()
    {
        ChangeState(GameState.GameOver);
    }

    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    public GameState GetCurrentState() => currentState;
}
