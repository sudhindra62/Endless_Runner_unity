using UnityEngine;

/// <summary>
/// Manages the game's pause state and integrates with the GameHUDController's pause button.
/// </summary>
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        // Singleton pattern
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

    private void Start()
    {
        // Subscribe to the pause button's click event
        if (GameHUDController.Instance != null && GameHUDController.Instance.PauseButton != null)
        {
            GameHUDController.Instance.PauseButton.onClick.AddListener(TogglePause);
        }
        else
        {
            Debug.LogError("PauseManager requires an instance of GameHUDController with a valid PauseButton.");
        }
    }

    /// <summary>
    /// Toggles the game between a paused and running state.
    /// </summary>
    public void TogglePause()
    {
        if (IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f; // Freeze game time
        // Additional pause logic (e.g., show pause menu) can be added here
        Debug.Log("Game Paused");
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f; // Resume game time
        // Additional resume logic (e.g., hide pause menu) can be added here
        Debug.Log("Game Resumed");
    }

    private void OnDestroy()
    {
        // Clean up the event listener when the object is destroyed
        if (GameHUDController.Instance != null && GameHUDController.Instance.PauseButton != null)
        {
            GameHUDController.Instance.PauseButton.onClick.RemoveListener(TogglePause);
        }
    }
}
