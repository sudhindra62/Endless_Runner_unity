
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the overall game state and flow.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    // --- GAMESTATE ---
    public enum GameState { MainMenu, Playing, Paused, GameOver }
    private GameState currentState;
    public static event Action<GameState> OnGameStateChanged;

    // --- UNITY LIFECYCLE ---
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
    }
    
    private void Start()
    {
        // Set the initial game state based on the active scene
        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            SetState(GameState.MainMenu);
        }
        else
        {
            SetState(GameState.Playing);
        }
    }

    // --- PUBLIC API ---

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
        SetState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            SetState(GameState.Paused);
            Time.timeScale = 0f; // Freeze time
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            SetState(GameState.Playing);
            Time.timeScale = 1f; // Resume time
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SetState(GameState.Playing);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
        SetState(GameState.MainMenu);
    }

    // --- PRIVATE METHODS ---

    private void HandlePlayerDeath()
    {
        SetState(GameState.GameOver);
    }

    private void SetState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}
