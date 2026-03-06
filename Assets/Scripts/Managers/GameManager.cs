
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Manages the core game loop, including game state, player death, and scene transitions.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public static event Action OnGameStart;
    public static event Action OnGameOver;

    public bool IsGameActive { get; private set; }

    private void Start()
    {
        // This setup assumes the game starts when the scene loads.
        // For a more robust system, you might have a main menu that calls StartGame().
        StartGame();
    }

    public void StartGame()
    {
        if (IsGameActive) return;

        IsGameActive = true;
        Time.timeScale = 1f; // Ensure the game is not paused
        OnGameStart?.Invoke();
        Debug.Log("Game Started!");

        // You could spawn the player here, reset scores, etc.
    }

    public void GameOver()
    {
        if (!IsGameActive) return;

        IsGameActive = false;
        Time.timeScale = 0f; // Pause the game
        OnGameOver?.Invoke();
        Debug.Log("Game Over!");

        // Here you would typically show a game over screen
        // For simplicity, we'll add a delay and restart the level.
        // StartCoroutine(RestartLevelAfterDelay(3f));
    }

    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*
    private System.Collections.IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        RestartGame();
    }
    */

    // Example of a method that might be called by an enemy or a death trigger
    public void PlayerDied()
    {
        GameOver();
    }
}
